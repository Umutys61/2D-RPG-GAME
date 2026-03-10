using UnityEngine;

public class MouseClick : MonoBehaviour
{
    public Camera cam;
    public PlayerInteraction playerInteraction;

    [Header("Click Settings")]
    public float clickRange = 8f;
    public float lootRange = 3f;

    [Header("Layer Settings")]
    public LayerMask enemyLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            Collider2D nodeCollider = Physics2D.OverlapCircle(mousePos, 0.1f, LayerMask.GetMask("ResourceNode"));
            if (nodeCollider != null)
            {
                var node = nodeCollider.GetComponent<ResourceNode>();
                if (node != null)
                {
                    float dist = Vector2.Distance(playerInteraction.transform.position, node.transform.position);
                    if (dist <= clickRange)
                    {
                        playerInteraction.SetTarget(node);
                        Debug.Log(" Node seçildi.");
                    }
                    else
                    {
                        Debug.Log(" Çok uzak, node seçilemedi!");
                    }
                }
            }

            Collider2D[] enemies = Physics2D.OverlapCircleAll(mousePos, 0.1f, enemyLayer);
            foreach (var col in enemies)
            {
                EnemyHealth enemy = col.GetComponent<EnemyHealth>();
                if (enemy != null && enemy.isDead)
                {
                    float dist = Vector2.Distance(playerInteraction.transform.position, enemy.transform.position);
                    Debug.Log($"[DEBUG] PlayerPos: {playerInteraction.transform.position} | EnemyPos: {enemy.transform.position} | Loot mesafe: {dist}");

                    if (dist <= lootRange)
                    {
                        if (enemy.TryGetComponent<EnemyLoot>(out var loot) && loot.Items != null && loot.Items.Count > 0)
                        {
                            LootManager.Instance.ShowLoot(loot);
                            Debug.Log(" Loot açıldı!");
                        }
                        else
                        {
                            Debug.Log(" Loot yok!");
                        }
                    }
                    else
                    {
                        Debug.Log(" Çok uzaktasın, loot için yaklaş!");
                    }
                }
            }
        }
    }
}
