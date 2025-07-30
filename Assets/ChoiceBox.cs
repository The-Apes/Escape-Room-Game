using System;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceBox : MonoBehaviour
{
 [SerializeField] private RectTransform subtitleRectTransform;
 [SerializeField] private GameObject choiceButtonPrefab;
 private ChoiceButton _currentChoiceButton;
 private Image _image;

 private void Awake()
 {
  _image = GetComponent<Image>();
  if (!subtitleRectTransform) Debug.LogError("subtitle rectTransform in ChoiceBox UI is not assigned");
  ShowChoices(false);
 }

 public void AddChoice(string text, int choiceNum)
 {
   _currentChoiceButton = Instantiate(choiceButtonPrefab).GetComponent<ChoiceButton>();
   _currentChoiceButton.text.SetText(choiceNum + ". " + text); 
 }
 public void ChosenChoice()
 {
  //for the child buttons to call, which will make the box remove it's choice and tell the manager ig
 }

 public void ShowChoices(bool show)
 {
  _image.enabled = show;

  subtitleRectTransform.position = show ? new Vector3(subtitleRectTransform.position.x, 15, 0) : new Vector3(subtitleRectTransform.position.x, -30, 0);
 }
 
}
