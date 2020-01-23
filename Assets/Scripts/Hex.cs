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

    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2; // so this maths is only done once as a Hexs width to height ratio doesnt change
    /// <summary>
    /// returns the world space position (vector 3) of the Hex (based on the q,r,s co ordinate system given to the Object)
    /// </summary>
    public Vector3 Position()
    {
        float radius = 1f;
        float height = radius * 2;
        float width = WIDTH_MULTIPLIER * height;

        float verticallSpacing = height * 0.75f; // The amount to offset a neighbouring Hex by verticallly
        float horizontalSpacing = width; // The amount to offset a neighbouring Hex by horizontally

        return new Vector3(horizontalSpacing * (this.Q + this.R / 2f), 0, this.R * verticallSpacing);
    }
}
