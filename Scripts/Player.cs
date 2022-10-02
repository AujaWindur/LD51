using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
  public float MaxHp = 100f;
  public float Hp;
  [SerializeField] private Slider healthBar;
  [SerializeField] private TextMeshProUGUI hpLabel;
  [SerializeField] private Game game;
  private bool isDead = false;

  private void Awake()
  {
    Hp = MaxHp;
    UpdateHealthBar ();
  }

  public void TakeDamage(float damage)
  {
    if (isDead) return;

    Debug.Log ($"player took {damage} damage!");
    Hp -= damage;
    if (Hp <= 0)
    {
      isDead = true;
      Hp = 0;
      game.OnGameOver ();
    }

    UpdateHealthBar ();
  }

  private void UpdateHealthBar()
  {
    hpLabel.text = Hp.ToString ("0") + " / " + MaxHp.ToString ("0");
    healthBar.value = Hp / MaxHp;
  }

  public void Heal(float hp)
  {
    this.Hp += hp;
    if (this.Hp > MaxHp) this.Hp = MaxHp;
    UpdateHealthBar ();
  }
}