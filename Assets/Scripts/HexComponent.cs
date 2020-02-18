using UnityEngine;

public class HexComponent : MonoBehaviour
{
    public Hex Hex;
    public HexMap HexMap;
    public float VerticalOffset = 0; // Map objects on this hex should be rendered higher than usual


    public void UpdateHexPosition()
    {
        transform.position = Hex.PositionFromCamera(Camera.main.transform.position, HexMap.numColumns, HexMap.numRows);
    }
}
