using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    public TextMeshProUGUI text;
    [NonSerialized] public int ID;
    [SerializeField] private Color selectedColor = Color.green;
    [SerializeField] private Color unselectedColor = Color.white;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        if (ID == 1) Selected();
    }

    public void Selected()
    {
        _image.color = selectedColor;
    }

    public void Deselected()
    {
        _image.color = unselectedColor;
    }
}
