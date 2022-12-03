using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class T1 : Building {
  protected override void CheckTier() {
    _tier = 1;
  }

  private void Update() {
    _timer += Time.deltaTime;

    Survive();
    ShowDangerous();
    Upgrade();
  }

  private void ShowDangerous() {
    if (_timer > 2) {
      Debug.Log("Dangerous");
    }
  }
}