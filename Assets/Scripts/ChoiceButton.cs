using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    public TextMeshProUGUI text;
    [NonSerialized] public int id;
    [SerializeField] private Color selectedColor = Color.green;
    [SerializeField] private Color unselectedColor = Color.white;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        if (id == 1) Selected();
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
