using System.Collections;
using UnityEngine;

[ExecuteAlways]
public class LookAtPlayerSingleAxis : MonoBehaviour
{
  public Vector3 OffsetAngles;
  private Transform player;

  private void Awake()
  {
    player = GameObject.FindGameObjectWithTag ("Player").transform;
  }

  private void Update()
  {
    var tarPos = player.position;
    tarPos.y = transform.position.y;

    var newRot = Quaternion.LookRotation (tarPos - transform.position, Vector3.up).eulerAngles;
    transform.eulerAngles = newRot + OffsetAngles;

  }
}