using RealWorldBiomeMapCreator.Biomes;

public class BiomeData
{
    public int X { get; set; }
    public int Y { get; set; }
    public Biome Biome { get; set; }

    public BiomeData(int x, int y, Biome biome)
    {
        X = x;
        Y = y;
        Biome = biome;
    }
}