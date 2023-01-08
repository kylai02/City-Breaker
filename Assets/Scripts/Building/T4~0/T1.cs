using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class T1 : Building {
  private bool _alreadyAlert;
  
  private void Update() {
    OnCorrodeCheck();
    OnFireCheck();
    ShakeCheck();

    Survive();
    ShowDangerous();
    Upgrade();
  }

  protected override void CheckTier() {
    _tier = 1;
    _alreadyAlert = false;
  }

  private void ShowDangerous() {
    if (!_alreadyAlert) {
      upgradeEffect.SetActive(true);
      GameObject.Find("GameManager").GetComponent<GameManager>().Alert();
      GameObject.Find("GameManager").GetComponent<GameManager>().AlertCounterChange(1);
      _alreadyAlert = true;
    }
  }
}