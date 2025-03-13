using System;
using UnityEngine;

namespace Player
{
    public class PlayerAudio: MonoBehaviour
    {
        [SerializeField] private AudioClip collectCoins;
        [SerializeField] private AudioClip loseCoins;
        [SerializeField] private AudioClip click;
        [SerializeField] private AudioClip fail;
        [SerializeField] private AudioClip win;
        
        [NonSerialized] public static PlayerAudio Instance;

        private AudioSource audioSource;
        
        private void Start()
        {
            Instance = this;
            audioSource = GetComponentInChildren<AudioSource>();
        }

        public void CollectCoins() => audioSource.PlayOneShot(collectCoins);
        public void LoseCoins() => audioSource.PlayOneShot(loseCoins);
        public void Click() => audioSource.PlayOneShot(click);
        public void Fail() => audioSource.PlayOneShot(fail);
        public void Win() => audioSource.PlayOneShot(win);
        public void Play(AudioClip clip) => audioSource.PlayOneShot(clip);
    }
}