using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySystem {
  private List<bool> _acquiredAbility;

  public AbilitySystem() {
    _acquiredAbility = new List<bool>(15);
    for (int i = 0; i < 15; ++i) {
      _acquiredAbility.Add(false);
    }
  }

  public void Acquired(int index) {
    _acquiredAbility[index] = true;
    // Use ability function
  }

  public bool isAcquired(int index) {
    return _acquiredAbility[index];
  }
}