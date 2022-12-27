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
  
  public bool onCorrode;
  public bool onFire;
  public float corrodeTimer;
  public float fireTimer;
  public float fireDamage;
  
  // Start is called before the first frame update
  void Start() {
    CheckTier();
    _timer = 0;

    onCorrode = false;
    onFire = false;
    corrodeTimer = -1;
    fireTimer = -1;
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

  protected void OnCorrodeCheck() {
    if (!onCorrode) {
      _timer += Time.deltaTime;
    }
    else {
      if (corrodeEffect) {
        corrodeEffect.SetActive(true);
      }
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
      if (fireEffect) {
        fireEffect.SetActive(true);
      }
    }
    if (fireTimer <= 0) {
      fireEffect.SetActive(false);
      onFire = false;
    }
  }

  protected void Survive() {
    if (health <= 0) {
      GameObject.Find("GameManager").GetComponent<GameManager>().AddExperience(experience);
      Destroy(gameObject);
    }
  }

  protected void Upgrade() {
    if (_tier != 0 && _timer >= countdown) {
      // Prevent to get double experience
      experience = 0;

      nextStage = Instantiate(nextStage, transform.position, transform.rotation);

      // Set nextStage's health to the remain health of this building
      nextStage.GetComponent<Building>().DealDmg(defaultHealth - health);
      Destroy(gameObject);
    }
  }

  protected abstract void CheckTier();
}
