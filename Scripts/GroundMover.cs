using System.Collections;
using UnityEngine;

public class GroundMover : MonoBehaviour
{
  //private const float groundLength = 137.7425f;
  private const float groundLength = 137.5625f;

  [SerializeField] private Transform[] groundObjects;

  private Transform player;
  private float zPos;
  private float movedLength;

  private void Awake()
  {
    player = GameObject.FindGameObjectWithTag ("Player").transform;
  }

  private void Update()
  {
    zPos = player.position.z - movedLength;

    if (zPos > groundLength * 1.5f)
    {
      movedLength += groundLength;
      groundObjects[0].transform.position += new Vector3 (0f, 0f, groundLength * groundObjects.Length);
      groundObjects = new Transform[] { groundObjects[1], groundObjects[2], groundObjects[3], groundObjects[0] };
    }
  }
}