using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The Skill choose UI on the bottom of screen
public class SkillIcon : MonoBehaviour {
  private RectTransform _rt;
  private RectTransform _cooldownRt;
  public GameObject cooldownCover;

  public int num;
  public float enlargement;
  public Vector2 iconScale;
  public Vector2 maskScale;

  public bool isChosen;
  // Start is called before the first frame update
  void Start() {
    enlargement = 1.25f;
    _rt = gameObject.GetComponent<RectTransform>();
    _cooldownRt = cooldownCover.GetComponent<RectTransform>();
    // gameObject.name = "Skill n"
    num = gameObject.name[6] - '0';
    iconScale = _rt.sizeDelta;
    maskScale = _cooldownRt.sizeDelta;
  }

  public void BeChosen() {
    isChosen = true;
    _rt.sizeDelta = iconScale * enlargement;
    _cooldownRt.sizeDelta = maskScale * enlargement;
  }

  public void UnChosen() {
    isChosen = false;
    _rt.sizeDelta = iconScale;
    _cooldownRt.sizeDelta = maskScale;
  }
}