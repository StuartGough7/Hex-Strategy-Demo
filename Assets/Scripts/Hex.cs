using System.Collections.Generic;
using UnityEngine;
using QPath;

/// <summary>
/// The Hex Class is a pure C# data class which defines the grid position/ world space position / neighbours etc
/// It does not interact with Unity directly but is rather a helper function
/// </summary>

public class Hex : IQPathTile {
  public Hex(int q, int r, HexMap hexMap) // constructor
  {
    this.Q = q;
    this.R = r;
    this.S = -(q + r);
    this.HexMap = hexMap;
  }

  public object this[string propertyName] {
    get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
    set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
  }

  // Q + R + S = 0 for cubic representation of Hex
  // S = - Q + R
  // readonly means these values can only be assigned once (ie the position of the Hex cannot be changed... Something to consider in Future)
  public readonly int Q;                                                            // Column
  public readonly int R;                                                            // Row
  public readonly int S;                                                            // Sum of Column and row ()
  public readonly HexMap HexMap;

  public enum TERRAIN_TYPE { PLAINS, GRASSLANDS, MARSH, FLOODPLAINS, DESERT, LAKE, OCEAN }
  public enum ELEVATION_TYPE { FLAT, HILL, MOUNTAIN, WATER }

  public TERRAIN_TYPE TerrainType { get; set; }
  public ELEVATION_TYPE ElevationType { get; set; }

  public enum FEATURE_TYPE { NONE, FOREST, RAINFOREST, MARSH }
  public FEATURE_TYPE FeatureType { get; set; }


  public float Elevation { get; set; }
  public float Moisture { get; set; }

  bool allowWrapEastWest = true;
  bool allowWrapNorthSouth = false;

  float radius = 1f;

  static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;                         // so this maths is only done once as a Hexs width to height ratio doesnt change
  /// <summary>
  /// returns the world space position (vector 3) of the Hex (based on the q,r,s co ordinate system given to the Object)
  /// </summary>
  public Vector3 Position() {
    return new Vector3(
        HexHorizontalSpacing() * (this.Q + this.R / 2f),
        0,
        this.R * HexVerticalSpacing()
    );
  }

  public float HexHeight() {
    return radius * 2;
  }

  public float HexWidth() {
    return WIDTH_MULTIPLIER * HexHeight();
  }

  public float HexVerticalSpacing() {
    return HexHeight() * 0.75f; // The amount to offset a neighbouring Hex by verticallly
  }

  public float HexHorizontalSpacing() {
    return HexWidth(); // The amount to offset a neighbouring Hex by verticallly
  }

  //Method overload to call directly from a single Hex
  public Vector3 PositionFromCamera() {
    return HexMap.GetHexPosition(this);
  }

  public Vector3 PositionFromCamera(Vector3 cameraPosition, int numColumns, int numRows) {
    float mapWidth = numColumns * HexHorizontalSpacing();
    float mapHeight = numRows * HexVerticalSpacing();

    Vector3 position = Position();

    if (allowWrapEastWest) {
      float widthsFromCameraToHex = (position.x - cameraPosition.x) / mapWidth; // we should always try keep this between -0.5 and 0.5 ie 1 mapWidthcentered on camera always

      if (widthsFromCameraToHex > 0f)
        widthsFromCameraToHex += 0.5f;
      else
        widthsFromCameraToHex -= 0.5f;

      int numWidthsToFix = (int)widthsFromCameraToHex;

      position.x -= numWidthsToFix * mapWidth;
    }
    if (allowWrapNorthSouth) {
      float heightsFromCameraToHex = (position.z - cameraPosition.z) / mapHeight; // we should always try keep this between -0.5 and 0.5 ie 1 mapWidthcentered on camera always

      if (heightsFromCameraToHex > 0f)
        heightsFromCameraToHex += 0.5f;
      else
        heightsFromCameraToHex -= 0.5f;

      int numHeightssToFix = (int)heightsFromCameraToHex;

      position.z -= numHeightssToFix * mapHeight;
    }

    return position;
  }

  public static float Distance(Hex a, Hex b) {
    //@TODO: Fix for wrapping
    return Mathf.Max(Mathf.Abs(a.Q - b.Q), Mathf.Abs(a.R - b.R), Mathf.Abs(a.S - b.S));
  }

  //----------------------------- Path Queing -----------------------------------------

  Hex[] neighbours;

  public int BaseMovementCost(bool isHillWalker, bool isForestWalker, bool isFlyer) {
    if ((ElevationType == ELEVATION_TYPE.MOUNTAIN || ElevationType == ELEVATION_TYPE.WATER) && isFlyer == false)
      return -99;

    int moveCost = 1;

    if (ElevationType == ELEVATION_TYPE.HILL && isHillWalker == false)
      moveCost += 1;

    if ((FeatureType == FEATURE_TYPE.FOREST || FeatureType == FEATURE_TYPE.RAINFOREST) && isForestWalker == false)
      moveCost += 1;

    return moveCost;
  }

  public static float CostEstimate(IQPathTile aa, IQPathTile bb) {
    return Distance((Hex)aa, (Hex)bb);
  }

  #region IQPathTile implementation
  public IQPathTile[] GetNeighbours() {
    if (this.neighbours != null)
      return this.neighbours;

    List<Hex> neighbours = new List<Hex>();

    neighbours.Add(HexMap.GetHexAt(Q + 1, R + 0));
    neighbours.Add(HexMap.GetHexAt(Q + -1, R + 0));
    neighbours.Add(HexMap.GetHexAt(Q + 0, R + +1));
    neighbours.Add(HexMap.GetHexAt(Q + 0, R + -1));
    neighbours.Add(HexMap.GetHexAt(Q + +1, R + -1));
    neighbours.Add(HexMap.GetHexAt(Q + -1, R + +1));

    List<Hex> neighbours2 = new List<Hex>();

    foreach (Hex h in neighbours) {
      if (h != null) {
        neighbours2.Add(h);
      }
    }

    this.neighbours = neighbours2.ToArray();

    return this.neighbours;
  }

  public float AggregateCostToEnter(float costSoFar, IQPathTile sourceTile, IQPathUnit theUnit) {
    // TODO: We are ignoring source tile right now, this will have to change when we have rivers
    return ((HexMapObject_Unit)theUnit).AggregateTurnsToEnterHex(this, costSoFar); ;
  }

  #endregion

}
