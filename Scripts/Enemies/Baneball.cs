using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Baneball : MonoBehaviour
{
  public float Speed;
  public float ExplosionDamage = 15f;
  public float TouchRange;
  public GameObject ExplosionParticles;

  private new Rigidbody rigidbody;
  private Transform player;
  private Enemy enemy;
  private Vector3 savedVel;
  private Vector3 savedAngularVel;

  private bool paused;

  private void Awake()
  {
    rigidbody = GetComponent<Rigidbody> ();
    player = GameObject.FindGameObjectWithTag ("Player").transform;
    enemy = GetComponent<Enemy> ();
    enemy.Died += Baneball_Died;
    enemy.MaxHp += Game.Mods.BaneballBonusHp;
    enemy.Hp += Game.Mods.BaneballBonusHp;

    Game.GameStateChange += GameStateChange;
  }

  private void OnDestroy()
  {
    Game.GameStateChange -= GameStateChange;
  }

  private void GameStateChange(GameState gameState)
  {
    if (gameState == GameState.Choice)
    {
      paused = true;
      savedVel = rigidbody.velocity;
      savedAngularVel = rigidbody.angularVelocity;
      rigidbody.velocity = Vector3.zero;
      rigidbody.angularVelocity = Vector3.zero;
      rigidbody.isKinematic = true;
    }
    else if (gameState == GameState.FPS)
    {
      paused = false;
      rigidbody.velocity = savedVel;
      rigidbody.angularVelocity = savedAngularVel;
      rigidbody.isKinematic = false;
    }
  }

  private void Baneball_Died()
  {
    Explode ();
  }

  private void FixedUpdate()
  {
    if (paused) return;

    var dir = player.position - transform.position;
    Debug.DrawRay (transform.position, dir);
    rigidbody.AddForce (dir.normalized * (Speed * Game.Mods.BaneballSpeedMult), ForceMode.Force);

    var dist = Vector3.Distance (player.position, transform.position);
    if (dist <= TouchRange * Game.Mods.BaneballRangeMult)
    {
      Explode ();
    }
  }

  private void Explode()
  {
    enemy.TakeDamage (float.PositiveInfinity);
    enemy.Died -= Baneball_Died;

    var dist = Vector3.Distance (player.position, transform.position);
    if (dist <= TouchRange * Game.Mods.BaneballRangeMult)
    {
      player.GetComponent<Player> ().TakeDamage (ExplosionDamage);
    }

    //Idk what works and what doesnt, havent tested enough
    var g = Instantiate (ExplosionParticles);
    g.transform.position = transform.position;
    var p = g.GetComponent<ParticleSystem> ();
    var m = p.main;
    var size = m.startSize;
    size.constantMax = size.constantMax * Game.Mods.BaneballRangeMult;
    size.constantMin = size.constantMin * Game.Mods.BaneballRangeMult;
    m.startSize = size;

    var emission = p.emission;
    var burst = emission.GetBurst (0);
    burst.count = (int) (burst.count.constant * Game.Mods.BaneballRangeMult);
    emission.SetBurst (0, burst);

    var shape = p.shape;
    shape.radius *= Game.Mods.BaneballRangeMult;
  }
}