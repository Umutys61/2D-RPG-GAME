using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class PlayerAttack : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Weapon initialWeapon;
    [SerializeField] private Weapon unarmedWeapon;
    [SerializeField] private Transform[] attackPositions;

    [Header("Melee Config")]
    [SerializeField] private ParticleSystem slashFX;
    [SerializeField] private float minDistanceMeleeAttack;

    public Weapon CurrentWeapon { get; private set; }

    private PlayerActions actions;
    private PlayerAnimations playerAnimations;
    private PlayerMovement playerMovement;
    private PlayerMana playerMana;
    private EnemyBrain enemyTarget;
    private Coroutine attackCoroutine;
    private Transform currentAttackPosition;
    private float currentAttackRotation;

    private void Awake()
    {
        actions = new PlayerActions();
        playerMana = GetComponent<PlayerMana>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimations = GetComponent<PlayerAnimations>();
    }

   private void Start()
{
    if (CurrentWeapon == null)
    {
        EquipWeapon(unarmedWeapon);
    }

    actions.Attack.ClickAttack.performed += ctx => Attack();
}

    private void Update()
    {
        GetFirePosition();
    }

    private void Attack()
    {
        if (enemyTarget == null || CurrentWeapon == null) return;

        float dist = Vector3.Distance(enemyTarget.transform.position, transform.position);
        if (dist > CurrentWeapon.Range)
        {
            Debug.Log($"❌ {CurrentWeapon.WeaponType} saldırısı iptal edildi! (Menzil: {CurrentWeapon.Range}, Mesafe: {dist:0.0})");
            return;
        }

        if (CurrentWeapon.RequiredMana > 0 && playerMana.CurrentMana < CurrentWeapon.RequiredMana)
        {
            Debug.Log("❌ Mana yetersiz! Saldırı iptal edildi.");
            return;
        }

        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        attackCoroutine = StartCoroutine(IEAttack());
    }

    private IEnumerator IEAttack()
    {
        if (currentAttackPosition == null) yield break;

        switch (CurrentWeapon.WeaponType)
        {
            case WeaponType.Magic:
                MagicAttack();
                break;

            case WeaponType.Bow:
                BowAttack();
                break;

            case WeaponType.Melee:
            default:
                MeleeAttack();
                break;
        }

        if (CurrentWeapon.RequiredMana > 0)
            playerMana.UseMana(CurrentWeapon.RequiredMana);

        playerAnimations.SetAttackAnimation(true);
        yield return new WaitForSeconds(0.5f);
        playerAnimations.SetAttackAnimation(false);
    }
private void MagicAttack()
{
    if (enemyTarget == null) return;

    Vector2 dir = (enemyTarget.transform.position - currentAttackPosition.position).normalized;
    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

    Quaternion rotation = Quaternion.Euler(0, 0, angle - 90f);

    Projectile projectile = Instantiate(CurrentWeapon.ProjectilePrefab, currentAttackPosition.position, rotation);
    projectile.Damage = GetAttackDamage();
    projectile.WeaponType = WeaponType.Magic;

    projectile.Init(CurrentWeapon.Range, enemyTarget.transform);
}

private void BowAttack()
{
    if (enemyTarget == null) return;

    Vector2 dir = (enemyTarget.transform.position - currentAttackPosition.position).normalized;
    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

    Quaternion rotation = Quaternion.Euler(0, 0, angle - 90f);

    Projectile projectile = Instantiate(CurrentWeapon.ProjectilePrefab, currentAttackPosition.position, rotation);
    projectile.Damage = GetAttackDamage();
    projectile.WeaponType = WeaponType.Bow;

    projectile.Init(CurrentWeapon.Range, enemyTarget.transform);
}

    private void MeleeAttack()
    {
        if (slashFX != null)
        {
            slashFX.transform.position = currentAttackPosition.position;
            slashFX.Play();
        }

        float currentDistanceToEnemy = Vector3.Distance(
            enemyTarget.GetComponent<EnemyHealth>().transform.position,
            transform.position
        );

        if (currentDistanceToEnemy <= minDistanceMeleeAttack)
        {
            enemyTarget.GetComponent<IDamageable>().TakeDamage(GetAttackDamage());
            stats.AddCombatExp(WeaponType.Melee, 1);
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        if (newWeapon == null)
        {
            CurrentWeapon = unarmedWeapon;
            Debug.Log("👊 Silah yok, tokat aktif!");
        }
        else
        {
            CurrentWeapon = newWeapon;
            Debug.Log($"⚔️ {newWeapon.Name} kuşanıldı!");
        }

        stats.TotalDamage = stats.BaseDamage + CurrentWeapon.Damage;
    }

    private float GetAttackDamage()
    {
        float damage = stats.BaseDamage + CurrentWeapon.Damage;

        if (CurrentWeapon.WeaponType == WeaponType.Melee)
            damage += stats.GetMeleeBonusDamage();
        else if (CurrentWeapon.WeaponType == WeaponType.Magic)
            damage += stats.GetMagicBonusDamage();
        else if (CurrentWeapon.WeaponType == WeaponType.Bow)
            damage += stats.GetBowBonusDamage();

        if (Random.Range(0f, 100f) <= stats.CriticalChance)
            damage += damage * (stats.CriticalDamage / 100f);

        return damage;
    }

    private void GetFirePosition()
    {
        Vector2 moveDirection = playerMovement.MoveDirection;
        switch (moveDirection.x)
        {
            case > 0f:
                currentAttackPosition = attackPositions[1];
                currentAttackRotation = -90f;
                break;
            case < 0f:
                currentAttackPosition = attackPositions[3];
                currentAttackRotation = -270f;
                break;
        }

        switch (moveDirection.y)
        {
            case > 0f:
                currentAttackPosition = attackPositions[0];
                currentAttackRotation = 0f;
                break;
            case < 0f:
                currentAttackPosition = attackPositions[2];
                currentAttackRotation = -180f;
                break;
        }
    }

    private void EnemySelectedCallback(EnemyBrain enemySelected) => enemyTarget = enemySelected;
    private void NoEnemySelectionCallback() => enemyTarget = null;

    private void OnEnable()
    {
        actions.Enable();
        SelectionManager.OnEnemySelectedEvent += EnemySelectedCallback;
        SelectionManager.OnNoSelectionEvent += NoEnemySelectionCallback;
        EnemyHealth.OnEnemyDeadEvent += NoEnemySelectionCallback;
    }

    private void OnDisable()
    {
        actions.Disable();
        SelectionManager.OnEnemySelectedEvent -= EnemySelectedCallback;
        SelectionManager.OnNoSelectionEvent -= NoEnemySelectionCallback;
        EnemyHealth.OnEnemyDeadEvent -= NoEnemySelectionCallback;
    }
}
