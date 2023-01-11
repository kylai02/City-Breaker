using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class T0 : Building {
  void Start() {
    GameObject.Find("GameManager").GetComponent<GameManager>().GameOver(transform);
  }

  void Update() {}

  void LateUpdate() {}

  protected override void CheckTier() {
    _tier = 0;
  }
}