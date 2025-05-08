using System.Text;
using Code.Weapons.Base;
using UnityEngine;

namespace Code.UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private UIStatusBar _weaponClipSizeBar;
        [SerializeField] private Weapon _weapon;

        private StringBuilder _stringBuilder;

        private void Awake()
        {
            _stringBuilder = new StringBuilder("0/0");
            _weapon.OnFire += UpdateAmmoClipSize;
            _weapon.OnReloaded += UpdateAmmoClipSize;
        }

        private void Start() => 
            UpdateAmmoClipSize();

        private void OnDestroy()
        {
            _weapon.OnFire -= UpdateAmmoClipSize;
            _weapon.OnReloaded -= UpdateAmmoClipSize;
        }

        private void UpdateAmmoClipSize()
        {
            _stringBuilder.Clear();
            _stringBuilder
                .Append(_weapon.CurrentAmmoClip)
                .Append("/")
                .Append(_weapon.ClipSize);
            
            _weaponClipSizeBar.UpdateTextInfo(_stringBuilder.ToString());
            _weaponClipSizeBar.ReportProgress((float)_weapon.CurrentAmmoClip / (float)_weapon.ClipSize);
        }
    }
}