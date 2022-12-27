using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Building : MonoBehaviour {
  public GameObject nextStage;
  public GameObject fireEffect;
  public GameObject corrodeEffect;
  protected int _tier;
  protected float _timer;

  [Header("Settings")]
  public float defaultHealth;
  public float health;
  public float countdown;
  public int experience;

  [Header("Explosion")]
  public LayerMask buildingLayer;
  public GameObject explosionEffect;
  public float sputteringRadius;
  public float sputteringDamege;
  
  [Header("Status")]
  public bool onCorrode;
  public bool onFire;
  public float corrodeTimer;
  public float fireTimer;
  public float fireDamage;
  
  // Start is called before the first frame update
  void Start() {
    CheckTier();
    _timer = 0;
  }

  // Update is called once per frame
  void Update() {
    OnCorrodeCheck();
    OnFireCheck();

    Survive();
    Upgrade();
  }

  public void DealDmg(float dmg) {
    health -= dmg;
  }

  public void SetOnFire(float onFireTime) {
    onFire = true;
    fireTimer = onFireTime;
    if (fireEffect) {
      fireEffect.SetActive(true);
    }
  }

  public void SetOnCorrode(float onCorrodeTime) {
    onCorrode = true;
    corrodeTimer = onCorrodeTime;
    if (corrodeEffect) {
      corrodeEffect.SetActive(true);
    }
  }

  protected void OnCorrodeCheck() {
    if (!onCorrode) {
      _timer += Time.deltaTime;
    }
    else {
      corrodeTimer -= Time.deltaTime;
    }

    if (corrodeTimer <= 0) {
      onCorrode = false;
      corrodeEffect.SetActive(false);
    }
  }

  protected void OnFireCheck() {
    if (onFire) {
      fireTimer -= Time.deltaTime;
      DealDmg(fireDamage * Time.deltaTime);
    }
    if (fireTimer <= 0) {
      fireEffect.SetActive(false);
      onFire = false;
    }
  }

  protected void Survive() {
    if (health <= 0) {
      GameObject.Find("GameManager").GetComponent<GameManager>().AddExperience(experience);
      if (onFire) {
        Explosion();
      }
      Destroy(gameObject);
    }
  }

  protected void Explosion() {
    if (explosionEffect) {
      Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation);
    }

    Collider[] objectsInRange = Physics.OverlapSphere(
      transform.position,
      sputteringRadius,
      buildingLayer
    );

    foreach (Collider target in objectsInRange) {
      Building targetBuilding = target.gameObject.GetComponent<Building>();

      targetBuilding.DealDmg(sputteringDamege);
    }
  }

  protected void Upgrade() {
    if (_tier != 0 && _timer >= countdown) {
      // Prevent to get double experience
      experience = 0;

      nextStage = Instantiate(nextStage, transform.position, transform.rotation);

      // Set nextStage's health to the remain health of this building
      nextStage.GetComponent<Building>().DealDmg(defaultHealth - health);
      if (onFire) {
        nextStage.GetComponent<Building>().SetOnFire(fireTimer);
      }
      Destroy(gameObject);
    }
  }

  protected abstract void CheckTier();
}
