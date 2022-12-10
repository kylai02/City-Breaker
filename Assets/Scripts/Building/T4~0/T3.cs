using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class T3 : Building {
  protected override void CheckTier() {
    _tier = 3;
  }
}