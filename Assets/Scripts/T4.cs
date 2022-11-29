using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class T4 : Building {
  public GameObject nextStage;

  protected override void CheckTier() {
    _tier = 4;
  }

  protected override void Upgrade() {
    if (_timer >= 5) {
      Instantiate(nextStage, transform.position, transform.rotation);
      Destroy(gameObject);
    }
  }
}