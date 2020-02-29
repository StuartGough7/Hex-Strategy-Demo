using System.Collections.Generic;
using UnityEngine;
using QPath;

public class HexMapObject_Unit : HexMapObject, IQPathUnit {

  public HexMapObject_Unit() {
    Name = "Default_Name";
  }

  public int Strenth = 8;
  public int Movement = 2;
  public int MovementRemaining = 2;
  public bool CanBuildCities = false;
  public bool SkipThisUnit = false;

  List<Hex> hexPath;                                                                // List of hexes to walk through (from pathfinder). NOTE: First item is always the hex we are standing in.
  const bool MOVEMENT_RULES_LIKE_CIV6 = true;                                       // TODO: This should probably be moved to some kind of central option/config file


  public void DUMMY_PATHING_FUNCTION() {
    Hex[] pathHexes = QPath.QPath.FindPath<Hex>(
        Hex.HexMap,
        this,
        Hex,
        Hex.HexMap.GetHexAt(Hex.Q, Hex.R + 4),
        Hex.CostEstimate
    );
    Debug.Log("Got pathfinding path of length: " + pathHexes.Length);
    SetHexPath(pathHexes);
  }

  override public void SetHex(Hex newHex) {
    // if (Hex != null) {
    //   Hex.RemoveUnit(this);
    // }
    base.SetHex(newHex);
    // Hex.AddUnit(this);
  }

  override public void Destroy() {
    base.Destroy();
  }

  public bool DoMove() {
    Debug.Log("DoMove");
    // Do queued move

    if (MovementRemaining <= 0)
      return false;

    if (hexPath == null || hexPath.Count == 0) {
      return false;
    }

    // Grab the first hex from our queue
    Hex hexWeAreLeaving = hexPath[0];
    Hex newHex = hexPath[1];

    int costToEnter = MovementCostToEnterHex(newHex);

    if (costToEnter > MovementRemaining && MovementRemaining < Movement && MOVEMENT_RULES_LIKE_CIV6) {
      return false; // We can't enter the hex this turn
    }

    hexPath.RemoveAt(0);

    if (hexPath.Count == 1) {
      hexPath = null; // The only hex left in the list, is the one we are moving to thefore clear path 
    }

    // Move to the new Hex
    SetHex(newHex);
    MovementRemaining = Mathf.Max(MovementRemaining - costToEnter, 0);

    return hexPath != null && MovementRemaining > 0;
  }

  public void RefreshMovement() {
    SkipThisUnit = false;
    MovementRemaining = Movement;
  }

  public int MovementCostToEnterHex(Hex hex) {
    // TODO:  Implement different movement traits

    return hex.BaseMovementCost(false, false, false);
  }

  public void ClearHexPath() {
    SkipThisUnit = false;
    this.hexPath = new List<Hex>();
  }

  public void SetHexPath(Hex[] hexArray) {
    SkipThisUnit = false;
    this.hexPath = new List<Hex>(hexArray);
  }

  public float CostToEnterHex(IQPathTile sourceTile, IQPathTile destinationTile) {
    return 1;                             // Turn cost to enter a hex (i.e. 0.5 turns if a movement cost is 1 and we have 2 max movement)
  }

  public float AggregateTurnsToEnterHex(Hex hex, float turnsToDate) {
    float baseTurnsToEnterHex = MovementCostToEnterHex(hex) / Movement; // Example: Entering a forest is "1" turn

    if (baseTurnsToEnterHex < 0) {
      // Impassible terrain
      //Debug.Log("Impassible terrain at:" + hex.ToString());
      return -99999;
    }

    if (baseTurnsToEnterHex > 1) {
      // Even if something costs 3 to enter and we have a max move of 2, 
      // you can always enter it using a full turn of movement.
      baseTurnsToEnterHex = 1;
    }


    float turnsRemaining = MovementRemaining / Movement;    // Example, if we are at 1/2 move, then we have .5 turns left

    float turnsToDateWhole = Mathf.Floor(turnsToDate); // Example: 4.33 becomes 4
    float turnsToDateFraction = turnsToDate - turnsToDateWhole; // Example: 4.33 becomes 0.33

    if ((turnsToDateFraction > 0 && turnsToDateFraction < 0.01f) || turnsToDateFraction > 0.99f) {
      Debug.LogError("Looks like we've got floating-point drift: " + turnsToDate);

      if (turnsToDateFraction < 0.01f)
        turnsToDateFraction = 0;

      if (turnsToDateFraction > 0.99f) {
        turnsToDateWhole += 1;
        turnsToDateFraction = 0;
      }
    }

    float turnsUsedAfterThismove = turnsToDateFraction + baseTurnsToEnterHex; // Example 0.33 + 1

    if (turnsUsedAfterThismove > 1) {
      // We have hit the situation where we don't actually have enough movement to complete this move.
      // What do we do?

      // Civ5-style movement state that we can always enter a tile, even if we don't
      // have enough movement left.
      turnsUsedAfterThismove = 1;
    }

    // turnsUsedAfterThismove is now some value from 0..1. (this includes
    // the factional part of moves from previous turns).


    // Do we return the number of turns THIS move is going to take?
    // I say no, this an an "aggregate" function, so return the total
    // turn cost of turnsToDate + turns for this move.

    return turnsToDateWhole + turnsUsedAfterThismove;

  }
}
