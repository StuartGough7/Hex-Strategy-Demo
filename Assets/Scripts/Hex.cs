using UnityEngine;

/// <summary>
/// The Hex Class is a pure C# data class which defines the grid position/ world space position / neighbours etc
/// It does not interact with Unity directly but is rather a helper function
/// </summary>

public class Hex
{
    public Hex(int q, int r) // constructor
    {
        this.Q = q;
        this.R = r;
        this.S = -(q + r);
    }

    // Q + R + S = 0 for cubic representation of Hex
    // S = - Q + R
    // readonly means these values can only be assigned once (ie the position of the Hex cannot be changed... Something to consider in Future)
    public readonly int Q; // Column
    public readonly int R; // Row
    public readonly int S; // Sum of Column and row ()

    float radius = 1f;

    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2; // so this maths is only done once as a Hexs width to height ratio doesnt change
    /// <summary>
    /// returns the world space position (vector 3) of the Hex (based on the q,r,s co ordinate system given to the Object)
    /// </summary>
    public Vector3 Position()
    {
        return new Vector3(
            HexHorizontalSpacing() * (this.Q + this.R / 2f),
            0,
            this.R * HexVerticalSpacing()
        );
    }

    public float HexHeight()
    {
        return radius * 2;
    }

    public float HexWidth()
    {
        return WIDTH_MULTIPLIER * HexHeight();
    }

    public float HexVerticalSpacing()
    {
        return HexHeight() * 0.75f; // The amount to offset a neighbouring Hex by verticallly
    }

    public float HexHorizontalSpacing()
    {
        return HexWidth(); // The amount to offset a neighbouring Hex by verticallly
    }


    public Vector3 PositionFromCamera(Vector3 cameraPosition, int numColumns, int numRows)
    {
        float mapHeight = numRows * HexVerticalSpacing();
        float mapWidth = numColumns * HexHorizontalSpacing();

        Vector3 position = Position();

        float widthsFromCameraToHex = (position.x - cameraPosition.x) / mapWidth; // we should always try keep this between -0.5 and 0.5 ie 1 mapWidthcentered on camera always

        if (widthsFromCameraToHex > 0f)
            widthsFromCameraToHex += 0.5f;
        else
            widthsFromCameraToHex -= 0.5f;

        int numWidthsToFix = (int)widthsFromCameraToHex;

        position.x -= numWidthsToFix * mapWidth;

        return position;
    }
}
