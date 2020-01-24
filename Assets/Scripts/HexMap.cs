using UnityEngine;

public class HexMap : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HexPrefab;
    public Material[] HexMaterials;
    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                Hex hex = new Hex(col, row);
                //Instantiate Hex Object
                GameObject hexGO = Instantiate(HexPrefab, hex.Position(), Quaternion.identity, this.transform);
                MeshRenderer hexMR = hexGO.GetComponentInChildren<MeshRenderer>();
                hexMR.material = HexMaterials[Random.Range(0, HexMaterials.Length)];
            }
        }
    }

}
