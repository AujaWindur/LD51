using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Pistol : Weapon
{
  public Pistol(GameObject gameObject) : base (gameObject)
  {
    Type = WeaponType.Pistol;
    Unlocked = true;
    Ammunition = 21;
    ClipSize = 7;
    AmmunitionInClip = ClipSize;
    fireCooldown = 0.4f;
    reloadCooldown = 2f;
    damagePerHit = 4;
    Range = 200f;
  }

  public override void Fire(Vector3 dir)
  {
    base.Fire (dir);

    if (Animator.GetCurrentAnimatorStateInfo (0).IsName ("PistolShot"))
    {
      Animator.Play ("PistolShot", -1, 0f);
    }
    else
    {
      Animator.SetTrigger ("Fire");
    }

    AmmunitionInClip -= 1;
    ProjectileManager.SpawnProjectile (ProjectileType.Bullet_Pistol, true,
      Camera.main.transform.position, dir, damagePerHit + Game.Mods.PistolBonusDamage, Range);
  }

  public override void Reload(out float duration)
  {
    base.Reload (out _);
    duration = reloadCooldown * Game.Mods.PistolReloadSpeedMult;
    
    Animator.SetFloat ("ReloadMult", 1f + Game.Mods.PistolReloadSpeedMult);
    Animator.SetTrigger ("Reload");
  }

  public override bool FireHasCooledDown()
  {
    return Time.timeSinceLevelLoad - lastFired >= fireCooldown * Game.Mods.PistolAttackSpeedMult;
  }

  public override bool ReloadHasCooledDown()
  {
    return Time.timeSinceLevelLoad - lastReloaded >= reloadCooldown * Game.Mods.PistolReloadSpeedMult;
  }


}