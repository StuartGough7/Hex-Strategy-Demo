﻿using UnityEngine;

public class HexMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }
    public GameObject HexPrefab;
    public Material[] HexMaterials;
    int numRows = 20;
    int numColumns = 40;

    public void GenerateMap()
    {
        for (int column = 0; column < numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                Hex hex = new Hex(column, row);
                //Instantiate Hex Object
                GameObject hexGO = Instantiate(HexPrefab, hex.PositionFromCamera(Camera.main.transform.position, numColumns, numRows), Quaternion.identity, this.transform);
                MeshRenderer hexMR = hexGO.GetComponentInChildren<MeshRenderer>();
                hexMR.material = HexMaterials[Random.Range(0, HexMaterials.Length)];
            }
        }
    }

}
