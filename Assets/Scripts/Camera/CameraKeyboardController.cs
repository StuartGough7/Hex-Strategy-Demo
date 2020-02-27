using UnityEngine;

public class CameraKeyboardController : MonoBehaviour {
  float moveSpeed = 3.5f;

  void Update() {
    Vector3 translate = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxis("Vertical"));
    this.transform.Translate(translate * moveSpeed * Time.deltaTime * (1 + this.transform.position.y / 2), Space.World);
  }

}
