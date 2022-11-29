using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class T0 : Building {
  protected override void CheckTier() {
    _tier = 0;
  }

  protected override void ResetHealth() {
    health = (_tier * -10) + 100;
  }

  protected override void Upgrade() {}
}