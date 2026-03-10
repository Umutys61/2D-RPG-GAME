using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 1.5f;

    private ResourceNode currentTarget;
    private PlayerMovement movement;
    private Animator animator;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>(); 
    }

    void Update()
    {
        if (currentTarget != null)
        {
            float dist = Vector2.Distance(transform.position, currentTarget.transform.position);

            if (movement.HasManualInput)
            {
                currentTarget.CancelGather();
                currentTarget = null;
                return;
            }

            if (dist <= interactRange)
            {
                movement.Stop();

                if (animator != null)
                {
                    switch (currentTarget.resourceType)
                    {
                        case ResourceType.Wood:
                            animator.SetTrigger("Chop");
                            break;
                        case ResourceType.Ore:
                            animator.SetTrigger("Mine");
                            break;
                        case ResourceType.Fish:
                            animator.SetTrigger("Fish");
                            break;
                    }
                }

                currentTarget.Gather();
            }
        }
    }

    public void SetTarget(ResourceNode node)
    {
        currentTarget = node;
        movement.MoveTo(node.transform.position);
    }

    public void ClearTarget()
    {
        currentTarget = null;
    }
}
