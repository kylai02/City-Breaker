using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBombAddon : MonoBehaviour {
  [Header("Settings")]
  public LayerMask buildingLayer;
  public GameObject explosionEffect;
  public AudioClip explosionSound;
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
    AudioSource.PlayClipAtPoint(explosionSound, transform.position);
    GameObject effect = Instantiate(
      explosionEffect,
      gameObject.transform.position,
      gameObject.transform.rotation
    );

    Collider[] objectsInRange = Physics.OverlapSphere(
      transform.position, 
      sputteringRadius, 
      buildingLayer
    );

    foreach (Collider target in objectsInRange) {
      Building targetBuilding = target.gameObject.GetComponent<Building>();

      if (target.gameObject == other.gameObject) {
        targetBuilding.DealDmg(directDamage, true);
      }
      else {
        targetBuilding.DealDmg(sputteringDamege, true);
      }
      targetBuilding.SetOnCorrode(8f);
    }
    
    Destroy(effect, 2f);
    Destroy(gameObject);
  }
}
