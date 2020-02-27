using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapObject_Unit : HexMapObject
{
  public HexMapObject_Unit()
  {
    Name = "Default_Name";
  }

  public int Strenth = 8;
  public int Movement = 2;
  public int MovementRemaining = 2;
  public bool CanBuildCities = false;
  public bool SkipThisUnit = false;


  override public void SetHex(Hex newHex)
  {
    base.SetHex(newHex);
  }

  override public void Destroy()
  {
    base.Destroy();
  }
}
