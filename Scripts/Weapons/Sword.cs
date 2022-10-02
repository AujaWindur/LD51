using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Sword : Weapon
{
  public Sword(GameObject gameObject) : base (gameObject)
  {
    Type = WeaponType.Melee;
    Unlocked = true;
    Ammunition = 1;
    AmmunitionInClip = 1;
    fireCooldown = 1f;
    reloadCooldown = 0.1f;
    damagePerHit = 3;
    Range = 4.2f;
  }

  public override void Fire(Vector3 dir)
  {
    base.Fire (dir);

    Animator.SetTrigger ("Fire");
    ProjectileManager.SpawnProjectile (ProjectileType.Melee, true,
      Camera.main.transform.position, dir, damagePerHit + Game.Mods.SwordBonusDamage, Range + Game.Mods.SwordRangeMult);
  }

  public override bool FireHasCooledDown()
  {
    return Time.timeSinceLevelLoad - lastFired >= fireCooldown * Game.Mods.SwordAttackSpeedMult;
  }
}