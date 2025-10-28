using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float rotateSpeed = 200f;  // dönüş hızı
    [SerializeField] private GameObject hitFXPrefab;

    public float Damage { get; set; }
    public WeaponType WeaponType { get; set; }

    private Transform target;      // sadece atanmış hedef
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

    /// <summary>
    /// Mermiyi başlatır, hedef atanır.
    /// </summary>
    public void Init(float range, Transform targetTransform)
    {
        maxRange = range;
        target = targetTransform;
    }

    private void FixedUpdate()
    {
        // hedef yoksa → mermiyi yok et (başka düşmana gitmesin)
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // hedef öldüyse → mermiyi yok et
        var health = target.GetComponent<EnemyHealth>();
        if (health == null || health.isDead)
        {
            Destroy(gameObject);
            return;
        }

        // hedef yönü
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;

        // açıyı hesapla
        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        // Rigidbody döndür
        rb.angularVelocity = -rotateAmount * rotateSpeed;

        // ileriye doğru sabit hızda ilerle
        rb.linearVelocity = transform.up * speed;

        // menzil kontrolü
        if (Vector3.Distance(spawnPos, transform.position) >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // sadece kendi hedefine çarptığında çalış
        if (other.transform != target) return;

        // hasar uygula
        other.GetComponent<IDamageable>()?.TakeDamage(Damage);

        // EXP ekle
        GameManager.Instance.Player.Stats.AddCombatExp(WeaponType, 1);

        // efekt
        if (hitFXPrefab != null)
            Instantiate(hitFXPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
