using System.Collections;
using UnityEngine;

/// <summary>
/// Detaches from parent on awake but follows based on initial offset
/// </summary>
public class FollowParent : MonoBehaviour
{
  private Transform parent;
  private Vector3 offset;

  private void Awake()
  {
    parent = transform.parent;
    offset = transform.position - parent.position;
    transform.SetParent (null);
    name = parent.name + " - " + name;
  }

  private void LateUpdate()
  {
    if (parent)
    {
      transform.position = parent.position + offset;
    }
    else
    {
      Destroy (gameObject);
    }
  }
}