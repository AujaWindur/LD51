using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
  public Slider Slider;
  public TextMeshProUGUI NameLabel;
  public TextMeshProUGUI HpLabel;

  internal void Initialize(string name, float hp, float maxHp)
  {
    NameLabel.text = name;
    HpLabel.text = HpString (hp, maxHp);
    Slider.value = hp / maxHp;
  }

  internal void UpdateValue(float hp, float maxHp)
  {
    HpLabel.text = HpString (hp, maxHp);
    Slider.value = hp / maxHp;

  }

  private string HpString(float hp, float maxHp)
  {
    return $"{hp} ({(hp / maxHp * 100f).ToString ("0.00")}%)";
  }
}