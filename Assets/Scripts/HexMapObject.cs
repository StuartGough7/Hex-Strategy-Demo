﻿/// <summary>
/// Map Object that exits on a Hex suchas a Unit or building
/// </summary>
public class HexMapObject
{
  public string Name;
  public int HitPoints = 100;
  public bool CanBeAttacked = true;
  public int FactionID = 0;

  public bool IsDestroyed { get; private set; }

  public Hex Hex { get; protected set; }

  public delegate void ObjectMovedDelegate(Hex oldHex, Hex newHex);
  public event ObjectMovedDelegate OnObjectMoved;

  public delegate void ObjectDestroyedDelegate(HexMapObject mapObject);
  public event ObjectDestroyedDelegate OnObjectDestroyed;

  /// <summary>
  /// This object is being removed from the map/game
  /// </summary>
  virtual public void Destroy()
  {
    IsDestroyed = true;

    if (OnObjectDestroyed != null)
    {
      OnObjectDestroyed(this);
    }
  }

  virtual public void SetHex(Hex newHex)
  {
    Hex oldHex = Hex;
    Hex = newHex;

    if (OnObjectMoved != null)
    {
      OnObjectMoved(oldHex, newHex);
    }
  }

}


