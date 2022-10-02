using UnityEngine;

/// <summary>
/// Component used by ScrollingCombatTextManager.
/// Moves recttransform, and then destroys it at end of timer
/// </summary>
public class ScrollingCombatText : MonoBehaviour
{
  public Vector2 MinVelocity => minVelocity;
  public Vector2 MaxVelocity => maxVelocity;
  public Vector2 Velocity { get; private set; }

  [SerializeField] private float maxTimeLimit = 3f;
  [SerializeField] private float minTimeLimit = 2.5f;
  [SerializeField] private Vector2 minVelocity;
  [SerializeField] private Vector2 maxVelocity;

  private RectTransform rectTrans;
  private float timeLimit;
  private float timer;

  private void Awake()
  {
    rectTrans = GetComponent<RectTransform> ();
    timeLimit = Random.Range (minTimeLimit, maxTimeLimit);
    Velocity = Helpers.Random (minVelocity, maxVelocity);
  }

  private void Update()
  {
    rectTrans.anchoredPosition += new Vector2 (Velocity.x, Velocity.y) * Time.deltaTime;
    timer += Time.deltaTime;
    if (timer >= timeLimit)
    {
      Destroy (gameObject);
    }
  }
}