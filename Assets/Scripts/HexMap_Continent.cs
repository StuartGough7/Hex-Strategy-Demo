
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap_Continent : HexMap
{
    public override void GenerateMap()
    {
        // First make the base map to generate all Hexs (all water)
        base.GenerateMap();
        int numContinents = 3;
        int continentSpacing = numColumns / numContinents;
        // Random.InitState(0); // this is to seed the random with the same val for repeatabity
        for (int c = 0; c < numContinents; c++)
        {
            int numSplats = Random.Range(4, 8);
            for (int i = 0; i < numSplats; i++)
            {
                int range = Random.Range(5, 8);
                int y = Random.Range(range, numRows - range);
                int x = Random.Range(0, 10) - y / 2 + (c * continentSpacing);
                ElevateArea(x, y, range);
            }
        }

        // Add noise to the elevation 
        float noiseResolution = 0.1f;
        float noiseScale = 2f; // Larger value makes more islands
        Vector2 noiseOffset = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        float perlinNoiseSquareScale = Mathf.Max(numColumns, numRows);

        for (int column = 0; column < numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                Hex hex = GetHexAt(column, row);
                float noiseAdded = Mathf.PerlinNoise(((float)column / perlinNoiseSquareScale / noiseResolution) + noiseOffset.x, ((float)row / perlinNoiseSquareScale / noiseResolution) + noiseOffset.y) - 0.5f; // this is a pseudo random predictable random nbumber btw -0.5 and 0.5 and repeats every whole number based on input ie 0.5f, 1.5f adn 1000.5f will return the same value
                hex.Elevation += noiseAdded * noiseScale;
                // had to cast float else int values would always resolve to 0 or 1
            }
        }

        UpdateHexVisuals();
    }

    public void ElevateArea(int q, int r, int range, float centerHeight = 1f)
    {
        Hex centerHex = GetHexAt(q, r);
        Hex[] areaOfHexes = GetHexesWithinRangeOf(centerHex, range);

        foreach (Hex item in areaOfHexes)
        {
            item.Elevation = centerHeight * Mathf.Lerp(1f, 0.25f, Hex.Distance(centerHex, item) / range);
        }
    }
}
