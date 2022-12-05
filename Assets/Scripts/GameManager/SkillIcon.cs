using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIcon : MonoBehaviour {
  private RectTransform _rt;
  
  public int num;
  public Vector2 enlargement;
  public Vector2 defaultScale;
  // Start is called before the first frame update
  void Start() {
    _rt = gameObject.GetComponent<RectTransform>();
    num = gameObject.name[6] - '0';
    defaultScale = _rt.sizeDelta;
    enlargement = new Vector2(125f, 125f);
  }

  // Update is called once per frame
  void Update() {
        
  }

  public void BeChosen() {
    _rt.sizeDelta = enlargement;
  }

  public void UnChosen() {
    _rt.sizeDelta = defaultScale;
  }
}