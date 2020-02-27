using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour {
  Vector3 positionOld;

  void Start() {
    positionOld = this.transform.position;
  }

  void Update() {
    CheckIfCameraMoved();
  }

  HexComponent[] hexes;

  void CheckIfCameraMoved() {
    if (positionOld != this.transform.position) {
      // Something moved the camera.
      positionOld = this.transform.position;

      // TODO: Probably HexMap will have a dictionary of all these later
      if (hexes == null)
        hexes = GameObject.FindObjectsOfType<HexComponent>();

      // TODO: Maybe there's a better way to cull what hexes get updated?

      foreach (HexComponent hex in hexes) {
        hex.UpdateHexPosition();
      }
    }
  }
}
