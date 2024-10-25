using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RealWorldBiomeMapCreator.Height
{
    public static class HeightMapper
    {
        private static int minheight = -64;
        private static int maxheight = 384;
        private static int sealevel = 62;

        private static readonly HttpClient client = new HttpClient();

        public static async Task<int> GetHeight(int x, int y, int zoom)
        {
            string cacheFile = GetCacheFilePath(x, y, zoom);

            // Controleer of de hoogte-informatie al is gecachet.
            if (File.Exists(cacheFile))
            {
                return await LoadHeightFromCacheAsync(cacheFile);
            }

            // Download de hoogtekaart als deze niet in de cache staat.
            string url = $"https://mt1.google.com/vt/lyrs=t&x={x}&y={y}&z={zoom}";
            Console.WriteLine($"[DOWNLOAD] Attempting to download heightmap from: {url}");

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

            try
            {
                HttpResponseMessage response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Error fetching data. Status code: {response.StatusCode}");
                }

                using (Stream imageStream = await response.Content.ReadAsStreamAsync())
                {
                    var image = Image.Load<Rgba32>(imageStream);
                    double normalizedHeight = CalculateAverageGrayValue(image);

                    // Schaal de genormaliseerde hoogte naar de gewenste schaal.
                    int height = GetHeightFromNormalized(normalizedHeight);

                    // Sla de hoogte-informatie op in de cache.
                    await SaveHeightToCacheAsync(height, cacheFile);

                    return height;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Geeft het pad van het cachebestand voor een specifieke tile
        private static string GetCacheFilePath(int x, int y, int zoom)
        {
            string cacheDir = Path.Combine("height_cache", $"zoom_{zoom}");
            Directory.CreateDirectory(cacheDir);
            return Path.Combine(cacheDir, $"height_{x}_{y}.json");
        }

        // Sla de hoogte op in een JSON-bestand.
        private static async Task SaveHeightToCacheAsync(int height, string filePath)
        {
            try
            {
                var json = JsonSerializer.Serialize(new { height }, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(filePath, json);
                //Console.WriteLine($"[CACHE] Saved height data to cache: {filePath}");
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"[ERROR] Failed to save height data to cache: {filePath}. Error: {ex.Message}");
            }
        }

        // Laad de hoogte uit een gecachet JSON-bestand.
        private static async Task<int> LoadHeightFromCacheAsync(string filePath)
        {
            try
            {
                string json = await File.ReadAllTextAsync(filePath);
                var data = JsonSerializer.Deserialize<HeightData>(json);
                if (data != null)
                {
                    //Console.WriteLine($"[CACHE] Loaded height {data.Height} from cache: {filePath}");
                    return validateHeight(data.Height);
                }

                throw new Exception("Deserialized data was null.");
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"[ERROR] Failed to load height data from cache: {filePath}. Error: {ex.Message}");
                throw;
            }
        }

        // Model class voor het parsen van JSON-data.
        private class HeightData
        {
            public int Height { get; set; }
        }

        // Methode om de gemiddelde hoogte te berekenen uit een hoogtemap-afbeelding.
        private static double CalculateAverageGrayValue(Image<Rgba32> image)
        {
            long totalGray = 0;
            int pixelCount = 0;

            // Loop door alle pixels van de afbeelding.
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Rgba32 pixel = image[x, y];

                    // Bereken de grijswaarde van de pixel.
                    int grayValue = (pixel.R + pixel.G + pixel.B) / 3;

                    // Voeg de grijswaarde toe aan het totaal.
                    totalGray += grayValue;
                    pixelCount++;
                }
            }

            // Bereken de gemiddelde grijswaarde.
            double averageGrayValue = (double)totalGray / pixelCount;

            // Log de gemiddelde grijswaarde voor debugging.
            Console.WriteLine($"[DEBUG] Average gray value for height calculation: {averageGrayValue}");

            // Normaliseer de gemiddelde grijswaarde tussen 0 en 1.
            return averageGrayValue / 255.0;
        }

        public static int GetHeightFromNormalized(double normalizedHeight)
        {
            // Schaal de genormaliseerde hoogte zodat 0 overeenkomt met `sealevel` en 1 met `maxheight`.
            int height = (int)(normalizedHeight * (maxheight - sealevel) + sealevel);

            // Log de berekende hoogte voor debugging.
            Console.WriteLine($"[DEBUG] Normalized height: {normalizedHeight}, Calculated height: {height}");

            return height;
        }

        public static int validateHeight(int height)
        {
            if (height < minheight) return minheight;
            if (height > maxheight) return maxheight;

            return height;
        }

        public static int GetMinHeight() => minheight;
        public static int GetMaxHeight() => maxheight;
        public static int GetSeaLevel() => sealevel;
    }
}
