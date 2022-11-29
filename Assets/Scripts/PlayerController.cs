using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
  public CharacterController controller;
  public Transform cam;
  public GameObject attackSpawn;
  public GameObject fistPrefab;
  public GameManager gamemanager;

  [Header("Player")]
  public float walkSpeed = 6.0f;
  public float sprintSpeed = 12.0f;

  // Trun around head smoothly
  public float turnSmoothTime = 0.1f;
  private float _turnSmoothVelocity;
  
  public float jumpHeight = 1.5f;
  public float gravity = -19.6f;
  private Vector3 _verticalVelocity;

  [Header("Player Grounded")]
  public bool isGrounded;

  public Transform groundCheck;
  public float groundDistance = 0.2f;
  public LayerMask groundMasks;

  // Start is called before the first frame update
  void Start() {
        
  }

  // Update is called once per frame
  void Update() {
    // Player Movement
    GroundedCheck();
    JumpAndGravity();
    Move();

    // Others
    Attack();
  }

  private void Move() {
    float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");
    Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
    Vector3 moveDirection = new Vector3(0f, 0f, 0f);

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

      moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    controller.Move(
      moveDirection.normalized * speed * Time.deltaTime + 
      _verticalVelocity * Time.deltaTime
    );
  }

  private void GroundedCheck() {
    isGrounded = Physics.CheckSphere(
      groundCheck.position,
      groundDistance,
      groundMasks
    );
  }

  private void JumpAndGravity() {
    if (isGrounded && _verticalVelocity.y < 0) {
      _verticalVelocity.y = -2f;
    }

    if (isGrounded && Input.GetKeyDown(KeyCode.Space)) {
      _verticalVelocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    _verticalVelocity.y += gravity * Time.deltaTime;
  }

  private void Attack() {
    if (!gamemanager.isPaused) {
      if (Input.GetMouseButtonDown(0)) {
        Instantiate(fistPrefab, attackSpawn.transform.position, transform.rotation);
      }
    }
  }
}
