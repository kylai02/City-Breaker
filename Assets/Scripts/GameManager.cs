using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
  public bool isPaused = false;
    // Start is called before the first frame update
  void Start() {
        
  }

    // Update is called once per frame
  void Update() {
    PauseToggle();

  }

  private void PauseToggle() {
    if (Input.GetKeyDown(KeyCode.P)) {
      if (isPaused) {
        Time.timeScale = 1;
        isPaused = false;
      }
      else {
        Time.timeScale = 0;
        isPaused = true;
      }
    }
  }
}
