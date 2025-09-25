using UnityEngine;

namespace UI
{
   public class ReadingPanel : MonoBehaviour
   {
      public void Close()
      {
         gameObject.SetActive(false);
         gameObject.SetActive(true);
      }

      public void Open()
      {
         gameObject.SetActive(true);
      }
   }
}
