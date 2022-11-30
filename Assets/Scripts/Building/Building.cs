using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Building : MonoBehaviour {
  public GameObject nextStage;
  protected int _tier;
  protected float _timer;

  [Header("Building Perameters")]
  public int defaultHealth;
  private int _health;
  public float countdown;
  public int experience;
  
  // Start is called before the first frame update
  void Start() {
    CheckTier();
    _health = defaultHealth;
    _timer = 0;
  }

  // Update is called once per frame
  void Update() {
    _timer += Time.deltaTime;
    
    Survive();
    Upgrade();
  }

  public void ResetHealth(int minusHealth) {
    _health += minusHealth;
  }

  public void DealDmg(int dmg) {
    _health -= dmg;
  }

  private void Survive() {
    if (_health <= 0) {
      GameObject.Find("GameManager").GetComponent<GameManager>().AddExperience(experience);
      Destroy(gameObject);
    }
  }

  private void Upgrade() {
    if (_tier != 0 && _timer >= countdown) {
      Instantiate(nextStage, transform.position, transform.rotation);
      nextStage.GetComponent<Building>().ResetHealth(_health - defaultHealth);
      Destroy(gameObject);
    }
  }

  protected abstract void CheckTier();
}
