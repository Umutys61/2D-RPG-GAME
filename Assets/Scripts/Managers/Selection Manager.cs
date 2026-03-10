    using System;
    using UnityEngine;
    using Vector2 = UnityEngine.Vector2;

    public class SelectionManager : MonoBehaviour
    {
        public static event Action<EnemyBrain> OnEnemySelectedEvent;
        public static event Action OnNoSelectionEvent;

        [Header("Config")]
        [SerializeField] private LayerMask enemyMask;
        

        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            SelectEnemy();
        }

       private void SelectEnemy()
{
    if (Input.GetMouseButtonDown(0))
    {
        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, enemyMask);

        if (hit.collider != null)
        {
            EnemyBrain enemy = hit.collider.GetComponent<EnemyBrain>();
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            EnemyLoot enemyLoot = hit.collider.GetComponent<EnemyLoot>();

            if (enemy != null && enemyHealth != null)
            {
                if (enemyHealth.CurrentHealth > 0f)
                {
                    OnEnemySelectedEvent?.Invoke(enemy);
                }
                else
                {
    if (enemyLoot != null && enemyLoot.Items != null && enemyLoot.Items.Count > 0)
    {
        float dist = Vector2.Distance(GameManager.Instance.Player.transform.position, hit.collider.transform.position);

        if (dist <= 3f)
        {
            LootManager.Instance.ShowLoot(enemyLoot);
            Debug.Log(" Loot panel açildi (yakin mesafeden cesede tikladin)");
        }
        else
        {
            Debug.Log(" Çok uzaksin, loot açilamadi!");
        }
    }

                }
            }
        }
        else
        {
            OnNoSelectionEvent?.Invoke();
        }
    }
}

    }
