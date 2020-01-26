using UnityEngine;

public class HexComponent : MonoBehaviour
{
    public Hex Hex;
    public HexMap HexMap;
    public void UpdateHexPosition()
    {
        this.transform.position = Hex.PositionFromCamera(Camera.main.transform.position, HexMap.numColumns, HexMap.numRows);
    }
}
