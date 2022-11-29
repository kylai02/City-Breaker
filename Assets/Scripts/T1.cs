using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class T1 : Building {
  public GameObject nextStage;

  protected override void CheckTier() {
    _tier = 1;
  }

  protected override void Upgrade() {
    if (_timer >= 5) {
      Instantiate(nextStage, transform.position, transform.rotation);
      Destroy(gameObject);
    }
  }
}