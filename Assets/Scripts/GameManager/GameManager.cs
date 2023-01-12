using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Cinemachine;

public class GameManager : MonoBehaviour {
  [Header("UI References")]
  public GameObject inGame;
  public GameObject pauseGame;
  public GameObject gameOver;
  public GameObject success;
  public Image mask;
  public TMP_Text fps;

  public GameObject cam;
  public GameObject gameOverCam;

  // Text of levelNum, on the experienceBar 
  public TMP_Text levelText;
  // Bar of exp, on the left top
  public Image experienceBar;
  public TMP_Text experienceNum;
  // Buttons for choose skill
  public GameObject chooseSkill;
  // Alert of T1 to T0
  public GameObject dangerous;
  public TMP_Text alertCounterUI;

  public Sprite[] sprites;
  public GameObject[] choosenObj;

  public int chosenSkill;

  [Header("System Info")]
  // Whether the game is paused
  public bool isPaused = false;
  public AudioSource alertSound;
  private float _alertTimer;
  private int _alertCounter;

  [Header("Player Info")]
  // Maintain a Level System
  public LevelSystem levelSystem;
  // Maintain a Skill System
  public SkillSystem skillSystem;

  public GameObject fireLaser;
  public bool explosionUpgrade;
  

  // Tmp for choose ability's index
  private List<int> _chosenArea;
  
  private int _eggCtr = 0;

  // Start is called before the first frame update
  void Start() {
    StartCoroutine(FadeIn());
    FindObjectOfType<AudioManager>().Play("BGM");

    Restart();
    _alertTimer = 0;
    _alertCounter = 0;
    explosionUpgrade = false;
    chooseSkill.SetActive(false);

    levelSystem = new LevelSystem();
    skillSystem = new SkillSystem();

    _chosenArea = new List<int>(3);
    for (int i = 0; i < 3; ++i) {
      _chosenArea.Add(-1);
    }

    chosenSkill = 0;

    skillSystem.OnSkillUnlocked += SkillSystem_OnSkillUnlocked;
    // GameObject.Find("Skill " + chosenSkill).GetComponent<SkillIcon>().BeChosen();

    InvokeRepeating(nameof(SuccessCheck), 1f, 1f);
  }

  // Update is called once per frame
  void Update() {
    if (Input.GetKeyDown(KeyCode.P)) {
      if (isPaused) {
        KeepGoing();
      }
      else {
        _eggCtr++;
        if (_eggCtr == 66) {
          FindObjectOfType<AudioManager>().Play("EGG");
        }
        else {
          FindObjectOfType<AudioManager>().Play("Pause");
        }
        PauseMenu();
      }

      PauseToggle();
    }
    
    float mouseCenter = Input.GetAxis("Mouse ScrollWheel");
    if (Input.GetKeyDown(KeyCode.Q) || mouseCenter < 0) {
      ChangeSkill(-1);
    }

    if (Input.GetKeyDown(KeyCode.E) || mouseCenter > 0) {
      ChangeSkill(1);
    }

    if (Input.GetKeyDown(KeyCode.Alpha1)) {
      ChangeSkill(0, 0);
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2)) {
      ChangeSkill(0, 1);
    }
    else if (Input.GetKeyDown(KeyCode.Alpha3)) {
      ChangeSkill(0, 2);
    }
    else if (Input.GetKeyDown(KeyCode.Alpha4)) {
      ChangeSkill(0, 3);
    }
    else if (Input.GetKeyDown(KeyCode.Alpha5)) {
      ChangeSkill(0, 4);
    }

    if (Input.GetKeyDown(KeyCode.R)) {
      AddExperience(1000);
    }

    SetLevelSystem();
    SetAlert();

    // Golden Finger
    if (Input.GetKeyDown(KeyCode.L)) {
      GameOver(transform);
    }

    fps.text = (1f / Time.deltaTime).ToString("0") + " FPS";
    if (Input.GetKeyDown(KeyCode.U)) {
      SetFPS();
    }

  }

  public void ChangeSkill(int parameter, int directive=-1) {
    int preSkill = chosenSkill;
    chosenSkill += parameter;
    if (directive != -1) {
      chosenSkill = directive;
    }

    if (chosenSkill > 4) chosenSkill = 0;
    if (chosenSkill < 0) chosenSkill = 4;

    GameObject.Find("Skill " + preSkill).GetComponent<SkillIcon>().UnChosen();
    GameObject.Find("Skill " + chosenSkill).GetComponent<SkillIcon>().BeChosen();
  }

  // Alert for T1 to T0
  public void Alert() {
    alertSound.Play();
    dangerous.SetActive(true);
    _alertTimer = 120f;
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

  // Choose Skill to skillSystem
  // Called in UI OnClick
  public void ChooseSkill(int index) {
    skillSystem.Unlockskill((SkillSystem.SkillType)_chosenArea[index]);

    ResetChosenArea();
    chooseSkill.SetActive(false);
    PauseToggle();
    CursorToggle();
  }

  public void GameOver(Transform t0) {
    PauseToggle();

    inGame.SetActive(false);
    StartCoroutine(LookT0(t0));
  }

  IEnumerator LookT0(Transform t0) {
    var reference = gameOverCam.GetComponent<CinemachineFreeLook>();
    reference.Follow = t0;
    reference.LookAt = t0;

    cam.SetActive(false);
    gameOverCam.SetActive(true);

    for (int i = 0; i < 360; ++i) {
      reference.m_XAxis.Value++;

      yield return new WaitForSecondsRealtime(0.01f);
    }

    gameOver.SetActive(true);
    FindObjectOfType<AudioManager>().Stop("BGM");
    FindObjectOfType<AudioManager>().Play("GameOver");
    Tweener tweener = gameOver.transform.GetChild(1).DOScale(2f, 0.5f);
    tweener.SetUpdate(true);
  }

  public void AlertCounterChange(int offset) {
    _alertCounter += offset;
  }

  private void PauseMenu() {
    inGame.SetActive(false);
    pauseGame.SetActive(true);
  }

  private void KeepGoing() {
    pauseGame.SetActive(false);
    inGame.SetActive(true);
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
    // if (Cursor.visible) {
    //   Cursor.visible = false;
    //   Cursor.lockState = CursorLockMode.Locked;
    // }
    // else {
    //   Cursor.visible = true;
    //   Cursor.lockState = CursorLockMode.None;
    // }
  }

  private void SkillSystem_OnSkillUnlocked(object sender, SkillSystem.OnSkillUnlockedEventArgs e) {
    switch (e.skillType) {
      case SkillSystem.SkillType.AcidBomb:
        GameObject.Find("Player").GetComponent<AcidBomb>().unlocked = true;
        break;
      case SkillSystem.SkillType.AcidCloud:
        GameObject.Find("Player").GetComponent<AcidCloud>().unlocked = true;
        break;
      case SkillSystem.SkillType.FanFlame:
        GameObject.Find("Player").GetComponent<FanFlame>().unlocked = true;
        break;
      case SkillSystem.SkillType.FireLaser:
        GameObject.Find("Player").GetComponent<FireLaser>().unlocked = true;
        break;
      case SkillSystem.SkillType.ExplosionRangeDouble:
        explosionUpgrade = true;
        break;
      case SkillSystem.SkillType.FistDamageDouble:
        GameObject.Find("Player").GetComponent<Fist>().damage *= 2;
        break;
      case SkillSystem.SkillType.FireCooldownDecrease:
        GameObject.Find("Player").GetComponent<FanFlame>().coolDown /= 2;
        GameObject.Find("Player").GetComponent<FireLaser>().coolDown /= 2;
        break;
      case SkillSystem.SkillType.FireLaserRangeDouble:
        fireLaser.GetComponent<FireLaserAddon>().RangeIncrease();
        break;
      case SkillSystem.SkillType.AcidCooldownDecrease:
        GameObject.Find("Player").GetComponent<AcidBomb>().coolDown /= 2;
        GameObject.Find("Player").GetComponent<AcidCloud>().coolDown /= 2;
        break;
      case SkillSystem.SkillType.AcidCloudLifetimeIncrease:
        GameObject.Find("Player").GetComponent<AcidCloud>().lifetime *= 2;
        break;
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
        int n = Random.Range(3, skillSystem.skillAmount);
        if (!skillSystem.CanUnlockSkill((SkillSystem.SkillType)n) || 
          _chosenArea.IndexOf(n) != -1
        ) {
          continue;
        }
        else {
          _chosenArea[i] = n;
          GameObject button = choosenObj[i];

          int index = 2 * (n - 3);
          button.GetComponent<Image>().sprite = sprites[index];
          SpriteState ss = new SpriteState();
          ss.highlightedSprite = sprites[index + 1];
          ss.pressedSprite = sprites[index + 1];
          ss.disabledSprite = sprites[index + 1];
          button.GetComponent<Button>().spriteState = ss;

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
    FindObjectOfType<AudioManager>().Play("LevelUp");
    SetChosenArea();
    CursorToggle();
    chooseSkill.SetActive(true);
  }

  // Set level's UI
  private void SetLevelSystem() {
    SetExperienceBarSize(levelSystem.GetExperienceNormalized());
    SetLevelNumber(levelSystem.GetLevel());
    SetExperienceNumber(levelSystem.GetExperience(), levelSystem.GetExperienceToNextLevel());
  }

  // Set exp bar UI
  private void SetExperienceBarSize(float experienceNormalized) {
    if (levelSystem.GetLevel() != 7) {
      experienceBar.fillAmount = experienceNormalized;
    }
    else {
      experienceBar.fillAmount = 1;
    }
  }

  // Set level num UI
  private void SetLevelNumber(int levelNumber) {
    if (levelNumber == 7) {
      levelText.fontSize = 30;
      levelText.text = "MAX";
    }
    else {
      levelText.text = (levelNumber + 1).ToString();
    }
  }

  private void SetExperienceNumber(int experience, int experienceToNextLevel) {
    if (levelSystem.GetLevel() != 7) {
      experienceNum.text = experience + " / " + experienceToNextLevel;
    }
    else {
      experienceNum.text = 10000 + " / " + 10000;
    }
  }

  private void Restart() {
  }

  private void SetAlert() {
    if (_alertCounter <= 0) {
      _alertTimer = 0;
    }

    if (_alertTimer > 0) {
      _alertTimer -= Time.deltaTime;
      alertCounterUI.text = "x" + _alertCounter;
    }
    else {
      dangerous.SetActive(false);
    }
  }

  private void SuccessCheck() {
    if (!FindObjectOfType<Building>()) {
      success.SetActive(true);
      inGame.SetActive(false);
      FindObjectOfType<AudioManager>().Play("Success");
      Tweener tweener = success.transform.GetChild(1).DOScale(1f, 0.5f);
      tweener.SetUpdate(true);

      PauseToggle();
    }
  }

  private void SetFPS() {
    if (fps.gameObject.activeInHierarchy) {
      fps.gameObject.SetActive(false); 
    }
    else {
      fps.gameObject.SetActive(true);
    }
  }

  IEnumerator FadeIn() {
    mask.gameObject.SetActive(true);
    while (mask.color.a > 0) {
      float a = mask.color.a;
      a -= 0.025f;
      if (a < 0) {
        a = 0;
      }

      mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, a);

      yield return new WaitForSeconds(0.025f);
    }

    mask.gameObject.SetActive(false);
  }
}
