using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RealWorldBiomeMapCreator.Tiles;

public interface ITile
{
    public Tuple<List<Biomes.Biome>, List<int>> AnalyzeTile();
    public Tuple<List<Biomes.Biome>, List<int>> GetTileData();
}