public class HexMap_Continent : HexMap
{
    public override void GenerateMap()
    {
        // First make the base map to generate all Hexs (all water)
        base.GenerateMap();
        ElevateArea(21, 16, 4);
        UpdateHexVisuals();
    }

    public void ElevateArea(int q, int r, int radius)
    {
        Hex centerHex = GetHexAt(q, r);
        centerHex.Elevation = 0.5f;
    }
}
