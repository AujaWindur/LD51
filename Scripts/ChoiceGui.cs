using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// Scary class written on the ass end of a long day
/// </summary>
public class ChoiceGui : MonoBehaviour
{
  public Transform ChoiceRoot;
  public Image[] Choices;
  public Sprite BoonBackground;
  public Sprite BoonBaneBackground;
  public Sprite EnemyBackground;
  public Sprite EnemyUpgradeBackground;
  public RectTransform SelectedImage;


  public delegate void MadeChoiceHandler();
  public event MadeChoiceHandler MadeChoice = delegate { };

  private Choice[] currentChoices;
  private bool isOpen = false;
  private int selectedOption = -1;

  private void LateUpdate()
  {
    if (!isOpen) return;

    if (Input.mousePosition.x <= Screen.width / 3f)
    {
      if (selectedOption != 0)
      {
        selectedOption = 0;
        SelectedImage.anchoredPosition = new Vector3 (-640f, SelectedImage.anchoredPosition.y);
      }
    }
    else if (Input.mousePosition.x <= (Screen.width / 3f) * 2)
    {
      if (selectedOption != 1)
      {
        selectedOption = 1;
        SelectedImage.anchoredPosition = new Vector3 (0f, SelectedImage.anchoredPosition.y);
      }
    }
    else
    {
      if (selectedOption != 2)
      {
        selectedOption = 2;
        SelectedImage.anchoredPosition = new Vector3 (640f, SelectedImage.anchoredPosition.y);
      }
    }

    if (Input.GetMouseButtonDown (0)
      && RectTransformUtility.RectangleContainsScreenPoint (SelectedImage, Input.mousePosition))
    {
      Debug.Log ($"Chose {selectedOption}: {currentChoices[selectedOption].TopDescription}");
      currentChoices[selectedOption].OnSelectCallback ();
      MadeChoice.Invoke ();
    }
  }

  internal void Close()
  {
    isOpen = false;
    selectedOption = -1;
    ChoiceRoot.gameObject.SetActive (false);
  }


  internal void Open()
  {
    isOpen = true;
    selectedOption = 1;
    SelectedImage.anchoredPosition = new Vector3 (0f, SelectedImage.anchoredPosition.y);
    ChoiceRoot.gameObject.SetActive (true);
    currentChoices = GameProgress.GetChoices ();

    Assert.AreEqual (currentChoices.Length, 3);
    Assert.AreEqual (Choices.Length, 3);

    for (int i = 0; i < currentChoices.Length; i++)
    {
      var topLabel = Choices[i].transform.Find ("Top").GetComponent<TextMeshProUGUI> ();
      topLabel.text = currentChoices[i].TopDescription;

      var midLabel = Choices[i].transform.Find ("Middle").GetComponent<TextMeshProUGUI> ();
      midLabel.text = currentChoices[i].MiddleDescription;

      var bottomLabel = Choices[i].transform.Find ("Bottom").GetComponent<TextMeshProUGUI> ();
      bottomLabel.text = currentChoices[i].BottomDescription;

      if (currentChoices[i].Type == ChoiceType.Boon)
      {
        topLabel.color = Color.white;
        Choices[i].sprite = BoonBackground;
      }
      else if (currentChoices[i].Type == ChoiceType.BoonBane)
      {
        topLabel.color = Color.black;
        midLabel.color = Color.white;
        bottomLabel.color = Color.white;
        Choices[i].sprite = BoonBaneBackground;
      }
      else if (currentChoices[i].Type == ChoiceType.Enemy)
      {
        topLabel.color = Color.white;
        Choices[i].sprite = EnemyBackground;
      }
      else if (currentChoices[i].Type == ChoiceType.EnemyUpgrade)
      {
        topLabel.color = Color.white;
        Choices[i].sprite = EnemyUpgradeBackground;
      }
    }
  }
}