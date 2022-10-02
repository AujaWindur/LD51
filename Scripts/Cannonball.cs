using System.Collections;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
  private float timer = 0f;
  private float time = 3f;

  private void Update()
  {
    timer += Time.deltaTime;
    if (timer >= time)
    {
      Destroy (gameObject);
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {
      other.GetComponent<Player> ().TakeDamage (20f);
      Destroy (gameObject);
    }
  }
}