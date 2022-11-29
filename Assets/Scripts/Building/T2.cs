using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class T2 : Building {
  public GameObject nextStage;

  protected override void CheckTier() {
    _tier = 2;
  }

  protected override void ResetHealth() {
    health = (_tier * -10) + 100;
  }

  protected override void Upgrade() {
    if (_timer >= countdown) {
      Instantiate(nextStage, transform.position, transform.rotation);
      Destroy(gameObject);
    }
  }
}