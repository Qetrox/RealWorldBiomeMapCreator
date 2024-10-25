using System;
using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using RealWorldBiomeMapCreator.Biomes;

namespace RealWorldBiomeMapCreator.Biomes;

public class BiomeVisualizer
{
    public static Image<Rgba32> CreateBiomeImage(int width, int height, List<Biome> biomes)
    {

        Console.WriteLine("Creating biome image...");

        // Controleer of de lijst van biomes overeenkomt met de afmetingen
        if (biomes.Count != width * height)
        {
            throw new ArgumentException("De lengte van de biomes-lijst komt niet overeen met de opgegeven breedte en hoogte.");
        }

        // Maak een nieuwe afbeelding aan met de opgegeven breedte en hoogte
        var image = new Image<Rgba32>(width, height);

        // Krijg de kleuren uit de BiomeColorMap voor visualisatie
        var biomeColorMap = BiomeColors.BiomeColorMap;

        // Loop door de pixels en stel de kleur in op basis van de biomes
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Bereken de index in de lijst van biomes
                int index = y * width + x;
                Biome biome = biomes[index];

                // Haal de RGB-kleur op van de huidige biome
                if (biomeColorMap.TryGetValue(biome, out var color))
                {
                    // Stel de pixelkleur in op basis van de RGB-waarde van de biome
                    image[x, y] = new Rgba32((byte)color.r, (byte)color.g, (byte)color.b, 255);
                }
                else
                {
                    // Stel een standaardkleur in als de biome niet in de map staat (bijvoorbeeld zwart)
                    image[x, y] = new Rgba32(0, 0, 0);
                }
            }
        }

        return image;
    }
}
