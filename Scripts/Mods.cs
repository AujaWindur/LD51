using System.Collections;
using UnityEngine;

public class Mods
{
  public float Luck { get; set; } = 0;

  public float ChoiceTimerMod { get; set; } = 1f;
  public float FpsTimerMod { get; set; } = 1f;

  public float PistolBonusDamage { get; set; } = 0f;
  public float PistolAttackSpeedMult { get; set; } = 1f;
  public float PistolReloadSpeedMult { get; set; } = 1f;
  public float SwordBonusDamage { get; set; } = 0f;
  public float SwordAttackSpeedMult { get; set; } = 1f;
  public float SwordRangeMult { get; set; } = 1f;
  public float PlayerMovementSpeedMult { get; set; } = 1f;
  public float PlayerJumpHeightMult { get; set; } = 1f;
  public float BaneballSpeedMult { get; set; } = 1f;

  public int BaneballsPerCycle { get; set; } = 0;
  public float BaneballBonusHp { get; set; } = 0f;
  public float BaneballRangeMult { get; set; } = 1f;

  public int AmmoPickupBonusAmmo { get; set; } = 0;
  public float BonusAmmoPickupPercent { get; set; } = 0f;

  public float HpPickupBonusHealing { get; set; } = 0f;
  public float HpRegenPerWave { get; set; } = 0f;

  public float TurretAimMult { get; set; } = 1f;
  public int TurretHpBonus { get; set; } = 0;
  public int TurretsPerCycle { get; set; } = 0;
}