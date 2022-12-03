using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
  [Header("References")]
  public GameManager gamemanager;
  public Transform orientation;

  public GameObject attackSpawn;
  public GameObject fistPrefab;

  [Header("Keybinds")]
  public KeyCode jumpKey = KeyCode.Space;
  private float _horizontalInput;
  private float _verticalInput;

  [Header("Movement")]
  public float moveSpeed;
  public float sprintSpeed;
  public float jumpForce;
  public float airMultiplier;

  private Vector3 _moveDirection;
  private Rigidbody _rb;

  [Header("Ground Check")]
  public bool isGrounded;
  public LayerMask groundMasks;
  public float playerHeight;
  public float groundDrag;

  // Start is called before the first frame update
  void Start() {
    _rb = GetComponent<Rigidbody>();
    // Prevent the player falls over
    _rb.freezeRotation = true;
  }

  // Update is called once per frame
  void Update() {
    GetInput();

    // Player Movement
    GroundedCheck();
    Move();

    // Others
    Attack();
  }

  private void GetInput() {
    _horizontalInput = Input.GetAxisRaw("Horizontal");
    _verticalInput = Input.GetAxisRaw("Vertical");

    if (Input.GetKeyDown(jumpKey) && isGrounded) {
      Jump();
    }
  }

  private void Move() {
    float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

    _moveDirection = 
      orientation.forward * _verticalInput + 
      orientation.right * _horizontalInput;
    
    if (isGrounded) {
      _rb.drag = groundDrag;
      _rb.AddForce(_moveDirection.normalized * speed, ForceMode.Force);
    }
    else {
      _rb.drag = 0;
      _rb.AddForce(_moveDirection.normalized * speed * airMultiplier, ForceMode.Force);
    }
  }

  private void GroundedCheck() {
    isGrounded = Physics.Raycast(
      transform.position, 
      Vector3.down, 
      playerHeight * 0.5f + 0.2f, 
      groundMasks
    );
  }

  private void Jump() {
    _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

    _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
  }

  private void Attack() {
    if (!gamemanager.isPaused) {
      if (Input.GetMouseButtonDown(0)) {
        Instantiate(fistPrefab, attackSpawn.transform.position, attackSpawn.transform.rotation);
      }
    }
  }
}
