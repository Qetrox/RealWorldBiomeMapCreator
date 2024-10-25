using SixLabors.ImageSharp.PixelFormats;

namespace RealWorldBiomeMapCreator.Biomes;

public static class BiomeMapper
{
    public static Biome DetermineBiome(Rgba32 color)
{
    // Converteer de kleur naar een tuple (we love tuples)
    var targetColor = (color.R, color.G, color.B);

    // Vind de dichtstbijzijnde kleur in de BiomeColorMap op basis van de afstand
    return BiomeColors.BiomeColorMap
        .OrderBy(b => Math.Sqrt(
            Math.Pow(b.Value.r - targetColor.Item1, 2) +
            Math.Pow(b.Value.g - targetColor.Item2, 2) +
            Math.Pow(b.Value.b - targetColor.Item3, 2)
        ))
        .First()
        .Key;
}
}
