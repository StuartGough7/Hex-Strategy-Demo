using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public Vector3 positionOld;
    // Update is called once per frame
    void Update()
    {
        // @TODO: WSAD move
        // @TODO: mouse click move
        // @TODO: zoom in and out
        CheckIfCameraMoved();
    }

    public void PanToHex()
    {
        // @TODO: pan to tile move
    }

    void CheckIfCameraMoved()
    {
        if (positionOld != this.transform.position)
        {
            // The camera has moved (could be any input)
            positionOld = this.transform.position;
            // @TODO: HexMap will probably ahve a dictionary of all Hexxes at somepopint replace this then (PERFORMANCE HIT)
            HexComponent[] hexxes = FindObjectsOfType<HexComponent>();
            foreach (HexComponent hex in hexxes)
            {
                hex.UpdateHexPosition();
            }
        }
    }

}
