using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class T0 : Building {
  private void Start() {
    GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
  }

  protected override void CheckTier() {
    _tier = 0;
  }
}