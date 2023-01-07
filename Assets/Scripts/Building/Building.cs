using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

abstract class Building : MonoBehaviour {
  [Header("References")]
  public GameObject nextStage;
  public GameObject wholeObject;
  public GameObject fractureObject;
  public GameObject fireEffect;
  public GameObject corrodeEffect;

  [Header("Settings")]
  public float defaultHealth;
  public float health;
  public float countdown;
  public int experience;
  public float shakeTime;
  public float shakeAmount;

  protected float _timer;
  protected int _tier;
  private float _shakeTimer;
  private Vector3 _initPos;
  private bool _isShaking;

  [Header("Explosion")]
  public LayerMask buildingLayer;
  public GameObject basicExplosionEffect;
  public GameObject upgradeExplosionEffect;
  public float sputteringRadius;
  public float sputteringDamege;
  public float explosionForce;
  
  [Header("Status")]
  public bool onCorrode;
  public bool onFire;
  public float corrodeTimer;
  public float fireTimer;
  public float fireDamage;
  
  private bool _died;
  
  // Start is called before the first frame update
  void Start() {
    _isShaking = false;
    _initPos = transform.localPosition;
    CheckTier();
    _timer = 0;
  }

  // Update is called once per frame
  void Update() {
    OnCorrodeCheck();
    OnFireCheck();
    ShakeCheck();

    Survive();
    Upgrade();
  }

  public void DealDmg(float dmg, bool shake) {
    health -= dmg;
    
    if (shake) {
      _shakeTimer = shakeTime;
    }
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
      DealDmg(fireDamage * Time.deltaTime, true);
    }
    if (fireTimer <= 0) {
      fireEffect.SetActive(false);
      onFire = false;
    }
  }

  protected void ShakeCheck() {
    if (!_died) {
      if (_shakeTimer > 0) {
        Vector3 shakePos = _initPos + Random.insideUnitSphere * shakeAmount;
        shakePos.y = _initPos.y;
        transform.localPosition = shakePos;

        _shakeTimer -= Time.deltaTime;
      }
      else {
        transform.localPosition = _initPos;
      }
    }
    else {
      Vector3 shakePos = _initPos + Random.insideUnitSphere * shakeAmount * 2f;
      shakePos.y = _initPos.y;
      transform.localPosition = shakePos;
    }
  }

  protected void Survive() {
    if (!_died && health <= 0) {
      GameObject.Find("GameManager").GetComponent<GameManager>().AddExperience(experience);
      if (onFire) {
        Explosion();
        Destroy(wholeObject, 3f);
      }
      else {
        _died = true;
        transform.DOLocalMoveY(-8, 5);
        Destroy(wholeObject, 5f);
      }
    }
  }

  protected void Explosion() {
    bool isUpgrade = GameObject.Find("GameManager").GetComponent<GameManager>().explosionUpgrade;
    GameObject effect = isUpgrade ?
      Instantiate(upgradeExplosionEffect, gameObject.transform.position, gameObject.transform.rotation) : 
      Instantiate(basicExplosionEffect, gameObject.transform.position, gameObject.transform.rotation);
    Destroy(effect, 1.5f);

    if (fractureObject) {
      gameObject.SetActive(false);
      fireEffect.SetActive(false);
      corrodeEffect.SetActive(false);
      fractureObject.SetActive(true);
      
      foreach (Transform chip in fractureObject.transform) {
        var rb = chip.GetComponent<Rigidbody>();

        rb.AddExplosionForce(
          explosionForce,
          transform.position,
          isUpgrade ? sputteringRadius * 2 : sputteringRadius
        );

        // StartCoroutine(Shrink(chip, 2));
      }
    }
    

    Collider[] objectsInRange = Physics.OverlapSphere(
      transform.position,
      isUpgrade ? sputteringRadius * 2 : sputteringRadius,
      buildingLayer
    );

    foreach (Collider target in objectsInRange) {
      Building targetBuilding = target.gameObject.GetComponent<Building>();

      targetBuilding.DealDmg(sputteringDamege, true);
    }
  }

  protected void Upgrade() {
    if (_tier != 0 && _timer >= countdown) {
      // Prevent to get double experience
      experience = 0;

      nextStage = Instantiate(nextStage, transform.position, transform.rotation);

      // Set nextStage's health to the remain health of this building
      nextStage.GetComponent<Building>().DealDmg(defaultHealth - health, false);
      if (onFire) {
        nextStage.GetComponent<Building>().SetOnFire(fireTimer);
      }
      Destroy(wholeObject);
    }
  }

  protected abstract void CheckTier();

  // IEnumerable Shrink(Transform t, float delay)
  // {
  //   yield return new WaitForSeconds(delay);

  //   Vector3 newScale = t.localScale;

  //   while (newScale.x >= 0)
  //   {
  //     newScale -= new Vector3(1f, 1f, 1f);

  //     t.localScale = newScale;
  //     yield return new WaitForSeconds(0.05f);
  //   }
  // }
}
