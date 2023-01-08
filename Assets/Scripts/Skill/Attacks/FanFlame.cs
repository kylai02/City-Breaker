using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FanFlame : BasicSkill {
  [Header("FanFlame")]
  public GameObject fanFlame;
  public float attackDuration;
  public AudioSource fanFlameSound;

  public override void UseSkill() {
    fanFlameSound.Play();
    readyToUse = false;

    fanFlame.SetActive(true);
    Invoke(nameof(CloseFanFlame), attackDuration);
  }

  private void CloseFanFlame() {
    fanFlameSound.Stop();
    coolDownTimer = coolDown;
    fanFlame.SetActive(false);
    Invoke(nameof(ResetUse), coolDown);
  }
}