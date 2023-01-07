using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AcidBomb : BasicSkill {
  [Header("AcidBomb")]
  public GameObject acidBombPrefab;
  public Transform cam;
  public float throwForce;
  public float throwUpwardForce;

  public override void UseSkill() {
    readyToUse = false;
    coolDownTimer = coolDown;

    GameObject acidBomb = Instantiate(
      acidBombPrefab,
      attackSpawn.position,
      attackSpawn.rotation
    );
    Rigidbody acidBombRb = acidBomb.GetComponent<Rigidbody>();

    Vector3 forceToAdd =
    cam.transform.forward * throwForce +
    transform.up * throwUpwardForce;

    acidBombRb.AddForce(forceToAdd, ForceMode.Impulse);

    Invoke(nameof(ResetUse), coolDown);
  }
}