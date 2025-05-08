using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Weapons.Base
{
    public class WeaponControlHandler : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                _weapon.Fire();
            
            if (Keyboard.current.rKey.wasPressedThisFrame)
                _weapon.Reload();
        }
    }
}