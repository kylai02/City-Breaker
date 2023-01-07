using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FireLaser : BasicSkill {
  [Header("FireLaser")]
  public GameObject fireLaser;
  public float attackDuration;

  public override void UseSkill() {
    readyToUse = false;

    fireLaser.SetActive(true);
    Invoke(nameof(CloseFireLaser), attackDuration);
  }

  private void CloseFireLaser() {
    coolDownTimer = coolDown;
    fireLaser.SetActive(false);
    Invoke(nameof(ResetUse), coolDown);
  }
}