using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
  [Header("References")]
  public GameManager gamemanager;
  public Transform orientation;
  public CharacterController controller;

  [Header("Keybinds")]
  public KeyCode jumpKey = KeyCode.Space;

  private float _horizontalInput;
  private float _verticalInput;

  [Header("Movement")]
  public float moveSpeed;
  public float sprintSpeed;
  public float gravity;
  public float jumpHeight;
  public Vector3 velocity;

  private Vector3 _moveDirection;
  private Vector3 _verticalVelocity;

  [Header("Ground Check")]
  public bool isGrounded;
  public LayerMask groundMasks;
  public float playerHeight;
  public float groundDrag;

  [Header("Attack")]
  public GameObject attackSpawn;
  public GameObject fistPrefab;

  [Header("Audio Source")]
  public AudioSource jumpSound;

  // Start is called before the first frame update
  void Start() {
    _verticalVelocity = new Vector3(0f, 0f, 0f);
  }

  // Update is called once per frame
  void Update() {
    // Player Movement
    GroundedCheck();
    JumpAndGravity();
    Move();

  }

  private void Move() {
    float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

    _horizontalInput = Input.GetAxisRaw("Horizontal");
    _verticalInput = Input.GetAxisRaw("Vertical");

    _moveDirection = 
      orientation.forward * _verticalInput + 
      orientation.right * _horizontalInput;

    velocity = _moveDirection * speed + _verticalVelocity;
    controller.Move(_moveDirection * speed * Time.deltaTime + _verticalVelocity * Time.deltaTime);
  }

  private void GroundedCheck() {
    isGrounded = Physics.Raycast(
      transform.position, 
      Vector3.down, 
      playerHeight * 0.5f + 0.2f, 
      groundMasks
    );
  }

  private void JumpAndGravity() {
    if (isGrounded && _verticalVelocity.y < 0) {
      _verticalVelocity.y = -2f;
    }
    if (Input.GetKeyDown(jumpKey) && isGrounded) {
      jumpSound.Play();
      _verticalVelocity.y += Mathf.Sqrt(jumpHeight * 2 * gravity);
    }

    _verticalVelocity.y -= gravity * Time.deltaTime;
  }
}
