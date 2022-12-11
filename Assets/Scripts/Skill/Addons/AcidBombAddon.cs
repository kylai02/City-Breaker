using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBombAddon : MonoBehaviour {
  [Header("Settings")]
  public LayerMask buildingLayer;
  public float directDamage;
  public float sputteringDamege;
  public float sputteringRadius;
  

  // Start is called before the first frame update
  void Start() {
    
  }

  // Update is called once per frame
  void Update() {
    
  }

  private void OnCollisionEnter(Collision other) {
    Collider[] objectsInRange = Physics.OverlapSphere(
      transform.position, 
      sputteringRadius, 
      buildingLayer
    );

    foreach (Collider target in objectsInRange) {
      Building targetBuilding = target.gameObject.GetComponent<Building>();

      if (target.gameObject == other.gameObject) {
        targetBuilding.DealDmg(directDamage);
      }
      else {
        targetBuilding.DealDmg(sputteringDamege);
      }
      targetBuilding.corrodeTimer = 4;
      targetBuilding.onCorrode = true;
    }

    Destroy(gameObject);
  }
}
