using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class T2 : Building {
  protected override void CheckTier() {
    _tier = 2;
  }
}