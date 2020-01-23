using System.Collections;
using System.Collections.Generic;
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
                //Instantiate Hex Object
                Instantiate(HexPrefab, new Vector3(col, 0, row), Quaternion.identity);
            }
        }
    }

}
