using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour {
  [Header("References")]
  public Transform orientation;
  public Transform player;
  public Transform playerObj;
  public Rigidbody rb;

  [Header("Camera")]
  public GameObject thirdPersonCam;
  public GameObject combatCam;
  public Transform combatLookAt;
  public CameraStyle currentStyle;
  public enum CameraStyle {
    Basic,
    Combat
  }

  public float rotationSpeed;

  // Start is called before the first frame update
  void Start() {
    // Set Cursor to invisible
    // Cursor.lockState = CursorLockMode.Locked;
    // Cursor.visible = false;
  }

    // Update is called once per frame
  void Update() {
    // if (Input.GetKeyDown(KeyCode.L)) {
    //   SwitchCameraStyle(CameraStyle.Basic);
    // }
    // if (Input.GetKeyDown(KeyCode.K)) {
    //   SwitchCameraStyle(CameraStyle.Combat);
    // }

    // Rotate orientation
    Vector3 viewDir = player.position - 
        new Vector3(
            transform.position.x, 
            player.position.y, 
            transform.position.z
        );
    orientation.forward = viewDir.normalized;

    if (currentStyle == CameraStyle.Basic) {
      // Rotate player object
      float horizontalInput = Input.GetAxis("Horizontal");
      float verticalInput = Input.GetAxis("Vertical");
      Vector3 inputDir =
          orientation.forward * verticalInput +
          orientation.right * horizontalInput;

      if (inputDir != Vector3.zero) {
        playerObj.forward = Vector3.Slerp(
            playerObj.forward,
            inputDir.normalized,
            rotationSpeed * Time.deltaTime
        );
      }
    } 
    else if (currentStyle == CameraStyle.Combat) {
      playerObj.forward = viewDir.normalized;
    }
  }

  private void SwitchCameraStyle(CameraStyle newStyle) {
    thirdPersonCam.SetActive(false);
    combatCam.SetActive(false);

    if (newStyle == CameraStyle.Basic) {
      thirdPersonCam.SetActive(true);
    }
    else {
      combatCam.SetActive(true);
    }

    currentStyle = newStyle;
  }
}
