using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }
    public GameObject HexPrefab;


    public Mesh MeshWater;
    public Mesh MeshFlat;
    public Mesh MeshHill;
    public Mesh MeshMountain;

    public Material MatGrassland;
    public Material MatOcean;
    public Material MatPlain;
    public Material MatMountain;


    public int numRows = 30;
    public int numColumns = 60;
    private Hex[,] hexes; // only setable in this class
    private Dictionary<Hex, GameObject> hexToGameObjectMap;

    // @TODO: Link with Hex version for vertical/horizonatl looping
    bool allowWrapEastWest = true;
    bool allowWrapNorthSouth = false;

    public Hex GetHexAt(int x, int y)
    {
        if (hexes == null)
        {
            //throw new UnityException("No Hex array to fetch from"); // This would be be a "loud" exception that will crash the code
            Debug.Log("No Hex array to fetch from");
            return null; //returns early
        }

        if (allowWrapEastWest)
        {
            x = x % numColumns;    //NOTE!! the modulo on x to num rows. This is due to the wrapping west east. ie row -1 will be row numRows -1
            if (x < 0)
            {
                x += numColumns;
            }
        }
        if (allowWrapNorthSouth)
        {
            y = y % numRows;
            if (y < 0)
            {
                y += numRows;
            }
        }

        return hexes[x, y];
    }

    virtual public void GenerateMap()
    {
        hexes = new Hex[numColumns, numRows];
        hexToGameObjectMap = new Dictionary<Hex, GameObject>();
        // Generate map with ocean
        for (int column = 0; column < numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                Hex hex = new Hex(column, row);
                hex.Elevation = -1; // initially all hexxes under water
                hexes[column, row] = hex;

                Vector3 postionFromCamera = hex.PositionFromCamera(Camera.main.transform.position, numColumns, numRows);
                //Instantiate Hex Object
                GameObject hexGO = Instantiate(HexPrefab, postionFromCamera, Quaternion.identity, this.transform);
                hexToGameObjectMap.Add(hex, hexGO); // add the link between hex and gameObject. NOTE! you can add to a directionary with hexToGameObjectMap.Add(hex, hexGO) but this is fine too

                hexGO.GetComponent<HexComponent>().Hex = hex; // Gives the Hex Component script reference to the instantiated hex
                hexGO.GetComponent<HexComponent>().HexMap = this; // Gives the Hex Component script reference to the instantiated hex
                hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0}, {1}", column, row);
            }
        }
        UpdateHexVisuals();
    }

    public void UpdateHexVisuals()
    {
        for (int column = 0; column < numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                Hex hex = hexes[column, row];
                GameObject hexGO = hexToGameObjectMap[hex];

                MeshRenderer hexMR = hexGO.GetComponentInChildren<MeshRenderer>();

                if (hex.Elevation >= 0)
                {
                    hexMR.material = MatGrassland;
                }
                else
                {
                    hexMR.material = MatOcean;
                }

                MeshFilter hexMF = hexGO.GetComponentInChildren<MeshFilter>();
                hexMF.mesh = MeshWater;
            }
        }

    }

    public Hex[] GetHexesWithinRangeOf(Hex centerHex, int range)
    {
        List<Hex> results = new List<Hex>();
        for (int dx = -range; dx <= range; dx++)
        {
            for (int dy = Mathf.Max(-range, -dx - range); dy <= Mathf.Min(range, -dx + range); dy++)
            {
                results.Add(GetHexAt(centerHex.Q + dx, centerHex.R + dy));
            }
        }
        return results.ToArray();
    }
}