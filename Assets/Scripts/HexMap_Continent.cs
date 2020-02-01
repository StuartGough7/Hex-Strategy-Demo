
using UnityEngine;

public class HexMap_Continent : HexMap
{
    public override void GenerateMap()
    {
        // First make the base map to generate all Hexs (all water)
        base.GenerateMap();
        int numContinents = 2;
        int continentSpacing = 20;

        for (int c = 0; c < numContinents; c++)
        {
            int numSplats = Random.Range(4, 8);
            for (int i = 0; i < numSplats; i++)
            {
                int range = Random.Range(5, 8);
                int y = Random.Range(range, numRows - range - 1);
                int x = Random.Range(0, 10) - y / 2 + (c * continentSpacing);
                ElevateArea(x, y, range);
            }
        }

        UpdateHexVisuals();
    }

    public void ElevateArea(int q, int r, int range, float centerHeight = 1f)
    {
        Hex centerHex = GetHexAt(q, r);
        Hex[] areaOfHexes = GetHexesWithinRangeOf(centerHex, range);

        foreach (var item in areaOfHexes)
        {
            item.Elevation += centerHeight * Mathf.Lerp(1f, 0.25f, Hex.Distance(centerHex, item) / range);
        }
    }
}
