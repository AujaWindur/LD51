using System.Reflection;
using UnityEngine;

public class UpgradeDisplayer : MonoBehaviour
{
  public Vector2 Pos;
  public Vector2 Size;
  public bool Enabled = false;
  public GUIStyle guiStyle;

  private PropertyInfo[] cachedProperties;

  private void Start()
  {
    cachedProperties = typeof (Mods).GetProperties ();
  }

  private void Update()
  {
    if (Input.GetKeyDown (KeyCode.T))
    {
      Enabled = !Enabled;
    }
  }

  private void OnGUI()
  {
    if (!Enabled) return;

    int drawnRects = 0;
    foreach (var property in cachedProperties)
    {
      GUI.Label (new Rect (0f, drawnRects * 30f, 250f, 25f), property.Name + ": " + property.GetValue (Game.Mods), guiStyle);
      drawnRects++;
    }

  }
}