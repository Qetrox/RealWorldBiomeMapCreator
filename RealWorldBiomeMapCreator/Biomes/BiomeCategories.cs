using System.Collections.Generic;

namespace RealWorldBiomeMapCreator.Biomes
{
    public static class BiomeCategories
    {
        // Biomes die worden beschouwd als water
        public static readonly HashSet<Biome> WaterBiomes = new HashSet<Biome>
        {
            Biome.OCEAN,
            Biome.RIVER,
            Biome.FROZEN_OCEAN,
            Biome.FROZEN_RIVER,
            Biome.DEEP_OCEAN,
            Biome.WARM_OCEAN,
            Biome.LUKEWARM_OCEAN,
            Biome.COLD_OCEAN,
            Biome.DEEP_LUKEWARM_OCEAN,
            Biome.DEEP_COLD_OCEAN,
            Biome.DEEP_FROZEN_OCEAN
        };

        // Biomes die worden beschouwd als land
        public static readonly HashSet<Biome> LandBiomes = new HashSet<Biome>
        {
            Biome.PLAINS,
            Biome.DESERT,
            Biome.WINDSWEPT_HILLS,
            Biome.FOREST,
            Biome.TAIGA,
            Biome.SWAMP,
            Biome.MANGROVE_SWAMP,
            Biome.NETHER_WASTES,
            Biome.THE_END,
            Biome.SNOWY_PLAINS,
            Biome.MUSHROOM_FIELDS,
            Biome.BEACH,
            Biome.JUNGLE,
            Biome.SPARSE_JUNGLE,
            Biome.STONY_SHORE,
            Biome.SNOWY_BEACH,
            Biome.BIRCH_FOREST,
            Biome.DARK_FOREST,
            Biome.SNOWY_TAIGA,
            Biome.OLD_GROWTH_PINE_TAIGA,
            Biome.WINDSWEPT_FOREST,
            Biome.SAVANNA,
            Biome.SAVANNA_PLATEAU,
            Biome.BADLANDS,
            Biome.WOODED_BADLANDS,
            Biome.SMALL_END_ISLANDS,
            Biome.END_MIDLANDS,
            Biome.END_HIGHLANDS,
            Biome.END_BARRENS,
            Biome.THE_VOID,
            Biome.SUNFLOWER_PLAINS,
            Biome.WINDSWEPT_GRAVELLY_HILLS,
            Biome.FLOWER_FOREST,
            Biome.ICE_SPIKES,
            Biome.OLD_GROWTH_BIRCH_FOREST,
            Biome.OLD_GROWTH_SPRUCE_TAIGA,
            Biome.WINDSWEPT_SAVANNA,
            Biome.ERODED_BADLANDS,
            Biome.BAMBOO_JUNGLE,
            Biome.SOUL_SAND_VALLEY,
            Biome.CRIMSON_FOREST,
            Biome.WARPED_FOREST,
            Biome.BASALT_DELTAS,
            Biome.DRIPSTONE_CAVES,
            Biome.LUSH_CAVES,
            Biome.DEEP_DARK,
            Biome.MEADOW,
            Biome.GROVE,
            Biome.SNOWY_SLOPES,
            Biome.FROZEN_PEAKS,
            Biome.JAGGED_PEAKS,
            Biome.STONY_PEAKS,
            Biome.CHERRY_GROVE
        };
    }
}
