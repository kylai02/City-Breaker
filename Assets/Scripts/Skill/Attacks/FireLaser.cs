using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FireLaser : BasicSkill {
  [Header("FireLaser")]
  public GameObject fireLaser;
  public float attackDuration;
  public AudioSource fireLaserSound;

  public override void UseSkill() {
    fireLaserSound.Play();
    readyToUse = false;

    fireLaser.SetActive(true);
    Invoke(nameof(CloseFireLaser), attackDuration);
  }

  private void CloseFireLaser() {
    fireLaserSound.Stop();
    coolDownTimer = coolDown;
    fireLaser.SetActive(false);
    Invoke(nameof(ResetUse), coolDown);
  }
}