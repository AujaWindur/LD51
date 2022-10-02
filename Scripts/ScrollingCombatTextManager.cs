using TMPro;
using UnityEngine;

//Resisting the urge to do an object pool that shares objects between all SCTM's in the scene
public class ScrollingCombatTextManager : MonoBehaviour
{
  [SerializeField] private GameObject textPrefab;
  /// <summary>
  /// Distance SCT can spawn away from 0,0.
  /// x is - and +, y is only +.
  /// </summary>
  [SerializeField] private Vector2 spawnArea;

  public void ShowFloatingDamage(float dmg)
  {
    var s = "-" + dmg.ToString ("0");
    var t = Instantiate (textPrefab);
    t.transform.SetParent (transform, false);
    var sct = t.GetComponent<ScrollingCombatText> ();
    t.GetComponent<TextMeshProUGUI> ().text = s;
    var x = sct.Velocity.x < 0 ? -spawnArea.x * (sct.Velocity.x / sct.MinVelocity.x)
      : spawnArea.x * (sct.Velocity.x / sct.MaxVelocity.x);
    var y = spawnArea.y * (sct.Velocity.y / sct.MaxVelocity.y);
    t.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (x, y);
  }
}