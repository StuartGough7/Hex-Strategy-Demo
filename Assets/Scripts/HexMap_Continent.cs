﻿public class HexMap_Continent : HexMap
{
    public override void GenerateMap()
    {
        // First make the base map to generate all Hexs (all water)
        base.GenerateMap();
        ElevateArea(21, 15, 6);
        ElevateArea(15, 22, 6);
        ElevateArea(22, 6, 6);
        UpdateHexVisuals();
    }

    public void ElevateArea(int q, int r, int range)
    {
        Hex centerHex = GetHexAt(q, r);
        Hex[] areaOfHexes = GetHexesWithinRangeOf(centerHex, range);

        foreach (var item in areaOfHexes)
        {
            item.Elevation = 0.5f;
        }
    }
}
