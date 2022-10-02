using System.Collections;
using UnityEngine;

public static class Helpers
{
  public static Vector2 Random(Vector2 min, Vector2 max)
  {
    return new Vector2 (UnityEngine.Random.Range (min.x, max.x),
      UnityEngine.Random.Range (min.y, max.y));
  }
}