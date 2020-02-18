using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    public GameObject HexPrefab;
    public GameObject ForestPrefab;
    public GameObject JunglePrefab;


    public Mesh MeshWater;
    public Mesh MeshFlat;
    public Mesh MeshHill;
    public Mesh MeshMountain;

    public Material MatDesert;
    public Material MatOcean;
    public Material MatPlain;
    public Material MatGrassland;
    public Material MatMountain;
    public GameObject UnitDwarfPrefab;
    HexMapObject_Unit Dwarf = new HexMapObject_Unit();


    // Tiles with height above x is given its appropriate mesh y
    // Serializing is assigns the variable in memory (basically you can control the variable in the inspector which can lead to confusion, this "hides" it from the inspector 
    // (hiding would actually still not work as the value is still saved as whatever it was when the script was compiled))
    [System.NonSerialized] public float HeightMountain = 1f;
    [System.NonSerialized] public float HeightHill = 0.6f;
    [System.NonSerialized] public float HeightFlat = 0f;
    [System.NonSerialized] public float MoistureJungle = 1f;
    [System.NonSerialized] public float MoistureForest = 0.3f;
    [System.NonSerialized] public float MoistureGrasslands = 0f;
    [System.NonSerialized] public float MoisturePlains = -0.75f;


    [System.NonSerialized] public int numRows = 30;
    [System.NonSerialized] public int numColumns = 60;
    private Hex[,] hexes; // only setable in this class
    private Dictionary<Hex, GameObject> hexToGameObjectMap;
    private Dictionary<HexMapObject_Unit, GameObject> unitToGameObjectMap;
    private Dictionary<GameObject, Hex> gameObjectToHexMap;

    // @TODO: Link with Hex version for vertical/horizontal looping
    bool allowWrapEastWest = true;
    bool allowWrapNorthSouth = false;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Move time");
            Hex toMoveto = new Hex(29, 13, this);
            Dwarf.SetHex(toMoveto);
        }
    }
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

        if (x < 0 || y < 0)
        {
            return null; // This is to retrieve the relevant area of the map even if there is overflow out of the map
        }

        return hexes[x, y];
    }

    public Vector3 GetHexPosition(int q, int r)
    {
        Hex hex = GetHexAt(q, r);
        return GetHexPosition(hex);
    }

    public Vector3 GetHexPosition(Hex hex)
    {
        return hex.PositionFromCamera(Camera.main.transform.position, numRows, numColumns);
    }

    public Hex GetHexFromGameObject(GameObject hexGO)
    {
        if (gameObjectToHexMap.ContainsKey(hexGO))
        {
            return gameObjectToHexMap[hexGO];
        }
        return null;
    }

    public GameObject GetHexGO(Hex h)
    {
        if (hexToGameObjectMap.ContainsKey(h))
        {
            return hexToGameObjectMap[h];
        }
        return null;
    }

    public GameObject GetUnitGO(HexMapObject_Unit c)
    {
        if (unitToGameObjectMap.ContainsKey(c))
        {
            return unitToGameObjectMap[c];
        }
        return null;
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
                Hex hex = new Hex(column, row, this); // This is passing the HexMap to the Hex so it is aware of certain Map parameters
                hex.Elevation = -0.5f; // initially all hexxes under water
                hexes[column, row] = hex;

                Vector3 postionFromCamera = hex.PositionFromCamera(Camera.main.transform.position, numColumns, numRows);
                //Instantiate Hex Object
                GameObject hexGO = Instantiate(HexPrefab, postionFromCamera, Quaternion.identity, this.transform);
                hexGO.name = string.Format("Hex {0}, {1}", column, row); // naming the object in the Heirachy
                hexToGameObjectMap.Add(hex, hexGO); // add the link between hex and gameObject. NOTE! you can add to a directionary with hexToGameObjectMap.Add(hex, hexGO) but this is fine too

                hexGO.GetComponent<HexComponent>().Hex = hex; // Gives the Hex Component script reference to the instantiated hex
                hexGO.GetComponent<HexComponent>().HexMap = this; // Gives the Hex Component script reference to the instantiated hex
                hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0}, {1}", column, row);
            }
        }
        UpdateHexVisuals();
        SpawnUnitAt(Dwarf, UnitDwarfPrefab, 28, 13);
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
                MeshFilter hexMF = hexGO.GetComponentInChildren<MeshFilter>();

                if (hex.Elevation >= HeightFlat && hex.Elevation < HeightMountain)
                {
                    if (hex.Moisture >= MoistureJungle)
                    {
                        hexMR.material = MatGrassland;

                        // Spawn trees
                        Vector3 p = hexGO.transform.position;
                        if (hex.Elevation >= HeightHill)
                        {
                            p.y += 0.25f;
                        }

                    }
                    else if (hex.Moisture >= MoistureForest)
                    {
                        hexMR.material = MatGrassland;

                        // Spawn trees
                        Vector3 p = hexGO.transform.position;
                        if (hex.Elevation >= HeightHill)
                        {
                            p.y += 0.25f;
                        }
                        Instantiate(ForestPrefab, p, Quaternion.identity, hexGO.transform);
                    }
                    else if (hex.Moisture >= MoistureGrasslands)
                    {
                        hexMR.material = MatGrassland;
                    }
                    else if (hex.Moisture >= MoisturePlains)
                    {
                        hexMR.material = MatPlain;
                    }
                    else
                    {
                        hexMR.material = MatDesert;
                    }
                }

                if (hex.Elevation >= HeightMountain)
                {
                    hexMR.material = MatMountain;
                    hexMF.mesh = MeshMountain;
                }
                else if (hex.Elevation >= HeightHill)
                {
                    hexMF.mesh = MeshHill;
                }
                else if (hex.Elevation >= HeightFlat)
                {
                    hexMF.mesh = MeshFlat;
                }
                else
                {
                    hexMR.material = MatOcean;
                    hexMF.mesh = MeshWater;
                }

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
                Hex retrievedHex = GetHexAt(centerHex.Q + dx, centerHex.R + dy);
                if (retrievedHex != null)
                {
                    results.Add(retrievedHex);
                }
            }
        }
        return results.ToArray();
    }

    public void SpawnUnitAt(HexMapObject_Unit unit, GameObject prefab, int q, int r)
    {
        if (unitToGameObjectMap == null)
        {
            unitToGameObjectMap = new Dictionary<HexMapObject_Unit, GameObject>();
        }

        Hex hexToSpawnAt = GetHexAt(q, r);
        GameObject hexToSpawnAtGO = hexToGameObjectMap[hexToSpawnAt];
        GameObject unitGO = Instantiate(prefab, hexToSpawnAtGO.transform.position, Quaternion.identity, hexToSpawnAtGO.transform);

        unit.SetHex(hexToSpawnAt);
        unit.OnObjectMoved += unitGO.GetComponent<HexMapObject_UnitVisuals>().OnUnitMoved;

        //CurrentPlayer.AddUnit(unit);
        //unit.OnObjectDestroyed += OnUnitDestroyed;
        //unitToGameObjectMap.Add(unit, unitGO);
    }

}