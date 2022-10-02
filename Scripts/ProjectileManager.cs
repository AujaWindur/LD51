using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
  public LayerMask PlayerMask;
  public LayerMask EnemyMask;
  public Transform BulletHoleParent;
  public GameObject ImpactEffect;
  public GameObject CannonballPrefab;

  [SerializeField] private GameObject[] smallBulletPrefabs;
  [SerializeField] private int maxPooledSmallBullets = 50;
  [SerializeField] private int MaxBulletHoles = 50;

  private static ProjectileManager instance;
  private Queue<GameObject> smallBulletPool = new Queue<GameObject> ();
  private Queue<GameObject> spawnedBullets = new Queue<GameObject> ();

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Debug.LogWarning ("Instance of ProjectileManager already exists,"
      + $" destroying new instance on {name} in scene: {gameObject.scene.name}!");
      Destroy (gameObject);
      return;
    }
  }

  private void OnDestroy()
  {
    instance = null;
  }

  public static void SpawnProjectile(ProjectileType projectileType, bool shotByPlayer,
    Vector3 origin, Vector3 direction, float damage, float range)
  {
    damage = Mathf.Clamp (damage, 0f, float.MaxValue);

    if (projectileType == ProjectileType.Bullet_Pistol)
    {
      if (Physics.Raycast (origin, direction, out RaycastHit rayHit, range, instance.PlayerMask, QueryTriggerInteraction.Ignore))
      {
        //Hit enemy
        if (rayHit.collider.gameObject.layer == 7)
        {
          Debug.Log ("Hit enemy: " + rayHit.collider.gameObject.name);
          rayHit.collider.GetComponent<Enemy> ().TakeDamage (damage);
        }
        else
        {
          Debug.Log ("Hit object: " + rayHit.collider.gameObject.name);
          var impact = instance.InstantiateSmallBullet ();
          impact.transform.forward = -rayHit.normal;
          //random so we avoid z buffering or w.e. its called
          impact.transform.position = rayHit.point - (impact.transform.forward * Random.Range (0.001f, 0.01f));
          impact.transform.Rotate (rayHit.normal, Random.Range (0f, 360f), Space.World);

          var p = Instantiate (instance.ImpactEffect);
          p.transform.position = rayHit.point;
          p.transform.forward = rayHit.normal;
        }
      }
    }
    else if (projectileType == ProjectileType.Melee)
    {
      if (Physics.Raycast (origin, direction, out RaycastHit rayHit, range, instance.PlayerMask, QueryTriggerInteraction.Ignore))
      {
        //Hit enemy
        if (rayHit.collider.gameObject.layer == 7)
        {
          Debug.Log ("Hit enemy: " + rayHit.collider.gameObject.name);
          rayHit.collider.GetComponent<Enemy> ().TakeDamage (damage);
        }
        else
        {
          Debug.Log ("Hit object: " + rayHit.collider.gameObject.name);

          var p = Instantiate (instance.ImpactEffect);
          p.transform.position = rayHit.point;
          p.transform.forward = rayHit.normal;
        }
      }
    }
    else if (projectileType == ProjectileType.Cannonball)
    {
      var g = Instantiate (instance.CannonballPrefab);
      g.transform.position = origin;
      g.GetComponent<Rigidbody> ().velocity = direction * 80f;
    }
  }

  private GameObject InstantiateSmallBullet()
  {
    if (smallBulletPool.Count >= 1)
    {
      var g = smallBulletPool.Dequeue ();
      g.SetActive (true);
      return g;
    }
    else
    {
      var g = Instantiate (smallBulletPrefabs.GetRandomElement ());
      g.transform.SetParent (BulletHoleParent);
      spawnedBullets.Enqueue (g);
      if (spawnedBullets.Count >= MaxBulletHoles)
      {
        DeleteSmallBullet (spawnedBullets.Dequeue ());
      }
      return g;
    }
  }

  private void DeleteSmallBullet(GameObject objectToDestroy)
  {
    if (smallBulletPool.Count < maxPooledSmallBullets)
    {
      smallBulletPool.Enqueue (objectToDestroy);
      objectToDestroy.SetActive (false);
    }
    else
    {
      Destroy (objectToDestroy);
    }
  }
}