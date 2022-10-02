using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
  public const float ChoiceTimerLength = 10f;
  public static bool IsPaused { get; private set; }

  public delegate void GameStateChangeHandler(GameState gameState);
  public static event GameStateChangeHandler GameStateChange;

  public static Mods Mods;

  public bool DebugPause;
  public Slider Slider;
  public Color FpsModeSliderColor;
  public Color ChoiceModeSliderColor;
  public ChoiceGui ChoiceGui;
  public FirstPersonController FpsController;
  public EnemySpawner EnemySpawner;
  public PickupSpawner PickupSpawner;
  public GameObject FpsUiRoot;
  public GameObject GameOverUiRoot;
  public Player Player;
  public TextMeshProUGUI WavesLabel;

  private Image sliderFill;
  private GameState State = GameState.None;
  private float choiceTimer;

  private void Awake()
  {
    sliderFill = Slider.transform.Find ("Fill Area").Find ("Fill").GetComponent<Image> ();
    ChoiceGui.MadeChoice += ChoiceGui_MadeChoice;
    GameOverUiRoot.SetActive (false);
    GameProgress.Cycle = 0;
    Mods = new Mods ();

    if (DebugPause)
    {
      FindObjectOfType<Deathwall> ().enabled = false;
    }
  }

  private void OnDestroy()
  {
    ChoiceGui.MadeChoice -= ChoiceGui_MadeChoice;
  }

  private void ChoiceGui_MadeChoice()
  {
    SetState (GameState.FPS);
  }

  private void Start()
  {
    if (DebugPause) return;
    SetState (GameState.FPS);
  }

  private void Update()
  {
    if (DebugPause) return;

    if (State == GameState.FPS)
    {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
      choiceTimer += Time.deltaTime * Game.Mods.FpsTimerMod;
      var p = choiceTimer / ChoiceTimerLength;
      Slider.value = 1f - p;
      sliderFill.color = Color.Lerp (FpsModeSliderColor, ChoiceModeSliderColor, p);
      if (choiceTimer >= ChoiceTimerLength)
      {
        SetState (GameState.Choice);
      }
    }
    else if (State == GameState.Choice)
    {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
      choiceTimer += Time.deltaTime * Game.Mods.ChoiceTimerMod;
      var p = choiceTimer / ChoiceTimerLength;
      Slider.value = p;
      sliderFill.color = Color.Lerp (ChoiceModeSliderColor, FpsModeSliderColor, p);
      if (choiceTimer >= ChoiceTimerLength)
      {
        SetState (GameState.FPS);
      }
    }
    else if (State == GameState.GameOver)
    {
      if (Input.GetKeyDown (KeyCode.R))
      {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
      }
    }
  }

  public void OnGameOver()
  {
    SetState (GameState.GameOver);
  }

  private void SetState(GameState newState)
  {
    if (State == newState)
    {
      Debug.LogWarning ($"Tried entering state {newState}, but already in it!");
      return;
    }

    Debug.Log ($"Entering {newState} state.");
    switch (newState)
    {
      case GameState.FPS:
        GameProgress.Cycle++;
        WavesLabel.text = "Waves: " + GameProgress.Cycle.ToString ();
        FpsUiRoot.SetActive (true);
        IsPaused = false;
        Player.Heal (Mods.HpRegenPerWave);
        GameStateChange.Invoke (GameState.FPS);
        FpsController.Locked = false;
        ChoiceGui.Close ();
        Slider.SetDirection (Slider.Direction.LeftToRight, true);
        sliderFill.color = FpsModeSliderColor;
        choiceTimer = 0f;
        Slider.value = 1f - (choiceTimer / ChoiceTimerLength);
        EnemySpawner.SpawnEnemies ();
        PickupSpawner.SpawnPickups ();
        break;

      case GameState.Choice:
        FpsUiRoot.SetActive (false);
        IsPaused = true;
        GameStateChange.Invoke (GameState.Choice);
        FpsController.Locked = true;
        ChoiceGui.Open ();
        Slider.SetDirection (Slider.Direction.RightToLeft, true);
        sliderFill.color = ChoiceModeSliderColor;
        choiceTimer = 0f;
        Slider.value = 1f - (choiceTimer / ChoiceTimerLength);
        break;

      case GameState.GameOver:
        IsPaused = true;
        FpsUiRoot.SetActive (false);
        ChoiceGui.Close ();
        GameStateChange.Invoke (GameState.GameOver);
        FpsController.Locked = true;
        GameOverUiRoot.SetActive (true);
        break;

      default:
        throw new System.Exception ($"Unhandled state: {newState}");
    }

    State = newState;
  }
}