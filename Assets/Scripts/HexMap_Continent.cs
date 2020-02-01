
using UnityEngine;

public class HexMap_Continent : HexMap
{
    public override void GenerateMap()
    {
        // First make the base map to generate all Hexs (all water)
        base.GenerateMap();
        int numSplats = 5;//Random.Range(4, 8);
        for (int i = 0; i < numSplats; i++)
        {
            int range = Random.Range(5, 8);
            int y = Random.Range(range, numRows - range - 1);
            int x = Random.Range(0, 10) - y / 2 + 20;
            ElevateArea(x, y, range);
        }
        UpdateHexVisuals();
    }

    public void ElevateArea(int q, int r, int range, float centerHeight = 1f)
    {
        Hex centerHex = GetHexAt(q, r);
        Hex[] areaOfHexes = GetHexesWithinRangeOf(centerHex, range);

        foreach (var item in areaOfHexes)
        {
            if (item.Elevation < 0)
                item.Elevation = 0;
            item.Elevation += centerHeight * Mathf.Lerp(1f, 0.1f, Hex.Distance(centerHex, item)) / range;
        }
    }
}
