using Objects;
using UnityEngine;

namespace Sound
{
    public class ThunderAmbience : MonoBehaviour
    {
        public AudioSource thunderAudioSource;
        public AudioClip[] thunderClips;
        private const float MinInterval = 10f;
        private const float MaxInterval = 40f;

        private float _nextThunderTime;
        private int _intensity;

        private void Start()
        {
            ScheduleNextThunder();
        }

        private void Update()
        {
            if (!(Time.time >= _nextThunderTime)) return;

            _intensity = Mathf.RoundToInt(SkewedRandom(1f, 5f, 2.5f));
        
            Debug.Log("Lightning Level:"+ _intensity);
            Thunder();
            
            ScheduleNextThunder();
        }
     
        private void Thunder()
        {
            if (!thunderAudioSource) return;
            thunderAudioSource.volume = 1f/(6-_intensity);
            thunderAudioSource.PlayOneShot(thunderClips[Random.Range(0, thunderClips.Length)]);
            
            // for each window, tell them to flash, use thunder intensity
            Window[] windows = FindObjectsByType<Window>(FindObjectsSortMode.None);
            foreach (Window window in windows)
            {                
                window.FlashWindow(_intensity);
            }
        }

        private void ScheduleNextThunder()
        {
            float interval = Random.Range(MinInterval, MaxInterval);
            _nextThunderTime = Time.time + interval;
            Debug.Log("Next thunder in: " + interval + " seconds.");
        }
    
        float SkewedRandom(float min, float max, float skew = 2f) {
            // skew > 1 favors lower values, <1 favors higher ones
            float r = Mathf.Pow(Random.value, skew);
            return Mathf.Lerp(min, max, r);
        }
    }
}
