    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class WeaponManager : Singleton<WeaponManager>
    {
        [Header("Config")]
        [SerializeField] private Image weaponIcon;
        [SerializeField] private TextMeshProUGUI weaponManaTMP;

      public void EquipWeapon(Weapon weapon)
{
    if (weapon == null)
    {
        weaponIcon.sprite = null;
        weaponIcon.gameObject.SetActive(false);
        weaponManaTMP.gameObject.SetActive(false);
        return;
    }

    weaponIcon.sprite = weapon.Icon;
    weaponIcon.gameObject.SetActive(true);

    if (weapon.RequiredMana > 0)
    {
        weaponManaTMP.text = weapon.RequiredMana.ToString();
        weaponManaTMP.gameObject.SetActive(true);
    }
    else
    {
        weaponManaTMP.gameObject.SetActive(false);
    }

    GameManager.Instance.Player.playerAttack.EquipWeapon(weapon);
}

    }
