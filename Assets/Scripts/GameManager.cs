using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
  public TMP_Text levelText;
  public Image experienceBar;
  public GameObject chooseAbility;

  [Header("System Info")]
  public bool isPaused = false;

  [Header("Player Info")]
  public LevelSystem levelSystem;
  
    // Start is called before the first frame update
  void Start() {
    chooseAbility.SetActive(false);
    levelSystem = new LevelSystem();
  }

    // Update is called once per frame
  void Update() {
    if (Input.GetKeyDown(KeyCode.P)) {
      PauseToggle();
    }

    SetLevelSystem();

    if (Input.GetKeyDown(KeyCode.R)) {
      AddExperience(30);
    }
  }

  private void PauseToggle() {
    if (isPaused) {
      Time.timeScale = 1;
      isPaused = false;
    }
    else {
      Time.timeScale = 0;
      isPaused = true;
    }
  }
  
  // Level System
  public void LevelUp() {
    chooseAbility.SetActive(true);
    Debug.Log("LEVEL UP");
  }

  public void AddExperience(int amount) {
    int preLevel = levelSystem.GetLevel();
    levelSystem.AddExperience(amount);
    int curLevel = levelSystem.GetLevel();

    if (preLevel != curLevel) {
      LevelUp();
      PauseToggle();
    }
  }

  private void SetLevelSystem() {
    SetExperienceBarSize(levelSystem.GetExperienceNormalized());
    SetLevelNumber(levelSystem.GetLevel());
  }

  private void SetExperienceBarSize(float experienceNormalized) {
    experienceBar.fillAmount = experienceNormalized;
  }

  private void SetLevelNumber(int levelNumber) {
    levelText.text = "LEVEL " + (levelNumber + 1);
  }

  // Ability System
  public void ChooseAbility() {
    PauseToggle();
    chooseAbility.SetActive(false);
  }
}
