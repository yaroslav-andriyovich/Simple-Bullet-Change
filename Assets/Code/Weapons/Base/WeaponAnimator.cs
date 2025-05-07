using UnityEngine;

namespace Code.Weapons.Base
{
    public class WeaponAnimator : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private Animator _animator;
        
        [Header("Audio")]
        [SerializeField] private AudioClip _fireClip;
        [SerializeField] private AudioClip _reloadingClip;
        [SerializeField] private AudioSource _audio;

        [Header("VFX")]
        [SerializeField] private ParticleSystem _muzzleFlash;

        private static readonly int triggerFire = Animator.StringToHash("Fire");
        private static readonly int triggerReloading = Animator.StringToHash("Reloading");

        private void Awake()
        {
            _weapon.OnFire += PlayFire;
            _weapon.OnReloading += PlayReloading;
        }

        private void OnDestroy()
        {
            _weapon.OnFire -= PlayFire;
            _weapon.OnReloading -= PlayReloading;
        }

        private void PlayFire()
        {
            _muzzleFlash.Play();
            _animator.SetTrigger(triggerFire);
            PlayAudio(_fireClip);
        }

        private void PlayReloading()
        {
            _animator.SetTrigger(triggerReloading);
            PlayAudio(_reloadingClip);
        }

        private void PlayAudio(AudioClip clip)
        {
            _audio.clip = clip;
            _audio.Play();
        }
    }
}