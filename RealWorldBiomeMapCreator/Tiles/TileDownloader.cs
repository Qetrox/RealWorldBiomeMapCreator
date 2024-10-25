using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RealWorldBiomeMapCreator.Tiles
{
    public class TileDownloader
    {
        private static readonly HttpClient client = new HttpClient();

        public TileDownloader()
        {
            // Voeg een User-Agent header toe aan elk verzoek, anders krijgen we een 403 error.
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
        }

        // Download de tile of laad deze vanuit een cache
        public async Task<Image<Rgba32>> DownloadTile(int x, int y, int zoom)
        {
            string cacheFile = GetCacheFilePath(x, y, zoom);

            // Probeer de tile uit de cache te laden als deze bestaat
            if (File.Exists(cacheFile))
            {
                Console.WriteLine($"Loading tile from cache: {cacheFile}");
                return await LoadTileFromJsonAsync(cacheFile);
            }

            // Download de tile van de server
            string url = $"https://mt1.google.com/vt/lyrs=s&x={x}&y={y}&z={zoom}";
            Console.WriteLine($"Downloading tile from {url}");
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using (Stream imageStream = await response.Content.ReadAsStreamAsync())
            {
                // Decode de afbeelding direct in een ImageSharp Image
                var image = Image.Load<Rgba32>(imageStream);

                // Sla de tile op in een JSON-cache
                await SaveTileToJsonAsync(image, cacheFile);

                return image;
            }
        }

        // Geeft het pad van het cachebestand voor een specifieke tile
        private string GetCacheFilePath(int x, int y, int zoom)
        {
            string cacheDir = Path.Combine("tile_cache", $"zoom_{zoom}");
            Directory.CreateDirectory(cacheDir);
            return Path.Combine(cacheDir, $"tile_{x}_{y}.json");
        }

        // Sla een afbeelding op in een JSON-bestand als base64
        private async Task SaveTileToJsonAsync(Image<Rgba32> image, string filePath)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Sla de afbeelding op als PNG in het geheugen
                await image.SaveAsPngAsync(memoryStream);
                string base64Image = Convert.ToBase64String(memoryStream.ToArray());

                // Maak een JSON-object
                var json = JsonSerializer.Serialize(new { base64 = base64Image }, new JsonSerializerOptions { WriteIndented = true });

                // Schrijf de JSON naar een bestand
                await File.WriteAllTextAsync(filePath, json);
                Console.WriteLine($"Saved tile to cache: {filePath}");
            }
        }

        // Laad een afbeelding uit een JSON-bestand met base64-encoded data
        private async Task<Image<Rgba32>> LoadTileFromJsonAsync(string filePath)
        {
            string json = await File.ReadAllTextAsync(filePath);
            var data = JsonSerializer.Deserialize<TileJson>(json);

            if (data?.Base64 == null)
                throw new Exception("Invalid JSON file: base64 data is missing.");

            // Decodeer de base64-afbeelding terug naar een afbeelding
            byte[] imageBytes = Convert.FromBase64String(data.Base64);
            using (var memoryStream = new MemoryStream(imageBytes))
            {
                return Image.Load<Rgba32>(memoryStream);
            }
        }

        // Klasse voor het parsen van JSON-data
        private class TileJson
        {
            [JsonPropertyName("base64")]
            public string? Base64 { get; set; }
        }
    }
}
