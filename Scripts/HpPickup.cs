using System.Collections;
using UnityEngine;

public class HpPickup : MonoBehaviour
{
  public float Healing = 15f;

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {
      other.GetComponent<Player> ().Heal (Healing + Game.Mods.HpPickupBonusHealing);
      Destroy (gameObject);
    }
  }
}