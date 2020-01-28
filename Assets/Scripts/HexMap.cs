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

    virtual public void GenerateMap()
    {
        // Generate map with ocean
        for (int column = 0; column < numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                Hex hex = new Hex(column, row);
                Vector3 postionFromCamera = hex.PositionFromCamera(Camera.main.transform.position, numColumns, numRows);
                //Instantiate Hex Object
                GameObject hexGO = Instantiate(HexPrefab, postionFromCamera, Quaternion.identity, this.transform);


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
