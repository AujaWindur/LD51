using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
  [SerializeField] private LayerMask groundMask;
  [SerializeField] private Vector3 crateSpawnDistance;
  [SerializeField] private float spawnDistanceFromPlayer;
  [SerializeField] private float hpCrateChancePerWavePercent = 50f;
  [SerializeField] private float ammoCrateChancePerWavePercent = 50f;
  [SerializeField] private GameObject hpCratePrefab;
  [SerializeField] private GameObject ammoCratePrefab;

  private Transform player;
  private float furtherPlayerZPos;

  private void Awake()
  {
    player = GameObject.FindGameObjectWithTag ("Player").transform;
  }

  private void Update()
  {
    if (player.transform.position.z > furtherPlayerZPos)
    {
      furtherPlayerZPos = player.transform.position.z;
    }
  }

  public void SpawnPickups()
  {
    var hp = hpCrateChancePerWavePercent * (1f + (Game.Mods.Luck * 0.1f));
    int hpCount = 0;
    if (hp > 100f)
    {
      hpCount = (int) (hp / 100f);
      hp = hp % 100f;
    }
    hpCount += Random.Range (0f, 100f) < hp ? 1 : 0;

    for (int i = 0; i < hpCount; i++)
    {
      var g = Instantiate (hpCratePrefab);
      var pos = new Vector3 ();
      pos.z = furtherPlayerZPos + spawnDistanceFromPlayer + Random.Range (-crateSpawnDistance.z, crateSpawnDistance.z);
      pos.x = Random.Range (-crateSpawnDistance.x, crateSpawnDistance.x);
      pos.y = Random.Range (-crateSpawnDistance.y, crateSpawnDistance.y);

      if (!Physics.Raycast (pos, Vector3.down, out _, 60f, groundMask))
      {
        pos.y += 30f;
      }

      g.transform.position = pos;
    }

    var ammo = ammoCrateChancePerWavePercent * (1f + (Game.Mods.Luck * 0.1f));
    int ammoCount = 0;
    if (ammo > 100f)
    {
      ammoCount = (int) (ammo / 100f);
      ammo = ammo % 100f;
    }
    ammoCount += Random.Range (0f, 100f) < ammo ? 1 : 0;

    for (int i = 0; i < ammoCount; i++)
    {
      var g = Instantiate (ammoCratePrefab);
      var pos = new Vector3 ();
      pos.z = furtherPlayerZPos + spawnDistanceFromPlayer + Random.Range (-crateSpawnDistance.z, crateSpawnDistance.z);
      pos.x = Random.Range (-crateSpawnDistance.x, crateSpawnDistance.x);
      pos.y = Random.Range (-crateSpawnDistance.y, crateSpawnDistance.y);

      if (!Physics.Raycast (pos, Vector3.down, out _, 60f, groundMask))
      {
        pos.y += 30f;
      }

      g.transform.position = pos;
    }
  }
}