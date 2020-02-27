using UnityEngine;

public class CameraKeyboardController : MonoBehaviour
{
  void Start()
  {

  }
  float moveSpeed = 15f;
  void Update()
  {
    Vector3 translate = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxis("Vertical"));
    this.transform.Translate(translate * moveSpeed * Time.deltaTime, Space.World);
  }
}
