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
 private bool _elevateText;
 
 private int _selectedChoice;
 private int _avaliableChoices;
 private List <ChoiceButton> _choiceButtons = new List<ChoiceButton>();
 private bool _choicesShown;
 
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
   _currentChoiceButton.ID = choiceNum; // do we need the id in choice button? why don't we just track it here
   _choiceButtons.Add(_currentChoiceButton);
 }
 public void ChosenChoice()
 {
  //for the child buttons to call, which will make the box remove it's choice and tell the manager ig
 }

 public void ShowChoices(bool show)
 {
  _image.enabled = show;
  _choicesShown = show;
  subtitleRectTransform.position = show ? new Vector3(subtitleRectTransform.position.x, 55, 0) : new Vector3(subtitleRectTransform.position.x, -30, 0);
 }

 public void Navigate(InputAction.CallbackContext context)
 {
  if (!_choicesShown) return;
  if (!context.started) return;
  print(context.ReadValue<float>());
  _choiceButtons[_selectedChoice].Deselected();
  if (context.ReadValue<float>().Equals(1))
  {
   _selectedChoice--;
  }
  else
  {
   _selectedChoice++;
  }
  _selectedChoice = Mathf.Clamp(_selectedChoice, 0, _choiceButtons.Count - 1);
  _choiceButtons[_selectedChoice].Selected();

 }
 public void SelectChoice(InputAction.CallbackContext context)
 {
  if (!_choicesShown) return;
  if (!context.started) return;
  print(_selectedChoice);
  ChoiceManager.instance.ChosenChoice(_selectedChoice);

  foreach (ChoiceButton choiceButton in _choiceButtons)
  {
   Destroy(choiceButton.gameObject);
  }

  _choiceButtons.Clear();
  ShowChoices(false);
  _selectedChoice = 0;
 }

 
}
