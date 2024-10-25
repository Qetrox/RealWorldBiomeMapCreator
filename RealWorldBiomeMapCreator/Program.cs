using System.Text.Json;
using System.Text.Json.Serialization;
using RealWorldBiomeMapCreator.Biomes;
using RealWorldBiomeMapCreator.Height;
using RealWorldBiomeMapCreator.Tiles;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RealWorldBiomeMapCreator;

public class Program
{
    public static async Task Main(string[] args)
    {
        TileDownloader tileDownloader = new TileDownloader();

        Tuple<int, int> topLeft = new Tuple<int, int>(0, 0);
        Tuple<int, int> botRight = new Tuple<int, int>(63, 63);
        int zoomLevel = 6;

        // Bereken de breedte en hoogte van het gebied
        int width = botRight.Item1 - topLeft.Item1 + 1;
        int height = botRight.Item2 - topLeft.Item2 + 1;

        // Maak een lijst aan om de ultieme biomes op te slaan
        List<Biome> ultimateBiomes = new List<Biome>(width * height);

        // Download en analyseer de tiles in parallel
        var tileTasks = new List<Task<(int x, int y, SateliteTile tile)>>();

        for (int x = topLeft.Item1; x <= botRight.Item1; x++)
        {
            for (int y = topLeft.Item2; y <= botRight.Item2; y++)
            {
                int localX = x;
                int localY = y;

                // Voeg de download- en analyseer-taken toe aan de lijst
                var task = Task.Run(async () =>
                {
                    var _tile = await tileDownloader.DownloadTile(localX, localY, zoomLevel);
                    SateliteTile _sateliteTile = new SateliteTile(_tile, localX, localY, zoomLevel);
                    await _sateliteTile.AnalyzeTile();
                    return (localX, localY, _sateliteTile);
                });

                tileTasks.Add(task);
            }
        }

        List<BiomeData> biomeDataList = new List<BiomeData>();

        // Wacht tot alle taken zijn voltooid en verzamel de resultaten
        var tiles = await Task.WhenAll(tileTasks);

        // Maak een map van gedownloade tiles voor snelle toegang
        var tileMap = tiles.ToDictionary(t => (t.x, t.y), t => t.tile);

        // Bepaal voor elke tile de ultieme biome en sla deze op in de lijst
        for (int y = topLeft.Item2; y <= botRight.Item2; y++)
        {
            for (int x = topLeft.Item1; x <= botRight.Item1; x++)
            {
                // Haal de huidige tile en de omliggende tiles op (kan null zijn als ze niet bestaan)
                var tile = tileMap[(x, y)];
                tileMap.TryGetValue((x, y + 1), out var tileN); // Tile noord
                tileMap.TryGetValue((x + 1, y), out var tileE); // Tile oost
                tileMap.TryGetValue((x, y - 1), out var tileS); // Tile zuid
                tileMap.TryGetValue((x - 1, y), out var tileW); // Tile west

                int _height = await HeightMapper.GetHeight(x, y, zoomLevel);
                
                // Bepaal de ultieme biome voor deze tile
                Biome ultimateBiome = BiomeChooser.UltimateBiomeSingle(tile, tileN, tileE, tileS, tileW);

                // Voeg de ultieme biome toe aan de lijst
                ultimateBiomes.Add(ultimateBiome);
                biomeDataList.Add(new BiomeData(x, y, ultimateBiome));
            }
        }

        List<Biome> ultimateBiomes_ = await OceanMapper.CorrectOceanBiomes(ultimateBiomes, topLeft.Item1, topLeft.Item2, botRight.Item1, botRight.Item2, zoomLevel);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() } // Dit zorgt ervoor dat enums als strings worden opgeslagen
        };
        string json = JsonSerializer.Serialize(ultimateBiomes_, options);
        File.WriteAllText("biomes.json", json);
        Console.WriteLine("Biomes with coordinates have been saved to biomes.json.");

        // Gebruik de visualisatiefunctie om de biomes om te zetten naar een afbeelding
        Image<Rgba32> biomeImage = BiomeVisualizer.CreateBiomeImage(width, height, ultimateBiomes_);

        // Sla de afbeelding op
        biomeImage.Save("output.png");
        Console.WriteLine("De biome map is opgeslagen als 'output.png'.");
    }
}
