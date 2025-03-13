using Player;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(Collider), typeof(Animator))]
    public class Gate: MonoBehaviour
    {
        [SerializeField] private AudioClip sound;

        private Animator anim;
        private Collider collider;

        private static readonly int OpenTrigger = Animator.StringToHash("open");

        private void Start()
        {
            anim = GetComponent<Animator>();
            collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<PlayerController>(out _)) return;

            anim.SetTrigger(OpenTrigger);
            collider.enabled = false;
            PlayerAudio.Instance.Play(sound);
        }
    }
}