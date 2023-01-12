using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanFlameAddon : MonoBehaviour {
  [Header("Settings")]
  public float attackRange;
  public int accurate = 10;
  public float attackAngle;

  // Start is called before the first frame update
  void Start() {
        
  }

  // Update is called once per frame
  void Update() {
    float subAngle = (attackAngle / 2) / accurate;

    for (int i = 0; i < accurate; ++i) {
      ShootLayer(Quaternion.Euler(0f, -1 * subAngle * (i + 1), 0f));
      ShootLayer(Quaternion.Euler(0f, subAngle * (i + 1), 0f));
    }
  }

  private void ShootLayer(Quaternion eulerAngle) {
    Debug.DrawRay(
      transform.position, 
      eulerAngle * transform.forward * attackRange,
      Color.green
    );

    RaycastHit hit;
    if (Physics.Raycast(
      transform.position,
      eulerAngle * transform.forward,
      out hit,
      attackRange
    ) && hit.collider.CompareTag("Building")) {
      Building targetBuilding = hit.collider.GetComponent<Building>();
      targetBuilding.SetOnFire(10f);
    }
  }
}
