using System.Collections;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
  public int MinAmount = 5;
  public int MaxAmount = 12;

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {
      other.GetComponent<WeaponController> ().AddAmmoToCurrentWeapon (Random.Range (MinAmount, MaxAmount) + Game.Mods.AmmoPickupBonusAmmo);
      Destroy (gameObject);
    }
  }
}