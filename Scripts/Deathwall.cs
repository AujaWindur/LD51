using System.Collections;
using System.Threading;
using UnityEngine;

public class Deathwall : MonoBehaviour
{
  //pivot of mesh if fucked, but moving it fucks the uvs, so we do some hacky shit instead
  private const float DistanceFromPivotToInstaKill = 41.875f;
  private const float DistanceFromPivotToBeginning = 28.125f;
  private const float DmgCooldown = 1f;

  public float Speed;
  public float DmgPerTickAtWall = 50f;

  private Transform player;
  private float dmgTimer;

  private void Awake()
  {
    player = GameObject.FindGameObjectWithTag ("Player").transform;
  }

  private void Update()
  {
    if (Game.IsPaused) return;

    if (Vector3.Distance (player.position, transform.position) >= 35f)
    {
      transform.position += new Vector3 (0f, 0f, Speed * 6f * Time.deltaTime);
    }
    else
    {
      transform.position += new Vector3 (0f, 0f, Speed * Time.deltaTime);
    }

    float dist = player.transform.position.z - (transform.position.z - DistanceFromPivotToInstaKill);
    float dmgDist = DistanceFromPivotToInstaKill - DistanceFromPivotToBeginning;
    float p = 1f - Mathf.Clamp (dist / dmgDist, 0f, 1f);

    dmgTimer -= Time.deltaTime;
    if (p > 0f && dmgTimer < 0f)
    {
      dmgTimer = DmgCooldown;

      if (p == 1f)
      {
        player.GetComponent<Player> ().TakeDamage (float.PositiveInfinity);
      }
      else
      {
        player.GetComponent<Player> ().TakeDamage (DmgPerTickAtWall * p);
      }
    }
  }
}