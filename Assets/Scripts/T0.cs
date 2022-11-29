using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class T0 : Building {

  protected override void CheckTier() {
    _tier = 0;
  }

  protected override void Upgrade() {}
}