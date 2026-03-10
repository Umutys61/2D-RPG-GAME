using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float rotateSpeed = 200f;
    [SerializeField] private GameObject hitFXPrefab;

    public float Damage { get; set; }
    public WeaponType WeaponType { get; set; }

    private Transform target;
    private float maxRange;
    private Vector3 spawnPos;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        spawnPos = transform.position;
    }

    public void Init(float range, Transform targetTransform)
    {
        maxRange = range;
        target = targetTransform;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        var health = target.GetComponent<EnemyHealth>();
        if (health == null || health.isDead)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = ((Vector2)target.position - rb.position).normalized;

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;

        rb.linearVelocity = transform.up * speed;

        if (Vector3.Distance(spawnPos, transform.position) >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform != target) return;

        other.GetComponent<IDamageable>()?.TakeDamage(Damage);

        GameManager.Instance.Player.Stats.AddCombatExp(WeaponType, 1);

        if (hitFXPrefab != null)
            Instantiate(hitFXPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
