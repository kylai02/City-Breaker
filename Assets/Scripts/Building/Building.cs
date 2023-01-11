using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

abstract class Building : MonoBehaviour {
  [Header("References")]
  public GameObject nextStage1;
  public GameObject nextStage2;
  public GameObject wholeObject;
  public GameObject fractureObject;
  public GameObject fireEffect;
  public GameObject upgradeEffect;
  public Animator animator;

  [Header("Audio")]
  public AudioSource burningSound;
  public AudioSource dissolvingSound;
  public AudioClip explosionSound;
  public AudioSource normalDestroySound;

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

  [Header("Health Bar")]
  public GameObject healthBarUI;
  public Slider slider;
  private float _healthBarTimer;
  private bool _healthBarTimeout;
  
  private bool _died;
  
  // Start is called before the first frame update
  void Start() {
    countdown += Random.Range(0f, 30f);
    _isShaking = false;
    _initPos = transform.localPosition;
    CheckTier();
    _timer = 0;
    _healthBarTimer = -1;
  }

  // Update is called once per frame
  void Update() {
    OnCorrodeCheck();
    OnFireCheck();
    ShakeCheck();

    Survive();
    Upgrade();

    SetHealthBar();
  }

  void LateUpdate() {
    healthBarUI.transform.LookAt(
      healthBarUI.transform.position + Camera.main.transform.rotation * Vector3.forward,
      Camera.main.transform.rotation * Vector3.up
    );
  }

  public void DealDmg(float dmg, bool shake) {
    health -= dmg;
    
    if (shake) {
      _shakeTimer = shakeTime;
    }

    _healthBarTimer = 2f;
    healthBarUI.SetActive(true);
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
    if (!animator.GetBool("Dissolve")) {
      animator.SetBool("Dissolve", true);
    }
  }

  protected void OnCorrodeCheck() {
    if (!onCorrode) {
      _timer += Time.deltaTime;
    }
    else {
      corrodeTimer -= Time.deltaTime;
      if (!dissolvingSound.isPlaying) {
        dissolvingSound.Play();
      }
    }

    if (corrodeTimer <= 0) {
      animator.SetBool("Dissolve", false);
      onCorrode = false;
      if (dissolvingSound.isPlaying) {
        dissolvingSound.Stop();
      }
    }
  }

  protected void OnFireCheck() {
    if (onFire) {
      fireTimer -= Time.deltaTime;
      DealDmg(fireDamage * Time.deltaTime, true);
      if (!burningSound.isPlaying) {
        burningSound.Play();
      }
    }
    if (fireTimer <= 0) {
      fireEffect.SetActive(false);
      onFire = false;
      if (burningSound.isPlaying) {
        burningSound.Stop();
      }
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
      if (upgradeEffect) {
        upgradeEffect.SetActive(false);
      }
      GameObject.Find("GameManager").GetComponent<GameManager>().AddExperience(experience);
      if (_tier == 1) {
        GameObject.Find("GameManager").GetComponent<GameManager>().AlertCounterChange(-1);
      }

      _died = true;
      if (onFire) {
        Explosion();
        Destroy(wholeObject, 5f);
      }
      else if (onCorrode) {
        animator.SetTrigger("Dissolve-Trigger");
        Destroy(wholeObject, 1f);
      }
      else {
        normalDestroySound.Play();
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

    AudioSource.PlayClipAtPoint(explosionSound, transform.position);

    gameObject.SetActive(false);
    fireEffect.SetActive(false);

    if (fractureObject) {
      fractureObject.SetActive(true);
      
      CoroutineManager coroutineManager = FindObjectOfType<CoroutineManager>();
      foreach (Transform chip in fractureObject.transform) {
        var rb = chip.GetComponent<Rigidbody>();

        rb.AddExplosionForce(
          explosionForce,
          transform.position,
          isUpgrade ? sputteringRadius * 2 : sputteringRadius
        );

        coroutineManager.StartShrink(chip, 0.5f);
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

      bool upgradePre = Random.Range(0, 2) == 0;
      GameObject nextStage = Instantiate(
        upgradePre ? nextStage1 : nextStage2,
        transform.position, 
        nextStage1.transform.rotation
      );
      Building nextStageBuilding = nextStage.transform.GetChild(0).GetComponent<Building>();
      
      if (_tier != 1) {
        // Set nextStage's health to the remain health of this building
        nextStageBuilding.DealDmg(defaultHealth - health, false);
        if (onFire) {
          nextStageBuilding.SetOnFire(fireTimer);
        }
      }

      Destroy(wholeObject);
    }
  }

  protected void SetHealthBar() {
    slider.value = health / defaultHealth;
    
    if (_died) {
      healthBarUI.SetActive(false);
    }
    else if (_healthBarTimer < 0) {
      healthBarUI.SetActive(false);
    } else {
      _healthBarTimer -= Time.deltaTime;
    }
  }

  protected abstract void CheckTier();
}
