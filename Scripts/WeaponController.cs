using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using UnityEngine;
using System.Linq.Expressions;
using System;
using TMPro;

public class WeaponController : MonoBehaviour
{
  [SerializeField] private Transform socket;
  [SerializeField] private GameObject weaponPistol;

  private FirstPersonController fpsController;
  private new Transform camera;
  private Weapon currentWeapon;
  private int currentWeaponIndex = 0;
  private bool clickStartedDuringPause = false;

  public List<Weapon> weapons = new List<Weapon> ();
  public TextMeshProUGUI AmmoLabel;

  private void Awake()
  {
    fpsController = GetComponent<FirstPersonController> ();
    camera = Camera.main.transform;

    PopulateWeapons ();

    foreach (Transform child in socket)
    {
      child.gameObject.SetActive (false);
    }

    SwapWeapon (WeaponType.Melee);
  }

  private void Start()
  {
    Game.GameStateChange += GameStateChanged;
  }

  private void OnDestroy()
  {
    Game.GameStateChange -= GameStateChanged;
  }

  private void GameStateChanged(GameState gameState)
  {
    if (gameState == GameState.Choice || gameState == GameState.GameOver)
    {
      currentWeapon.Animator.speed = 0f;
    }
    else if (gameState == GameState.FPS)
    {
      currentWeapon.Animator.speed = 1f;
    }
  }

  public void AddAmmoToCurrentWeapon(int amount)
  {
    if (currentWeapon.Type == WeaponType.Melee)
    {
      weapons.Where (x => x.Type == WeaponType.Pistol).First ().Ammunition += amount;
    }
    else
    {
      currentWeapon.Ammunition += amount;
    }
    UpdateAmmo ();
  }

  private void PopulateWeapons()
  {
    weapons.Add (new Sword (socket.Find ("Sword").gameObject));
    weapons.Add (new Pistol (socket.Find ("pistol").gameObject));
  }

  private void Update()
  {
    if (Game.IsPaused)
    {
      if (Input.GetMouseButtonDown (0))
      {
        clickStartedDuringPause = true;
      }
    }

    if (Input.GetMouseButtonUp (0))
    {
      clickStartedDuringPause = false;
    }

    if (Input.GetAxis ("Mouse ScrollWheel") < 0f || Input.GetButtonDown ("NextWeapon"))
    {
      int i = currentWeaponIndex;
      //Will crash the game if no weapon is unlocked :)
      while (true)
      {
        i++;
        if (i > weapons.Count - 1) i = 0;
        if (weapons[i].Unlocked)
        {
          Debug.Log ("NextWeapon to " + weapons[i].Type);
          SwapWeapon (weapons[i].Type);
          break;
        }
      }
    }
    else if (Input.GetAxis ("Mouse ScrollWheel") > 0f)
    {
      int i = currentWeaponIndex;
      //Will crash the game if no weapon is unlocked :)
      while (true)
      {
        i--;
        if (i < 0) i = weapons.Count - 1;
        if (weapons[i].Unlocked)
        {
          Debug.Log ("PrevWeapon to " + weapons[i].Type);
          SwapWeapon (weapons[i].Type);
          break;
        }
      }
    }
    else
    {
      CheckWeaponBinds ();
    }

    if (Input.GetMouseButton (0) && currentWeapon.CanFire () && !clickStartedDuringPause)
    {
      currentWeapon.Fire (camera.forward);
      UpdateAmmo ();
    }
    else if (Input.GetButtonDown ("Reload") && currentWeapon.CanReload ())
    {
      currentWeapon.Reload (out float reloadDuration);
      StartCoroutine (DoReload (reloadDuration, currentWeapon));
    }
    else
    {
      var speed = Mathf.Clamp (fpsController.Velocity.magnitude / 0.015f, 0, 2f);
      currentWeapon.Animator.SetFloat ("VelocityMult", speed);
    }
  }

  private IEnumerator DoReload(float duration, Weapon weapon)
  {
    yield return new WaitForSecondsRealtime (duration);

    int want = weapon.ClipSize - weapon.AmmunitionInClip;
    if (weapon.Ammunition >= want)
    {
      weapon.Ammunition -= want;
      weapon.AmmunitionInClip = weapon.ClipSize;
    }
    else
    {
      weapon.AmmunitionInClip += weapon.Ammunition;
      weapon.Ammunition = 0;
    }

    UpdateAmmo ();
  }

  private void CheckWeaponBinds()
  {
    for (int i = 1; i < 10; i++)
    {
      if (Input.GetButtonDown ("Weapon" + i.ToString ()))
      {
        if (i - 1 < weapons.Count && weapons[i - 1].Unlocked)
        {
          SwapWeapon (weapons[i - 1].Type);
          break;
        }
      }
    }
  }

  private void SwapWeapon(WeaponType newWeapon)
  {
    if (currentWeapon != null && currentWeapon.Type == newWeapon) return;

    //Debug.Log ($"Swapping from {currentWeapon?.Type} to {newWeapon}");
    currentWeapon?.GameObject.SetActive (false);

    for (int i = 0; i < weapons.Count; i++)
    {
      if (weapons[i].Type == newWeapon)
      {
        currentWeapon = weapons[i];
        currentWeaponIndex = i;
      }
    }

    currentWeapon.GameObject.SetActive (true);

    UpdateAmmo ();
  }

  private void UpdateAmmo()
  {
    AmmoLabel.text = currentWeapon.AmmunitionInClip + "/" + currentWeapon.Ammunition;
  }
}