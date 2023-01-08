using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Fist : BasicSkill {
  [Header("Fist")]
  public GameObject fistPrefab;
  public float speed;
  public float lifetime;
  public float damage;
 
  public override void UseSkill() {
    readyToUse = false;
    coolDownTimer = coolDown;

    GameObject fist = Instantiate(
      fistPrefab, 
      attackSpawn.position, 
      attackSpawn.rotation
    );
    Rigidbody fistRb = fist.GetComponent<Rigidbody>();

    fistRb.velocity = attackSpawn.transform.forward * speed;

    Destroy(fist, lifetime);
    Invoke(nameof(ResetUse), coolDown);
  }
}