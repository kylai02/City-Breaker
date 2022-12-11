using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FanFlame : BasicSkill {
  [Header("FanFlame")]
  public GameObject fanFlame;
  public float attackDuration;

  public override void UseSkill() {
    readyToUse = false;

    fanFlame.SetActive(true);
    Invoke(nameof(CloseFanFlame), attackDuration);
  }

  private void CloseFanFlame() {
    fanFlame.SetActive(false);
    Invoke(nameof(ResetUse), coolDown);
  }
}