using UnityEngine;

public enum WeaponType
{
    Magic,
    Melee,
    Bow
}

[CreateAssetMenu(fileName = "Weapon_", menuName = "Items/Weapon")]
public class Weapon : InventoryItem
{
    [Header("Config")]
    public WeaponType WeaponType;
    public float Damage;

    [Header("Projectile")]
    public Projectile ProjectilePrefab;
    public float RequiredMana;

    [Header("Range Settings")]
    [Tooltip("Silahın maksimum menzili")]
    public float Range;

    [Header("Stat Bonuses")]
    public float attackBonus;   // Silahın verdiği ekstra saldırı
    public float healthBonus;   // Bazı silahlar HP verebilir
    public float manaBonus;     // Bazı silahlar mana verebilir

    // ✅ Kuşanma override
    public override void EquipItem()
    {
        var playerEquip = GameManager.Instance.Player.GetComponent<PlayerEquipment>();
        if (playerEquip != null)
        {
            playerEquip.EquipMainWeapon(this);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // 🎯 WeaponType seçildiğinde varsayılan Range ayarla
        switch (WeaponType)
        {
            case WeaponType.Melee:
                if (Range <= 0f || Range == 5f) Range = 1.5f;
                break;

            case WeaponType.Bow:
                if (Range <= 0f || Range == 5f) Range = 7f;
                break;

            case WeaponType.Magic:
                if (Range <= 0f || Range == 5f) Range = 5f;
                break;
        }
    }
#endif
}
