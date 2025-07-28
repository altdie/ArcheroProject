using UnityEngine;

namespace Project.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;

        [SerializeField] private AudioClip shotClip;
        [SerializeField] private AudioClip enemyDestroyedClip;
        [SerializeField] private AudioClip backgroundMusic;

        public void PlayShotSound() => sfxSource.PlayOneShot(shotClip);

        public void PlayEnemyDestroyedSound() => sfxSource.PlayOneShot(enemyDestroyedClip);

        public void PlayBackgroundMusic()
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}
