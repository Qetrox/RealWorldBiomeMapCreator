using RealWorldBiomeMapCreator.Biomes;
using RealWorldBiomeMapCreator.Tiles;
using System.Collections.Generic;
using System.Linq;

class BiomeChooser
{

    // Bepaal de meest voorkomende biome uit een lijst
    public static Biome GetMostCommonBiome(List<Biome> biomes)
    {
        return biomes
            .GroupBy(b => b)
            .OrderByDescending(g => g.Count())
            .First()
            .Key;
    }

    // Bepaal de ultieme biome voor een tile op basis van de biomes van de huidige tile en de omliggende tiles (liever niet gebruiken)
    public static Biome UltimateBiome(SateliteTile tile, SateliteTile? tileN = null, SateliteTile? tileE = null, SateliteTile? tileS = null, SateliteTile? tileW = null)
    {
        // Combineer de biomes van de huidige tile en de omliggende tiles.
        // Omliggende tiles kunnen null zijn als ze niet bestaan

        var combinedBiomes = new List<Biome>();

        // Voeg de biomes van de huidige tile toe
        combinedBiomes.AddRange(tile.GetTileData().Item1);

        // Voeg de biomes van omliggende tiles toe, alleen als ze niet null zijn
        if (tileN != null)
        {
            combinedBiomes.Add(GetMostCommonBiome(tileN.GetTileData().Item1));
        }

        if (tileE != null)
        {
            combinedBiomes.Add(GetMostCommonBiome(tileE.GetTileData().Item1));
        }

        if (tileS != null)
        {
            combinedBiomes.Add(GetMostCommonBiome(tileS.GetTileData().Item1));
        }

        if (tileW != null)
        {
            combinedBiomes.Add(GetMostCommonBiome(tileW.GetTileData().Item1));
        }

        // Bepaal de meest voorkomende biome uit alle verzamelde biomes
        Biome mostCommonBiome = GetMostCommonBiome(combinedBiomes);

        // Speciale logica voor rivieren: controleer of de rivier ingesloten is door land
        if (mostCommonBiome == Biome.RIVER)
        {
            // Controleer de omliggende tiles op land-biomes
            bool isNorthLand = tileN != null && BiomeCategories.LandBiomes.Contains(GetMostCommonBiome(tileN.GetTileData().Item1));
            bool isSouthLand = tileS != null && BiomeCategories.LandBiomes.Contains(GetMostCommonBiome(tileS.GetTileData().Item1));
            bool isEastLand = tileE != null && BiomeCategories.LandBiomes.Contains(GetMostCommonBiome(tileE.GetTileData().Item1));
            bool isWestLand = tileW != null && BiomeCategories.LandBiomes.Contains(GetMostCommonBiome(tileW.GetTileData().Item1));

            // Als de rivier niet ingesloten is door land aan beide zijden (noord/zuid of oost/west), maak er dan zee van
            if (!(isNorthLand && isSouthLand) && !(isEastLand && isWestLand))
            {
                return Biome.OCEAN;
            }
        }

        return mostCommonBiome;
    }

    // Bepaal de ultieme biome voor een tile op basis van de biomes van de huidige tile (liever gebruiken)
    public static Biome UltimateBiomeSingle(SateliteTile tile, SateliteTile? tileN = null, SateliteTile? tileE = null, SateliteTile? tileS = null, SateliteTile? tileW = null)
    {
        Biome mostCommonBiome = GetMostCommonBiome(tile.GetTileData().Item1);

        // Speciale logica voor rivieren: controleer of de rivier ingesloten is door land
        if (mostCommonBiome == Biome.RIVER)
        {
            // Controleer de omliggende tiles op land-biomes
            bool isNorthLand = tileN != null && BiomeCategories.LandBiomes.Contains(GetMostCommonBiome(tileN.GetTileData().Item1));
            bool isSouthLand = tileS != null && BiomeCategories.LandBiomes.Contains(GetMostCommonBiome(tileS.GetTileData().Item1));
            bool isEastLand = tileE != null && BiomeCategories.LandBiomes.Contains(GetMostCommonBiome(tileE.GetTileData().Item1));
            bool isWestLand = tileW != null && BiomeCategories.LandBiomes.Contains(GetMostCommonBiome(tileW.GetTileData().Item1));

            // Als de rivier niet ingesloten is door land aan beide zijden (noord/zuid of oost/west), maak er dan zee van
            if (!(isNorthLand && isSouthLand) && !(isEastLand && isWestLand))
            {
                return Biome.OCEAN;
            }
        }

        return mostCommonBiome;

    }
}
