using UnityEngine;

namespace Objects
{
    public class Window : MonoBehaviour
    {
        [SerializeField] private GameObject flashGlass1;
        [SerializeField] private GameObject flashGlass2;
        [SerializeField] private GameObject flashGlass3;
        [SerializeField] private GameObject flashGlass4;
        [SerializeField] private GameObject flashGlass5;
        [SerializeField] private Light windowLight;
        private GameObject _glass;

    
        public void FlashWindow(int intensity)
        {
            _glass = intensity switch
            {
                1 => flashGlass1,
                2 => flashGlass2,
                3 => flashGlass3,
                4 => flashGlass4,
                5 => flashGlass5,
                _ => flashGlass1
            };
        
            windowLight.intensity = intensity switch
            {
                1 => 1f,
                2 => 2f,
                3 => 10.5f,
                4 => 14.125f,
                5 => 20f,
                _ => 1f
            };
        
        
            _glass.SetActive(true);
            windowLight.gameObject.SetActive(true);

            float duration = intensity switch
            {
                1 => 0.1f,
                2 => 0.15f,
                3 => 0.2f,
                4 => 0.25f,
                5 => 0.35f,
                _ => 0.1f
            };
            Invoke(nameof(StopFlash), duration); //Invoke, where have you been all of my life?
        }
        private void StopFlash()
        {
            windowLight.gameObject.SetActive(false);
            _glass.SetActive(false);
        }
    }
}
