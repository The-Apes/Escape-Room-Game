using System;
using TMPro;
using UnityEngine;

namespace UI
{
   public class ReadingPanel : MonoBehaviour
   {
      [SerializeField] private TextMeshProUGUI text;

      private void Start()
      {
         Close();
      }

      public void Open()
      {
         gameObject.SetActive(true);
      }
      public void Close()
      {
         gameObject.SetActive(false);
      }
      public void Read(string textStr)
      {
         text.SetText(textStr);
         Open();
      }
   }
}
