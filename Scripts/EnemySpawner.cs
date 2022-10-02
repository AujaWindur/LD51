using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
  [SerializeField] private Vector3 enemySpawnDistance;
  [SerializeField] private float spawnDistanceFromPlayer;
  [SerializeField] private GameObject baneballPrefab;
  [SerializeField] private GameObject turretPrefab;
  [SerializeField] private LayerMask groundMask;

  //remove dict, use count from Mods instead
  private Dictionary<EnemyTypes, int> enemies = new Dictionary<EnemyTypes, int> ();
  private Transform player;
  private float furtherPlayerZPos;

  private void Awake()
  {
    player = GameObject.FindGameObjectWithTag ("Player").transform;
    enemies.Add (EnemyTypes.Baneball, 1);
    enemies.Add (EnemyTypes.Turret, 1);
  }

  private void Update()
  {
    if (player.transform.position.z > furtherPlayerZPos)
    {
      furtherPlayerZPos = player.transform.position.z;
    }
  }

  public void SpawnEnemies()
  {
    foreach (var enemy in enemies)
    {
      Debug.Log ($"Spawning {Game.Mods.BaneballsPerCycle} {enemy.Key}");
      switch (enemy.Key)
      {
        case EnemyTypes.Baneball:
          for (int i = 0; i < Game.Mods.BaneballsPerCycle; i++)
          {
            var g = Instantiate (baneballPrefab);
            var pos = new Vector3 ();
            pos.z = furtherPlayerZPos + spawnDistanceFromPlayer + Random.Range (-enemySpawnDistance.z, enemySpawnDistance.z);
            pos.x = Random.Range (-enemySpawnDistance.x, enemySpawnDistance.x);
            pos.y = 20f;

            if (Physics.Raycast (pos, Vector3.down, out RaycastHit rayHit, 40f, groundMask))
            {
              pos.y = rayHit.point.y + 1f;
              g.transform.position = pos;
            }
            else
            {
              Debug.LogError ($"DIDNT FIND THE GROUND AT {pos}!");
            }
          }
          break;

        case EnemyTypes.Turret:
          for (int i = 0; i < Game.Mods.TurretsPerCycle; i++)
          {
            var g = Instantiate (turretPrefab);
            var pos = new Vector3 ();
            pos.z = furtherPlayerZPos + spawnDistanceFromPlayer + Random.Range (-enemySpawnDistance.z, enemySpawnDistance.z);
            pos.x = Random.Range (-enemySpawnDistance.x, enemySpawnDistance.x);
            pos.y = 20f;

            if (Physics.Raycast (pos, Vector3.down, out RaycastHit rayHit, 40f, groundMask))
            {
              pos.y = rayHit.point.y;
              g.transform.position = pos;
              g.transform.up = rayHit.normal;
            }
            else
            {
              Debug.LogError ($"DIDNT FIND THE GROUND AT {pos}!");
            }
          }
          break;

        default:
          throw new System.Exception ($"Unhandled enemy type: {enemy.Key}");
      }
    }
  }
}