using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controls what to spawn/do at what points in the game
/// </summary>
public static class GameProgress
{
  public static int Cycle;

  /// <summary>
  /// Returns one choice for each option(3) for the current cycle
  /// </summary>
  public static Choice[] GetChoices()
  {
    if (Cycle == 1 || Cycle % 5 == 0)
    {
      if (Cycle == 1 || Cycle % 10 == 0)
      {
        return new Choice[] {
          GetChoice (ChoiceType.Enemy),
          GetChoice (ChoiceType.Enemy),
          GetChoice (ChoiceType.Enemy)
        };
      }
      else
      {
        var choices = new Choice[] {
          GetChoice (ChoiceType.EnemyUpgrade | ChoiceType.Enemy),
          GetChoice (ChoiceType.EnemyUpgrade | ChoiceType.Enemy),
          GetChoice (ChoiceType.EnemyUpgrade | ChoiceType.Enemy)
        };
        if (choices.Count (x => x.Type == ChoiceType.EnemyUpgrade) < 1)
        {
          choices[0] = GetChoice (ChoiceType.EnemyUpgrade);
        }
        return choices;
      }
    }
    else
    {
      return new Choice[]{
        GetChoice(ChoiceType.Boon | ChoiceType.BoonBane),
        GetChoice(ChoiceType.Boon | ChoiceType.BoonBane),
        GetChoice(ChoiceType.Boon | ChoiceType.BoonBane)
    };
    }
  }

  private static Choice GetChoice(ChoiceType allowedTypes)
  {
    var choices = new List<Choice> ();

    if (allowedTypes.HasFlag (ChoiceType.Boon))
    {
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Boon,
        TopDescription = "Pistol shoots and reloads 20% faster.",
        OnSelectCallback = () =>
        {
          Game.Mods.PistolAttackSpeedMult += 0.2f;
          Game.Mods.PistolReloadSpeedMult += 0.2f;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Boon,
        TopDescription = "+15% sword attack range.",
        OnSelectCallback = () =>
        {
          Game.Mods.SwordRangeMult += 0.15f;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Boon,
        TopDescription = "+10% sword attack speed.",
        OnSelectCallback = () =>
        {
          Game.Mods.SwordAttackSpeedMult += 0.1f;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Boon,
        TopDescription = "+20% sword attack range and +5% sword attack speed.",
        OnSelectCallback = () =>
        {
          Game.Mods.SwordAttackSpeedMult += 0.05f;
          Game.Mods.SwordRangeMult += 0.2f;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Boon,
        TopDescription = "+10% movement speed.",
        OnSelectCallback = () =>
        {
          Game.Mods.PlayerMovementSpeedMult += 0.1f;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Boon,
        TopDescription = "+4 healing from HP pickups.",
        OnSelectCallback = () =>
        {
          Game.Mods.HpPickupBonusHealing += 4f;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Boon,
        TopDescription = "+1 ammo from ammo pickups.",
        OnSelectCallback = () =>
        {
          Game.Mods.AmmoPickupBonusAmmo += 1;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Boon,
        TopDescription = "+3 luck (more pickups)",
        OnSelectCallback = () =>
        {
          Game.Mods.Luck += 3;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Boon,
        TopDescription = "+3 HP regen (per wave)",
        OnSelectCallback = () =>
        {
          Game.Mods.HpRegenPerWave += 3;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Boon,
        TopDescription = "+2 pistol damage",
        OnSelectCallback = () =>
        {
          Game.Mods.PistolBonusDamage += 2;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Boon,
        TopDescription = "+2 sword damage",
        OnSelectCallback = () =>
        {
          Game.Mods.SwordBonusDamage += 2;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Boon,
        TopDescription = "+10% jump height",
        OnSelectCallback = () =>
        {
          Game.Mods.PlayerJumpHeightMult += 0.1f;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Boon,
        TopDescription = "+25 HP",
        OnSelectCallback = () =>
        {
          var p = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
          p.MaxHp += 25;
          p.Hp += 25;
        }
      });
    }


    if (allowedTypes.HasFlag (ChoiceType.BoonBane))
    {
      choices.Add (new Choice ()
      {
        Type = ChoiceType.BoonBane,
        TopDescription = "+5 sword damage",
        MiddleDescription = "but",
        BottomDescription = "-2 pistol damage",
        OnSelectCallback = () =>
        {
          Game.Mods.SwordBonusDamage += 5;
          Game.Mods.PistolBonusDamage -= 2;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.BoonBane,
        TopDescription = "+50% sword attack range",
        MiddleDescription = "but",
        BottomDescription = "baneballs move 20% faster",
        OnSelectCallback = () =>
        {
          Game.Mods.SwordRangeMult += 0.5f;
          Game.Mods.BaneballSpeedMult += 0.2f;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.BoonBane,
        TopDescription = "+35% movement speed",
        MiddleDescription = "but",
        BottomDescription = "baneballs have 30% larger explosion radius",
        OnSelectCallback = () =>
        {
          Game.Mods.PlayerMovementSpeedMult += 0.35f;
          Game.Mods.BaneballRangeMult += 0.3f;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.BoonBane,
        TopDescription = "+4 ammo from ammo pickups",
        MiddleDescription = "but",
        BottomDescription = "-25% sword attack range",
        OnSelectCallback = () =>
        {
          Game.Mods.AmmoPickupBonusAmmo += 5;
          Game.Mods.SwordRangeMult -= 0.25f;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.BoonBane,
        TopDescription = "+1 ammo pickups per wave",
        MiddleDescription = "but",
        BottomDescription = "-1 pistol damage",
        OnSelectCallback = () =>
        {
          Game.Mods.BonusAmmoPickupPercent += 100f;
          Game.Mods.PistolBonusDamage -= 1;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.BoonBane,
        TopDescription = "+100% jump height",
        MiddleDescription = "but",
        BottomDescription = "Two additional baneball spawns, and +10 baneball HP",
        OnSelectCallback = () =>
        {
          Game.Mods.PlayerJumpHeightMult += 1f;
          Game.Mods.BaneballsPerCycle += 2;
          Game.Mods.BaneballBonusHp += 10f;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.BoonBane,
        TopDescription = "+15% pistol fire rate, +15% pistol reload speed",
        MiddleDescription = "but",
        BottomDescription = "one additional turret spawns",
        OnSelectCallback = () =>
        {
          Game.Mods.PistolAttackSpeedMult += 0.1f;
          Game.Mods.TurretsPerCycle += 1;
          Game.Mods.PistolReloadSpeedMult += 0.1f;
        }
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.BoonBane,
        TopDescription = "+65 HP",
        MiddleDescription = "but",
        BottomDescription = "one additional turret spawns",
        OnSelectCallback = () =>
        {
          var p = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
          p.MaxHp += 65;
          p.Hp += 65;

          Game.Mods.TurretsPerCycle += 1;
        }
      });
    }


    if (allowedTypes.HasFlag (ChoiceType.Enemy))
    {
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Enemy,
        TopDescription = "One additional baneball spawns every wave",
        OnSelectCallback = () => Game.Mods.BaneballsPerCycle += 1
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Enemy,
        TopDescription = "Two additional baneballs spawn every wave",
        OnSelectCallback = () => Game.Mods.BaneballsPerCycle += 2
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Enemy,
        TopDescription = "Two additional turrets spawn every wave",
        OnSelectCallback = () => Game.Mods.TurretsPerCycle += 2
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.Enemy,
        TopDescription = "Three additional turrets spawn every wave",
        OnSelectCallback = () => Game.Mods.TurretsPerCycle += 3
      });
    }


    if (allowedTypes.HasFlag (ChoiceType.EnemyUpgrade))
    {
      choices.Add (new Choice ()
      {
        Type = ChoiceType.EnemyUpgrade,
        TopDescription = "+30 baneball HP",
        OnSelectCallback = () => Game.Mods.BaneballBonusHp += 30f
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.EnemyUpgrade,
        TopDescription = "+10 baneball HP",
        OnSelectCallback = () => Game.Mods.BaneballBonusHp += 10f
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.EnemyUpgrade,
        TopDescription = "+10% baneball explosion radius",
        OnSelectCallback = () => Game.Mods.BaneballRangeMult += 0.1f
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.EnemyUpgrade,
        TopDescription = "+20% baneball explosion radius",
        OnSelectCallback = () => Game.Mods.BaneballRangeMult += 0.2f
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.EnemyUpgrade,
        TopDescription = "+10% baneball movement speed",
        OnSelectCallback = () => Game.Mods.BaneballSpeedMult += 0.1f
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.EnemyUpgrade,
        TopDescription = "+15% turret aiming speed",
        OnSelectCallback = () => Game.Mods.TurretAimMult += 0.15f
      });
      choices.Add (new Choice ()
      {
        Type = ChoiceType.EnemyUpgrade,
        TopDescription = "+50 turret HP",
        OnSelectCallback = () => Game.Mods.TurretHpBonus += 50
      });
    }

    return choices.GetRandomElement ();
  }
}