using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using RealWorldBiomeMapCreator.Biomes;
using RealWorldBiomeMapCreator.util;

namespace RealWorldBiomeMapCreator.Biomes
{
    class OceanMapper
    {
        private static readonly HttpClient client = new HttpClient();

        static OceanMapper()
        {
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
        }

        public static async Task<Image<Rgba32>> DownloadCorrectionTile(int x, int y, int zoom)
        {
            string cacheFile = GetCacheFilePath(x, y, zoom);

            // Controleer of de tile al in de cache staat
            if (File.Exists(cacheFile))
            {
                Console.WriteLine($"[CACHE] Loading correction tile from cache: {cacheFile}");
                return await LoadTileFromCacheAsync(cacheFile);
            }

            string url = $"https://mt1.google.com/vt/lyrs=m&x={x}&y={y}&z={zoom}";
            Console.WriteLine($"[DOWNLOAD] Downloading correction tile from {url}");

            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using (Stream imageStream = await response.Content.ReadAsStreamAsync())
            {
                var image = Image.Load<Rgba32>(imageStream);

                // Sla de tile op in een JSON-cache
                await SaveTileToCacheAsync(image, cacheFile);

                return image;
            }
        }

        private static string GetCacheFilePath(int x, int y, int zoom)
        {
            string cacheDir = Path.Combine("correction_cache", $"zoom_{zoom}");
            Directory.CreateDirectory(cacheDir);
            return Path.Combine(cacheDir, $"correction_{x}_{y}.json");
        }

        private static async Task SaveTileToCacheAsync(Image<Rgba32> image, string filePath)
        {
            using (var memoryStream = new MemoryStream())
            {
                await image.SaveAsPngAsync(memoryStream);
                string base64Image = Convert.ToBase64String(memoryStream.ToArray());

                var json = JsonSerializer.Serialize(new { base64 = base64Image }, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(filePath, json);
            }
        }

        private static async Task<Image<Rgba32>> LoadTileFromCacheAsync(string filePath)
        {
            string json = await File.ReadAllTextAsync(filePath);
            var data = JsonSerializer.Deserialize<TileJson>(json);

            if (data?.Base64 == null)
                throw new Exception("Invalid JSON file: base64 data is missing.");

            byte[] imageBytes = Convert.FromBase64String(data.Base64);
            using (var memoryStream = new MemoryStream(imageBytes))
            {
                return Image.Load<Rgba32>(memoryStream);
            }
        }

        private class TileJson
        {
            [JsonPropertyName("base64")]
            public string? Base64 { get; set; }
        }

        public static async Task<List<Biome>> CorrectOceanBiomes(List<Biome> ultimateBiomes, int topLeftX, int topLeftY, int botRightX, int botRightY, int zoomLevel)
        {
            Console.WriteLine("Starting ocean biome correction process...");

            var tasks = new List<Task>();

            for (int x = topLeftX; x <= botRightX; x++)
            {
                for (int y = topLeftY; y <= botRightY; y++)
                {
                    int localX = x;
                    int localY = y;

                    tasks.Add(Task.Run(async () =>
                    {
                        var correctionTile = await DownloadCorrectionTile(localX, localY, zoomLevel);

                        var averageColor = ColorUtil.CalculateAverageColor(correctionTile);

                        Rgba32 oceanColor = new Rgba32(102, 209, 230, 255);
                        Console.WriteLine($"Average color of tile ({localX}, {localY}): {averageColor}");

                        if (ColorUtil.IsColorCloseTo(averageColor, oceanColor, tolerance: 10))
                        {
                            for (int py = 0; py < correctionTile.Height; py++)
                            {
                                for (int px = 0; px < correctionTile.Width; px++)
                                {
                                    int index = (localY - topLeftY) * correctionTile.Height * (botRightX - topLeftX + 1)
                                                + (localX - topLeftX) * correctionTile.Width + py * correctionTile.Width + px;

                                    if (index < ultimateBiomes.Count && ultimateBiomes[index] != Biome.SNOWY_PLAINS)
                                    {
                                        ultimateBiomes[index] = Biome.SNOWY_PLAINS;
                                    }
                                }
                            }
                            Console.WriteLine($"Marked tile ({localX}, {localY}) as OCEAN based on average color.");
                        }
                    }));
                }
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("Finished ocean biome correction process.");
            return ultimateBiomes;
        }
    }
}
