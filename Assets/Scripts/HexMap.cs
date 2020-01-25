using UnityEngine;

public class HexMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }
    public GameObject HexPrefab;
    public Material[] HexMaterials;
    int mapHeight = 20;
    int mapWidth = 40;

    public void GenerateMap()
    {
        for (int column = 0; column < mapWidth; column++)
        {
            for (int row = 0; row < mapHeight; row++)
            {
                Hex hex = new Hex(column, row);
                //Instantiate Hex Object
                GameObject hexGO = Instantiate(HexPrefab, hex.Position(), Quaternion.identity, this.transform);
                MeshRenderer hexMR = hexGO.GetComponentInChildren<MeshRenderer>();
                hexMR.material = HexMaterials[Random.Range(0, HexMaterials.Length)];
            }
        }
    }

}
