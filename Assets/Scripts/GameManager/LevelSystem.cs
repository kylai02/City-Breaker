using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem {
  private int _level;
  private int _experience;
  private int _experienceToNextLevel;
  private int _experienceOffset;

  public LevelSystem() {
    _level = 0;
    _experience = 0;
    _experienceToNextLevel = 100;
    _experienceOffset = 10;
  }

  public void AddExperience(int amount) {
    if (_level != 9) {
      _experience += amount;

      if (_experience >= _experienceToNextLevel) {
        _level++;
        _experience -= _experienceToNextLevel;
        _experienceToNextLevel += _experienceOffset;
        
        if (_level == 9) _experience = 0;
      }
    }
  }

  public int GetLevel() {
    return _level;
  }

  public float GetExperienceNormalized() {
    return (float)_experience / _experienceToNextLevel;
  }
}