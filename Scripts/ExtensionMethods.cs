using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class ExtensionMethods
{
  public static T GetRandomElement<T>(this T[] arr)
  {
    return arr[UnityEngine.Random.Range (0, arr.Length)];
  }
  public static T GetRandomElement<T>(this List<T> arr)
  {
    return arr[UnityEngine.Random.Range (0, arr.Count)];
  }
}