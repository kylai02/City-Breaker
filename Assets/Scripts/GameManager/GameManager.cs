using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
  [Header("UI References")]
  // Text of levelNum, on the experienceBar 
  public TMP_Text levelText;
  // Bar of exp, on the left top
  public Image experienceBar;
  // Buttons for choose ability
  public GameObject chooseAbility;
  // Alert of T1 to T0
  public GameObject dangerous;

  [Header("System Info")]
  // Whether the game is paused
  public bool isPaused = false;

  [Header("Player Info")]
  // Maintain a Level System
  public LevelSystem levelSystem;
  // Maintain a Ability System
  public AbilitySystem abilitySystem;

  // Tmp for choose ability's index
  private List<int> _chosenArea;
  
  // Start is called before the first frame update
  void Start() {
    chooseAbility.SetActive(false);

    levelSystem = new LevelSystem();
    abilitySystem = new AbilitySystem();

    _chosenArea = new List<int>(3);
    for (int i = 0; i < 3; ++i) _chosenArea.Add(-1);
  }

  // Update is called once per frame
  void Update() {
    if (Input.GetKeyDown(KeyCode.P)) {
      PauseToggle();
    }

    if (Input.GetKeyDown(KeyCode.R)) {
      AddExperience(50);
    }

    SetLevelSystem();
  }

  // Alert for T1 to T0
  public void Alert() {
    dangerous.SetActive(true);
    Invoke("CloseDangerous", 2f);
  }

  // Add exp to levelSystem, if level up, call LevelUp
  public void AddExperience(int amount) {
    int preLevel = levelSystem.GetLevel();
    levelSystem.AddExperience(amount);
    int curLevel = levelSystem.GetLevel();

    if (preLevel != curLevel) {
      LevelUp();
      PauseToggle();
    }
  }

  // Choose ability to abilitySystem
  public void ChooseAbility(int index) {
    abilitySystem.Acquired(_chosenArea[index]);

    ResetChosenArea();
    PauseToggle();
    chooseAbility.SetActive(false);
    CursorToggle();
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

  private void CursorToggle() {
    if (Cursor.visible) {
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
    }
    else {
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
    }
  }

  // Reset chosenArea elms to -1
  private void ResetChosenArea() {
    for (int i = 0; i < 3; ++i) {
      _chosenArea[i] = -1;
    }
  }

  // Set chosenArea elms to the num random picked 
  // And it doesn't be acquired in abilitySystem
  private void SetChosenArea() {
    for (int i = 0; i < 3; ++i) {
      bool gotNum = false;
      while (!gotNum) {
        int n = Random.Range(0, 15);
        if (abilitySystem.isAcquired(n) || _chosenArea.IndexOf(n) != -1) {
          continue;
        }
        else {
          _chosenArea[i] = n;
          gotNum = true;
        }
      }
    }
  }

  // Call by Invoke
  private void CloseDangerous() {
    dangerous.SetActive(false);
  }

  // When level up, let user choose ability
  private void LevelUp() {
    SetChosenArea();
    CursorToggle();
    chooseAbility.SetActive(true);
  }

  // Set level's UI
  private void SetLevelSystem() {
    SetExperienceBarSize(levelSystem.GetExperienceNormalized());
    SetLevelNumber(levelSystem.GetLevel());
  }

  // Set exp bar UI
  private void SetExperienceBarSize(float experienceNormalized) {
    experienceBar.fillAmount = experienceNormalized;
  }

  // Set level num UI
  private void SetLevelNumber(int levelNumber) {
    levelText.text = "LEVEL " + (levelNumber + 1);
  }
}
