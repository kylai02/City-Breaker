using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AcidCloud : BasicSkill {
  [Header("AcidCloud")]
  public GameObject acidCloudPrefab;
  public float lifetime;

  public override void UseSkill() {
    readyToUse = false;

    Vector3 spawn = attackSpawn.position;
    spawn.y = 9;

    GameObject acidCloud = Instantiate(
      acidCloudPrefab,
      spawn,
      attackSpawn.rotation
    );

    Destroy(acidCloud, lifetime);
    Invoke(nameof(ResetUse), coolDown);
  }
}