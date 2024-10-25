using RealWorldBiomeMapCreator.Biomes;
using RealWorldBiomeMapCreator.Height;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealWorldBiomeMapCreator.Tiles
{
    public class SateliteTile
    {
        private readonly Image<Rgba32> imageTile;
        private readonly int worldX;
        private readonly int worldY;
        private readonly int zoomLevel;
        private List<Biome> biomes;

        public SateliteTile(Image<Rgba32> imageSource, int worldX, int worldY, int zoomLevel)
        {
            this.imageTile = imageSource;
            this.worldX = worldX;
            this.worldY = worldY;
            this.zoomLevel = zoomLevel;
            this.biomes = new List<Biome>();
        }

        // Deze methode moet de analyse van de tile uitvoeren en de gemiddelde kleur gebruiken.
        public async Task AnalyzeTile()
        {
            // Haal de gemiddelde kleur van de tile-afbeelding op.
            Rgba32 averageColor = CalculateAverageColor(imageTile);
            Console.WriteLine($"Average color for tile at ({worldX}, {worldY}) is R: {averageColor.R}, G: {averageColor.G}, B: {averageColor.B}");

            // Gebruik de gemiddelde kleur om het biome te bepalen.
            Biome biome = BiomeMapper.DetermineBiome(averageColor);

            List<Biome> possible_ocean = new List<Biome> { Biome.DARK_FOREST };

            // Als de hoogte lager is dan de zeespiegel, zet het biome naar OCEAN.
            int averageHeight = await HeightMapper.GetHeight(worldX, worldY, zoomLevel);
            if (averageHeight < HeightMapper.GetSeaLevel() && biome != Biome.OCEAN && possible_ocean.Contains(biome))
            {
                biome = Biome.OCEAN;
            }

            // Voeg het bepaalde biome toe voor alle pixels in de tile.
            for (int i = 0; i < imageTile.Width * imageTile.Height; i++)
            {
                biomes.Add(biome);
            }

            Console.WriteLine($"Finished analysis for tile ({worldX}, {worldY}) with {biomes.Count} biomes processed as {biome}.");
        }

        // Methode om de gemiddelde kleur van de tile te berekenen.
        private static Rgba32 CalculateAverageColor(Image<Rgba32> image)
        {
            long totalR = 0;
            long totalG = 0;
            long totalB = 0;
            int pixelCount = 0;

            // Loop door alle pixels van de afbeelding.
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Rgba32 pixel = image[x, y]; // Gebruik de indexer om de pixel op te halen.

                    totalR += pixel.R;
                    totalG += pixel.G;
                    totalB += pixel.B;
                    pixelCount++;
                }
            }

            // Bereken het gemiddelde van elke kleurcomponent.
            byte avgR = (byte)(totalR / pixelCount);
            byte avgG = (byte)(totalG / pixelCount);
            byte avgB = (byte)(totalB / pixelCount);

            return new Rgba32(avgR, avgG, avgB);
        }

        // Methode om de biomes van de tile op te halen.
        public Tuple<List<Biome>, int> GetTileData()
        {
            return new Tuple<List<Biome>, int>(biomes, biomes.Count);
        }
    }
}
