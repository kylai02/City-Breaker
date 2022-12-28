using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Skill choose UI on the bottom of screen
public class SkillIcon : MonoBehaviour {
  private RectTransform _rt;
  private RectTransform _cooldownRt;
  public GameObject cooldownCover;
  
  public int num;
  public Vector2 enlargement;
  public Vector2 defaultScale;
  // Start is called before the first frame update
  void Start() {
    enlargement = new Vector2(125f, 125f);
    _rt = gameObject.GetComponent<RectTransform>();
    _cooldownRt = cooldownCover.GetComponent<RectTransform>();
    // gameObject.name = "Skill n"
    num = gameObject.name[6] - '0';
    defaultScale = _rt.sizeDelta;
  }

  // Update is called once per frame
  void Update() {
        
  }

  public void BeChosen() {
    _rt.sizeDelta = enlargement;
    _cooldownRt.sizeDelta = enlargement;
  }

  public void UnChosen() {
    _rt.sizeDelta = defaultScale;
    _cooldownRt.sizeDelta = defaultScale;
  }
}