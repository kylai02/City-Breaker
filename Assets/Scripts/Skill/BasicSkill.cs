using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract class BasicSkill : MonoBehaviour {
  [Header("References")]
  public Transform attackSpawn;
  public GameManager gameManager;
  public SkillSystem skillSystem;
  public Image cooldownCover;

  [Header("Settings")]
  public KeyCode attackKey = KeyCode.F;
  public float coolDown;
  public bool unlocked;
  public int skillNumber;

  public bool readyToUse {
    get; 
    protected set;
  }

  private float coolDownTimer;

  // Start is called before the first frame update
  void Start() {
    readyToUse = true;
    skillSystem = gameManager.skillSystem;
  }

  // Update is called once per frame
  void Update() {
    if (skillNumber == gameManager.chosenSkill && 
      Input.GetKeyDown(attackKey) &&
      readyToUse &&
      unlocked
    ) {
      UseSkill();
      coolDownTimer = coolDown;
    }

    SetCoolDownCover();
  }

  public abstract void UseSkill();

  protected void ResetUse() {
    readyToUse = true;
  }

  private void SetCoolDownCover() {
    if (unlocked) {
      if (coolDownTimer > 0) {
        cooldownCover.fillAmount = coolDownTimer / coolDown;
        coolDownTimer -= Time.deltaTime;
      }
      else {
        cooldownCover.fillAmount = 0;
      }
    }
    else {
      cooldownCover.fillAmount = 1;
    }
  }
}
