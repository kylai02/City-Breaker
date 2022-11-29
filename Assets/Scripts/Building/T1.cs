using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class T1 : Building {
  public GameObject nextStage;

  protected override void CheckTier() {
    _tier = 1;
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