using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class T1 : Building {
  private bool _alreadyAlert;
  private bool _isUpgrade;
  public bool _canUpgrade;

  private void Awake() {
    _canUpgrade = Random.Range(0, 10) == 0;
  }

  private void Update() {
    OnCorrodeCheck();
    OnFireCheck();
    ShakeCheck();

    Survive();
    ShowDangerous();
    Upgrade();

    SetHealthBar();
  }

  protected override void CheckTier() {
    _tier = 1;
    _alreadyAlert = false;
  }

  protected override void Upgrade() {
    if (_canUpgrade && _timer >= countdown) {
      experience = 0;

      GameObject nextStage = Instantiate(
        nextStage1,
        transform.position,
        nextStage1.transform.rotation
      );

      Destroy(wholeObject);
    }
  }

  private void ShowDangerous() {
    if (_canUpgrade && !_alreadyAlert) {
      upgradeEffect.SetActive(true);
      GameObject.Find("GameManager").GetComponent<GameManager>().Alert();
      GameObject.Find("GameManager").GetComponent<GameManager>().AlertCounterChange(1);
      _alreadyAlert = true;
    }
  }
}