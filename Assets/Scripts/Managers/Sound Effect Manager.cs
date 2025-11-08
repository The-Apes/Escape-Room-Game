using UnityEngine;

namespace Managers
{
    public class SoundEffectManager : MonoBehaviour
    {
        public static SoundEffectManager Instance;
        
        private AudioSource _audioSource;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            _audioSource = GetComponent<AudioSource>();
        }
        
        public void Play2DSoundEffect(AudioClip clip)
        {
            _audioSource.spatialBlend = 0.0f;
            _audioSource.PlayOneShot(clip);
        }

        public void Play3DSoundEffect(AudioClip clip, Vector3 pos)
        {
            AudioSource.PlayClipAtPoint(clip, pos);
        }

    }
}
