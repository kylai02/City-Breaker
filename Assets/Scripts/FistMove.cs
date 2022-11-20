using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistMove : MonoBehaviour {
  public float speed = 10f;

  private Rigidbody _rigid;

  // Start is called before the first frame update
  void Start() {
    this._rigid = GetComponent<Rigidbody>();
    this._rigid.velocity = transform.forward * speed;

    Destroy(gameObject, 0.25f);
  }

  // Update is called once per frame
  void Update() {

  }

  private void OnCollisionEnter(Collision other) {
    Destroy(gameObject);
  }
}
