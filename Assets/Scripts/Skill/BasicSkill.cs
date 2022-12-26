using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class BasicSkill : MonoBehaviour {
  [Header("References")]
  public Transform attackSpawn;
  public GameManager gameManager;
  public SkillSystem skillSystem;

  [Header("Settings")]
  public KeyCode attackKey = KeyCode.F;
  public float coolDown;
  public int skillNumber;

  public bool readyToUse {
    get; 
    protected set;
  }

  // Start is called before the first frame update
  void Start() {
    readyToUse = true;
    skillSystem = gameManager.skillSystem;
  }

  // Update is called once per frame
  void Update() {
    Debug.Log(skillSystem.IsSkillUnlocked(SkillSystem.SkillType.Fire));
    if (skillNumber == gameManager.chosenSkill && 
      Input.GetKeyDown(attackKey) && 
      readyToUse
    ) {
      UseSkill();
    }
  }

  public abstract void UseSkill();

  protected void ResetUse() {
    readyToUse = true;
  }
}
