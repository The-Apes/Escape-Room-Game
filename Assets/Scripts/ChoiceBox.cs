using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChoiceBox : MonoBehaviour
{
 [SerializeField] private RectTransform subtitleRectTransform;
 [SerializeField] private GameObject choiceButtonPrefab;
 private ChoiceButton _currentChoiceButton;
 private Image _image;
 private bool elevateText;
 
 private int selectedChoice = 0;
 private int avaliableChoices;
 private List <ChoiceButton> choiceButtons = new List<ChoiceButton>();
 private bool choicesShown = false;
 
 private void Awake()
 {
  _image = GetComponent<Image>();
  if (!subtitleRectTransform) Debug.LogError("subtitle rectTransform in ChoiceBox UI is not assigned");
  ShowChoices(false);
 }

 public void AddChoice(string text, int choiceNum)
 {
   _currentChoiceButton = Instantiate(choiceButtonPrefab, gameObject.transform).GetComponent<ChoiceButton>();
   _currentChoiceButton.text.SetText(choiceNum + ". " + text); 
   _currentChoiceButton.id = choiceNum; // do we need the id in choice button? why don't we just track it here
   choiceButtons.Add(_currentChoiceButton);
 }
 public void ChosenChoice()
 {
  //for the child buttons to call, which will make the box remove it's choice and tell the manager ig
 }

 public void ShowChoices(bool show)
 {
  _image.enabled = show;
  choicesShown = show;
  subtitleRectTransform.position = show ? new Vector3(subtitleRectTransform.position.x, 55, 0) : new Vector3(subtitleRectTransform.position.x, -30, 0);
 }

 public void Navigate(InputAction.CallbackContext context)
 {
  if (!choicesShown) return;
  if (!context.started) return;
  print(context.ReadValue<Vector2>().y);
  choiceButtons[selectedChoice].Deselected();
  if (context.ReadValue<Vector2>().y.Equals(1))
  {
   selectedChoice--;
  }
  else
  {
   selectedChoice++;
  }
  selectedChoice = Mathf.Clamp(selectedChoice, 0, choiceButtons.Count - 1);
  choiceButtons[selectedChoice].Selected();

 }
 public void SelectChoice(InputAction.CallbackContext context)
 {
  if (!choicesShown) return;
  if (!context.started) return;
  print(selectedChoice);
  ChoiceManager.instance.ChosenChoice(selectedChoice);

  foreach (ChoiceButton choiceButton in choiceButtons)
  {
   Destroy(choiceButton.gameObject);
  }

  choiceButtons.Clear();
  ShowChoices(false);
  selectedChoice = 0;
 }

 
}
