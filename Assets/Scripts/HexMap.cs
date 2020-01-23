using UnityEngine;

public class HexMap : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HexPrefab;
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
                Instantiate(HexPrefab, hex.Position(), Quaternion.identity);
            }
        }
    }

}
