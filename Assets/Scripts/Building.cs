using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Building : MonoBehaviour {
  protected int _tier;
  protected float _timer;
  
  // Start is called before the first frame update
  void Start() {
    CheckTier();
    _timer = 0;
  }

  // Update is called once per frame
  void Update() {
    _timer += Time.deltaTime;

    Upgrade();
  }

  protected abstract void CheckTier();
  protected abstract void Upgrade();
}
