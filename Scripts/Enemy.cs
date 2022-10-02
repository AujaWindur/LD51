using UnityEngine;

public class Enemy : MonoBehaviour
{
  public string Name;
  public bool IsDead = false;
  public float Hp;
  public float MaxHp;
  public ScrollingCombatTextManager ScrollingCombatText;

  public delegate void DiedHandler();
  public event DiedHandler Died = delegate { };

  private HealthBar healthBar;

  private void Awake()
  {
    healthBar = GetComponent<HealthBar> ();
    healthBar.Initialize (Name, Hp, MaxHp);
  }

  public void TakeDamage(float damage)
  {
    if (IsDead) return;

    ScrollingCombatText.ShowFloatingDamage (damage);
    Hp -= damage;
    if (Hp <= 0)
    {
      IsDead = true;
      Died.Invoke ();
      Debug.Log ($"{name}:{Name} died!");
      Destroy (gameObject);
    }
    healthBar.UpdateValue (Hp, MaxHp);

  }
}
