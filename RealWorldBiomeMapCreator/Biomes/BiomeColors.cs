using System.Collections.Generic;
using RealWorldBiomeMapCreator.Biomes;

namespace RealWorldBiomeMapCreator.Biomes
{
    public class BiomeColors
    {
        public static Dictionary<Biome, (int r, int g, int b)> BiomeColorMap = new Dictionary<Biome, (int r, int g, int b)>
        {
            { Biome.OCEAN, (0, 0, 255) },
            { Biome.PLAINS, (124, 252, 0) },
            { Biome.DESERT, (238, 221, 130) },
            { Biome.WINDSWEPT_HILLS, (169, 169, 169) },
            { Biome.FOREST, (34, 139, 34) },
            { Biome.TAIGA, (0, 100, 0) },
            { Biome.SWAMP, (85, 107, 47) },
            { Biome.MANGROVE_SWAMP, (46, 139, 87) },
            { Biome.RIVER, (70, 130, 180) },
            { Biome.FROZEN_OCEAN, (176, 224, 230) },
            { Biome.FROZEN_RIVER, (175, 238, 238) },
            { Biome.SNOWY_PLAINS, (255, 255, 255) },
            { Biome.BEACH, (255, 245, 186) },
            { Biome.JUNGLE, (0, 100, 0) },
            { Biome.SPARSE_JUNGLE, (34, 139, 34) },
            { Biome.DEEP_OCEAN, (0, 0, 128) },
            { Biome.STONY_SHORE, (128, 128, 128) },
            { Biome.SNOWY_BEACH, (240, 255, 255) },
            { Biome.BIRCH_FOREST, (152, 251, 152) },
            { Biome.DARK_FOREST, (1, 50, 32) },
            { Biome.SNOWY_TAIGA, (169, 223, 191) },
            { Biome.OLD_GROWTH_PINE_TAIGA, (85, 107, 47) },
            { Biome.WINDSWEPT_FOREST, (70, 130, 180) },
            { Biome.SAVANNA, (244, 164, 96) },
            { Biome.SAVANNA_PLATEAU, (210, 180, 140) },
            { Biome.BADLANDS, (205, 133, 63) },
            { Biome.WOODED_BADLANDS, (139, 69, 19) },
            { Biome.WARM_OCEAN, (0, 191, 255) },
            { Biome.LUKEWARM_OCEAN, (70, 130, 180) },
            { Biome.COLD_OCEAN, (100, 149, 237) },
            { Biome.DEEP_LUKEWARM_OCEAN, (65, 105, 225) },
            { Biome.DEEP_COLD_OCEAN, (0, 0, 139) },
            { Biome.DEEP_FROZEN_OCEAN, (176, 224, 230) },
            { Biome.SUNFLOWER_PLAINS, (255, 215, 0) },
            { Biome.WINDSWEPT_GRAVELLY_HILLS, (172, 172, 172) },
            { Biome.FLOWER_FOREST, (255, 105, 180) },
            { Biome.ICE_SPIKES, (173, 216, 230) },
            { Biome.OLD_GROWTH_BIRCH_FOREST, (144, 238, 144) },
            { Biome.OLD_GROWTH_SPRUCE_TAIGA, (0, 100, 0) },
            { Biome.WINDSWEPT_SAVANNA, (222, 184, 135) },
            { Biome.ERODED_BADLANDS, (210, 105, 30) },
            { Biome.BAMBOO_JUNGLE, (50, 205, 50) },
            { Biome.MEADOW, (173, 255, 47) },
            { Biome.GROVE, (0, 100, 0) },
            { Biome.SNOWY_SLOPES, (238, 238, 238) },
            { Biome.FROZEN_PEAKS, (224, 255, 255) },
            { Biome.JAGGED_PEAKS, (211, 211, 211) },
            { Biome.STONY_PEAKS, (170, 172, 170) },
            { Biome.CHERRY_GROVE, (255, 182, 193) }
        };
    }
}