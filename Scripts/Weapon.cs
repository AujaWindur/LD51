using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon
{
  public WeaponType Type;
  public GameObject GameObject;
  public Animator Animator;
  public bool Unlocked;
  public int Ammunition;
  public int ClipSize;
  public int AmmunitionInClip;
  public Transform ProjectileOrigin;
  public float Range;

  protected float fireCooldown;
  protected float reloadCooldown;
  protected int damagePerHit;

  protected float lastFired;
  protected float lastReloaded;

  public Weapon(GameObject gameObject)
  {
    GameObject = gameObject;
    Animator = gameObject.GetComponent<Animator> ();
    ProjectileOrigin = gameObject.transform.Find ("ProjectileOrigin");
    lastFired = int.MinValue;
    lastReloaded = int.MinValue;
  }

  public virtual void Fire(Vector3 dir)
  {
    lastFired = Time.timeSinceLevelLoad;
  }

  public virtual void Reload(out float duration)
  {
    duration = 0f;
    lastReloaded = Time.timeSinceLevelLoad;
  }

  public virtual bool FireHasCooledDown()
  {
    return Time.timeSinceLevelLoad - lastFired >= fireCooldown;
  }

  public virtual bool ReloadHasCooledDown()
  {
    return Time.timeSinceLevelLoad - lastReloaded >= reloadCooldown;
  }

  public bool CanReload()
  {
    return AmmunitionInClip < ClipSize
      && Ammunition > 0
      && FireHasCooledDown ()
      && ReloadHasCooledDown ();
  }

  public bool CanFire()
  {
    return AmmunitionInClip >= 1
      && FireHasCooledDown ()
      && ReloadHasCooledDown ();
  }
}