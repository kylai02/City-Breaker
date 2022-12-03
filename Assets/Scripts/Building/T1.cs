using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class T1 : Building {
  protected override void CheckTier() {
    _tier = 1;
  }
}