using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

  public CharacterController controller;
  public Transform cam;
  public GameObject attackSpawn;
  public GameObject fistPrefab;

  public float walkSpeed = 6.0f;
  public float sprintSpeed = 12.0f;

  public float turnSmoothTime = 0.1f;
  private float _turnSmoothVelocity;
  
  public float jumpHeight = 1.5f;
  public float gravity = -9.8f;
  private Vector3 _velocity;

  // Start is called before the first frame update
  void Start() {
        
  }

  // Update is called once per frame
  void Update() {
    Attack();
    Jump();
    Move();
  }

  private void Move() {
    float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");
    Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

    if (direction.magnitude >= 0.1f) {
      float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg 
        + cam.eulerAngles.y;
      float angle = Mathf.SmoothDampAngle(
        transform.eulerAngles.y, 
        targetAngle, 
        ref _turnSmoothVelocity, 
        turnSmoothTime
      );
      transform.rotation = Quaternion.Euler(0f, angle, 0f);

      Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
      controller.Move(moveDirection.normalized * speed * Time.deltaTime);
    }
  }

  private void Jump() {
    if (controller.isGrounded) {
      _velocity.y = 0f;
    }
  
    if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded) {
      _velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    _velocity.y += gravity * Time.deltaTime;

    controller.Move(_velocity * Time.deltaTime);
  }

  private void Attack() {
    if (Input.GetMouseButtonDown(0)) {
      Instantiate(fistPrefab, attackSpawn.transform.position, transform.rotation);
    }
  }
}
