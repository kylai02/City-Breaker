using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Building : MonoBehaviour {
  public GameObject nextStage;
  protected int _tier;
  protected float _timer;

  [Header("Settings")]
  public int defaultHealth;
  public int health;
  public float countdown;
  public int experience;
  
  public bool onCorrode;
  public bool onFire;
  public float corrodeTimer;
  public float fireTimer;
  
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

    Survive();
    Upgrade();
  }

  public void DealDmg(int dmg) {
    health -= dmg;
  }

  private void OnCorrodeCheck() {
    if (!onCorrode) {
      _timer += Time.deltaTime;
    }
    else {
      corrodeTimer -= Time.deltaTime;
    }

    if (corrodeTimer <= 0) {
      onCorrode = false;
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
