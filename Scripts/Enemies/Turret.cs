using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
  public float RotationSpeed;
  public float GunRotationSpeed;
  public float TurretMinAngle;
  private Transform player;
  private Transform playerCam;
  private Transform gunTrans;
  private Quaternion TurretDefaultRotation;
  public float ShotCooldown;
  private float shotTimer;
  public float MinShootingAngle;
  private float directDamage;
  public float Range = 100f;
  private Enemy enemy;

  private void Awake()
  {
    player = GameObject.FindGameObjectWithTag ("Player").transform;
    playerCam = Camera.main.transform;
    gunTrans = transform.Find ("TurretGun");
    TurretDefaultRotation = gunTrans.localRotation;
    enemy = GetComponent<Enemy> ();
    enemy.MaxHp += Game.Mods.TurretHpBonus;
    enemy.Hp += Game.Mods.TurretHpBonus;
  }

  private void Update()
  {
    if (Game.IsPaused) return;

    shotTimer -= Time.deltaTime;

    Plane p = new Plane (transform.up, gunTrans.position);
    Ray r = new Ray (playerCam.position, -Vector3.up);
    if (p.Raycast (r, out float dist))
    {
      var point = r.GetPoint (dist);
      var dir = point - gunTrans.position;
      var lookRot = Quaternion.LookRotation (dir, transform.up);
      var newRot = Quaternion.RotateTowards (transform.rotation, lookRot, RotationSpeed * Game.Mods.TurretAimMult * Time.deltaTime);
      transform.rotation = newRot;

      if (Vector3.Angle (newRot.eulerAngles, lookRot.eulerAngles) < TurretMinAngle)
      {
        var turretDir = playerCam.position - gunTrans.position;
        var turretLook = Quaternion.LookRotation (turretDir);
        gunTrans.rotation = Quaternion.RotateTowards (gunTrans.rotation, turretLook, GunRotationSpeed * Game.Mods.TurretAimMult * Time.deltaTime);

        if (Vector3.Angle (gunTrans.eulerAngles, turretLook.eulerAngles) < MinShootingAngle
          && Vector3.Distance (gunTrans.position, player.position) < Range)
        {
          TryShoot ();
        }
        return;
      }
    }

    gunTrans.localRotation = Quaternion.RotateTowards (gunTrans.localRotation, TurretDefaultRotation, GunRotationSpeed * Game.Mods.TurretAimMult * Time.deltaTime);
  }

  private void TryShoot()
  {
    if (shotTimer < 0f)
    {
      shotTimer = ShotCooldown;
      ProjectileManager.SpawnProjectile (ProjectileType.Cannonball, false, gunTrans.position,
        gunTrans.forward, directDamage, Range);
    }
  }
}