using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;
    [Header("Test")]
    public ItemHealthPotion healthPotion;
    public ItemManaPotion manaPotion;
    private PlayerAnimations playerAnimations;
    public PlayerMana playerMana { get; private set; }
    public PlayerHealth playerHealth { get; private set; }
    public PlayerAttack playerAttack { get; private set; }
    public PlayerStats Stats => stats;

    private void Awake()
    {
        playerAnimations = GetComponent<PlayerAnimations>();
        playerMana = GetComponent<PlayerMana>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAttack = GetComponent<PlayerAttack>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {

            if ((healthPotion.UseItem()))
            {
                Debug.Log("Using Health Potion");
            }
             if((manaPotion.UseItem()))
            {
                Debug.Log("Using Mana Potion");
            }
        }   
    }
    public void ResetPlayer()
    {
        stats.resetPlayer();
        playerAnimations.ResetPlayer();
        playerMana.ResetMana();
    }
}
