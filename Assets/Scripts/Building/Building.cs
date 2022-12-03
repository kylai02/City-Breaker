using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Building : MonoBehaviour {
  public GameObject nextStage;
  protected int _tier;
  protected float _timer;

  [Header("Building Perameters")]
  public int defaultHealth;
  public int health;
  public float countdown;
  public int experience;
  
  // Start is called before the first frame update
  void Start() {
    CheckTier();
    _timer = 0;
  }

  // Update is called once per frame
  void Update() {
    _timer += Time.deltaTime;
    
    Survive();
    Upgrade();
  }

  public void DealDmg(int dmg) {
    health -= dmg;
  }

  private void Survive() {
    if (health <= 0) {
      GameObject.Find("GameManager").GetComponent<GameManager>().AddExperience(experience);
      Destroy(gameObject);
    }
  }

  private void Upgrade() {
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
