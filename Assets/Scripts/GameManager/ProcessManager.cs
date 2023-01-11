using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProcessManager : MonoBehaviour {
  public GameObject menu;
  public Image mask;
  public GameObject intro;
  public Image keepGoing;

  public AudioSource bgm;

  private bool canStart;
  
  void Start() {
    canStart = false;
  }

  void Update() {
    if (canStart) {
      if (Input.GetKeyDown(KeyCode.Space)) {
        keepGoing.gameObject.SetActive(false);
        StopCoroutine(KeepGoingBlank());
        StartCoroutine(FadeOut());
      }
    }
  }

  public void GameStart() {
    mask.gameObject.SetActive(true);
    StartCoroutine(MenuFadeOut());
  }

  IEnumerator MenuFadeOut() {
    while (mask.color.a < 1) {
      float a = mask.color.a;
      a += 0.025f;
      if (a > 1) {
        a = 1;
      }
      
      mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, a);

      yield return new WaitForSeconds(0.025f);
    }

    menu.SetActive(false);
    intro.SetActive(true);
    StartCoroutine(NewsFadeIn());
  }

  IEnumerator NewsFadeIn() {
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
    yield return new WaitForSeconds(5f);
    canStart = true;
    StartCoroutine(KeepGoingBlank());
  }

  IEnumerator KeepGoingBlank() {
    keepGoing.gameObject.SetActive(true);
    
    bool increasing = true;
    while (true) {
      float a = keepGoing.color.a;
      if (increasing) {
        a += 0.025f;
      } else {
        a -= 0.025f;
      }

      if (a > 1) {
        a = 1f;
        increasing = false;
      }
      if (a < 0.25) {
        a = 0.25f;
        increasing = true;
      }

      keepGoing.color = new Color(
        keepGoing.color.r,
        keepGoing.color.g,
        keepGoing.color.b,
        a
      );

      yield return new WaitForSeconds(0.025f);
    }
  }

  IEnumerator FadeOut() {
    mask.gameObject.SetActive(true);
    while (mask.color.a < 1) {
      float a = mask.color.a;
      a += 0.025f;
      bgm.volume -= 0.003125f;
      if (a > 1) {
        a = 1;
      }

      mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, a);

      yield return new WaitForSeconds(0.025f);
    }

    SceneManager.LoadScene(1);
  }
}
