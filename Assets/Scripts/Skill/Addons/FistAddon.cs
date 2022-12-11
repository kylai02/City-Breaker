using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistAddon : MonoBehaviour {
  [Header("Settings")]
  public float damage;

  // Start is called before the first frame update
  void Start() {
        
  }

    // Update is called once per frame
  void Update() {
        
  }

  private void OnCollisionEnter(Collision other) {
    if (other.gameObject.tag == "Building") {
      other.gameObject.GetComponent<Building>().DealDmg(damage);
      Destroy(gameObject);
    }
  }
}
