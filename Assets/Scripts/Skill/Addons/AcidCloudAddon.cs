using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidCloudAddon : MonoBehaviour {
  [Header("References")]
  public CharacterController controller;

  [Header("Settings")]
  public LayerMask buildingLayer;
  public float speed;
  public float damage;

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
    float angle = Random.Range(-45f, 45f);

    gameObject.transform.Rotate(Vector3.up, angle);
  }

  private void Rain() {
    Vector3 groundPos = transform.position;
    groundPos.y -= 10;

    Collider[] objectsUnderCloud = Physics.OverlapCapsule(
      transform.position,
      groundPos,
      2.5f,
      buildingLayer
    );

    foreach (Collider target in objectsUnderCloud) {
      Building targetBuilding = target.gameObject.GetComponent<Building>();
      targetBuilding.SetOnCorrode(4f);
      targetBuilding.DealDmg(damage * Time.deltaTime, true);
    }
  }
}
