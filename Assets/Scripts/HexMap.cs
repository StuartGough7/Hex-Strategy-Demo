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


    public int numRows = 20;
    public int numColumns = 40;
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
            x = x % numRows;    //NOTE!! the modulo on x to num rows. This is due to the wrapping west east. ie row -1 will be row numRows -1
        }
        if (allowWrapNorthSouth)
        {
            y = y % numColumns;
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
                hexes[column, row] = hex;

                Vector3 postionFromCamera = hex.PositionFromCamera(Camera.main.transform.position, numColumns, numRows);
                //Instantiate Hex Object
                GameObject hexGO = Instantiate(HexPrefab, postionFromCamera, Quaternion.identity, this.transform);
                hexToGameObjectMap.Add(hex, hexGO); // add the link between hex and gameObject. NOTE! you can add to a directionary with hexToGameObjectMap.Add(hex, hexGO) but this is fine too

                hexGO.GetComponent<HexComponent>().Hex = hex; // Gives the Hex Component script reference to the instantiated hex
                hexGO.GetComponent<HexComponent>().HexMap = this; // Gives the Hex Component script reference to the instantiated hex
                hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0}, {1}", column, row);

                MeshRenderer hexMR = hexGO.GetComponentInChildren<MeshRenderer>();
                hexMR.material = MatOcean;

                MeshFilter hexMF = hexGO.GetComponentInChildren<MeshFilter>();
                hexMF.mesh = MeshWater;

            }
        }
    }

}
