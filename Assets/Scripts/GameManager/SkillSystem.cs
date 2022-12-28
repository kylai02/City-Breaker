using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem {
  public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;
  public class OnSkillUnlockedEventArgs : EventArgs {
    public SkillType skillType;
  }

  public enum SkillType {
    None,
    Acid,
    Fire,
    AcidBomb, 
    AcidCloud, 
    FanFlame, 
    FireLaser, 
    ExplosionRangeDouble, // TODO
    FistDamageDouble,
    FireCooldownDecrease,
    FireLaserRangeDouble,
    AcidCooldownDecrease,
    AcidCloudLifetimeIncrease
  }
  
  public int skillAmount = 13;
  private List<bool> _unlockedSkill;

  public SkillSystem() {
    _unlockedSkill = new List<bool>(skillAmount);
    for (int i = 0; i < skillAmount; ++i) {
      _unlockedSkill.Add(false);
    }

    _unlockedSkill[0] = true;
  }

  public void Unlockskill(SkillType skillType) {
    _unlockedSkill[(int)skillType] = true;
    OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs {skillType = skillType});

    if (skillType == SkillType.AcidBomb || skillType == SkillType.AcidCloud) {
      _unlockedSkill[(int)SkillType.Acid] = true;
    }
    else if (skillType == SkillType.FanFlame || skillType == SkillType.FireLaser) {
      _unlockedSkill[(int)SkillType.Fire] = true;
    }
  }

  public bool IsSkillUnlocked(SkillType skillType) {
    return _unlockedSkill[(int)skillType];
  }

  public bool CanUnlockSkill(SkillType skillType) {
    SkillType skillRequirement = GetSkillRequirement(skillType);

    if (!IsSkillUnlocked(skillType)) {
      return IsSkillUnlocked(skillRequirement);
    }
    return false;
  }

  public SkillType GetSkillRequirement(SkillType skillType) {
    switch (skillType) {
      case SkillType.ExplosionRangeDouble:
      case SkillType.FireCooldownDecrease:
        return SkillType.Fire;

      case SkillType.AcidCooldownDecrease:
        return SkillType.Acid;

      case SkillType.FireLaserRangeDouble:
        return SkillType.FireLaser;

      case SkillType.AcidCloudLifetimeIncrease:
        return SkillType.AcidCloud;
    }

    return SkillType.None;
  }
}
