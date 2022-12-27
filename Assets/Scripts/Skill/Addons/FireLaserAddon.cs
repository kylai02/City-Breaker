using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLaserAddon : MonoBehaviour {
  [Header("Settings")]
  public float attackRange;
  public float damage;

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    Debug.DrawRay(
      transform.position,
      transform.forward * attackRange,
      Color.green
    );

    RaycastHit hit;
    if (Physics.Raycast(
      transform.position,
      transform.forward,
      out hit,
      attackRange
    ) && hit.collider.CompareTag("Building")) {
      Building targetBuilding = hit.collider.GetComponent<Building>();
      targetBuilding.DealDmg(damage * Time.deltaTime);
      targetBuilding.SetOnFire(5f);
    }
  }
}
