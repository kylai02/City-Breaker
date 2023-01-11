using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Fist : BasicSkill {
  [Header("Fist")]
  public GameObject fistPrefab;
  public AudioSource punchSource;
  public float speed;
  public float lifetime;
  public float damage;
 
  public override void UseSkill() {
    if (!punchSource.isPlaying) {
      punchSource.Play();
    }
    readyToUse = false;
    coolDownTimer = coolDown;

    Vector3 source = attackSpawn.position + attackSpawn.transform.right * 1;

    GameObject fist = Instantiate(
      fistPrefab, 
      source, 
      attackSpawn.rotation
    );
    fist.transform.Rotate(Vector3.forward, 90f);

    Rigidbody fistRb = fist.GetComponent<Rigidbody>();

    Vector3 destination = 
      attackSpawn.position + 
      attackSpawn.transform.forward * (speed * lifetime);
    fistRb.velocity = (destination - source).normalized * speed * 1.08f;

    Destroy(fist, lifetime);
    Invoke(nameof(ResetUse), coolDown);
  }
}