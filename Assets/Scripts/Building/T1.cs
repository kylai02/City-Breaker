using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class T1 : Building {
  public GameObject upgradeAnimation;

  private bool _alreadyAlert;

  protected override void CheckTier() {
    _tier = 1;
    _alreadyAlert = false;
  }

  private void Update() {
    _timer += Time.deltaTime;

    Survive();
    ShowDangerous();
    Upgrade();
  }

  private void ShowDangerous() {
    if (!_alreadyAlert && _timer > 2) {
      upgradeAnimation.SetActive(true);
      GameObject.Find("GameManager").GetComponent<GameManager>().Alert();
      _alreadyAlert = true;
    }
  }
}