using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem {
  private int _level;
  private int _experience;
  private int _experienceToNextLevel;
  private List<int> _levelupTable;

  public LevelSystem() {
    _levelupTable = new List<int>(4);
    _levelupTable.Add(1000);
    _levelupTable.Add(2500);
    _levelupTable.Add(5000);
    _levelupTable.Add(10000);

    _level = 0;
    _experience = 0;
    _experienceToNextLevel = _levelupTable[0];
  }

  public void AddExperience(int amount) {
    if (_level != 6) {
      _experience += amount;

      while (_experience >= _experienceToNextLevel) {
        _level++;
        _experience -= _experienceToNextLevel;
        
        if (_level < 4) {
          _experienceToNextLevel = _levelupTable[_level];
        }
        else {
          _experienceToNextLevel = 10000;
        }
        
        if (_level == 6) _experience = 0;
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