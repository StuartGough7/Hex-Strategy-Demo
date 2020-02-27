using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
  // Use this for initialization
  void Start()
  {
    Update_CurrentFunc = Update_DetectModeStart;
  }

  // Generic bookkeeping variables
  Vector3 lastMousePosition;  // From Input.mousePosition

  // Camera Dragging bookkeeping variables
  int mouseDragThreshold = 1; // Threshold of mouse movement to start a drag
  Vector3 lastMouseGroundPlanePosition;
  Vector3 cameraTargetOffset;
  delegate void UpdateFunc();
  UpdateFunc Update_CurrentFunc;

  void Update()
  {
    Update_CurrentFunc();
    Update_ScrollZoom();
    lastMousePosition = Input.mousePosition;
  }

  public void CancelUpdateFunc()
  {
    Update_CurrentFunc = Update_DetectModeStart;
  }

  void Update_DetectModeStart()
  {
    // Check here(?) to see if we are over a UI element,
    // if so -- ignore mouse clicks and such.

    if (Input.GetMouseButton(0) &&
        Vector3.Distance(Input.mousePosition, lastMousePosition) > mouseDragThreshold)
    {
      // Left button is being held down AND the mouse moved? That's a camera drag!
      Update_CurrentFunc = Update_CameraDrag;
      lastMouseGroundPlanePosition = MouseToGroundPlane(Input.mousePosition);
      Update_CurrentFunc();
    }
  }

  void Update_ScrollZoom()
  {
    // Zoom to scrollwheel
    float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
    float minHeight = 2;
    float maxHeight = 20;
    // Move camera towards hitPos
    Vector3 hitPos = MouseToGroundPlane(Input.mousePosition);
    Vector3 dir = hitPos - Camera.main.transform.position;

    Vector3 p = Camera.main.transform.position;

    // Stop zooming out at a certain distance.
    // TODO: Maybe you should still slide around at 20 zoom?
    if (scrollAmount > 0 || p.y < (maxHeight - 0.1f))
    {
      cameraTargetOffset += dir * scrollAmount;
    }
    Vector3 lastCameraPosition = Camera.main.transform.position;
    Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + cameraTargetOffset, Time.deltaTime * 5f);
    cameraTargetOffset -= Camera.main.transform.position - lastCameraPosition;


    p = Camera.main.transform.position;
    if (p.y < minHeight)
    {
      p.y = minHeight;
    }
    if (p.y > maxHeight)
    {
      p.y = maxHeight;
    }
    Camera.main.transform.position = p;

    // Change camera angle
    Camera.main.transform.rotation = Quaternion.Euler(
        Mathf.Lerp(30, 75, Camera.main.transform.position.y / maxHeight),
        Camera.main.transform.rotation.eulerAngles.y,
        Camera.main.transform.rotation.eulerAngles.z
    );
  }

  void Update_CameraDrag()
  {
    if (Input.GetMouseButtonUp(0))
    {
      CancelUpdateFunc();
      return;
    }

    Vector3 hitPos = MouseToGroundPlane(Input.mousePosition);
    Vector3 diff = lastMouseGroundPlanePosition - hitPos;
    Camera.main.transform.Translate(diff, Space.World);

    lastMouseGroundPlanePosition = hitPos = MouseToGroundPlane(Input.mousePosition);
  }

  Vector3 MouseToGroundPlane(Vector3 mousePos)
  {
    Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
    // What is the point at which the mouse ray intersects Y=0
    if (mouseRay.direction.y >= 0)
    {
      //Debug.LogError("Why is mouse pointing up?");
      return Vector3.zero;
    }
    float rayLength = (mouseRay.origin.y / mouseRay.direction.y);
    return mouseRay.origin - (mouseRay.direction * rayLength);
  }


}
