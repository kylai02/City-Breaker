using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidCloudAddon : MonoBehaviour {
  [Header("References")]
  public CharacterController controller;

  [Header("Settings")]
  public LayerMask buildingLayer;
  public float speed;

  // Start is called before the first frame update
  void Start() {
    InvokeRepeating(nameof(ChangeForward), 5f, 5f);
  }

  // Update is called once per frame
  void Update() {
    controller.Move(transform.forward * speed * Time.deltaTime);

    Rain();
  }

  private void ChangeForward() {
    float x = Random.Range(-1f, 1f);
    float z = Random.Range(-1f, 1f);

    transform.forward = new Vector3(x, 0f, z).normalized;
  }

  private void Rain() {
    Vector3 groundPos = transform.position;
    groundPos.y -= 10;

    Collider[] objectsUnderCloud = Physics.OverlapCapsule(
      transform.position,
      groundPos,
      3.2f,
      buildingLayer
    );

    foreach (Collider target in objectsUnderCloud) {
      Building targetBuilding = target.gameObject.GetComponent<Building>();
      targetBuilding.SetOnCorrode(4f);
    }
  }
}
