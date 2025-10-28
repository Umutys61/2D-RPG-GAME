using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerMovement : MonoBehaviour
{


    [Header("Config")]
    [SerializeField] private float moveSpeed;
    private PlayerAnimations playerAnimations;
    public Vector2 MoveDirection => moveDirection;
    private Rigidbody2D rb2D;
    private PlayerActions actions;
    private Player player;
    private Vector2 moveDirection;
    private Vector2? targetPos;

    public bool HasManualInput => moveDirection != Vector2.zero;



    private void Awake()
    {
        player = GetComponent<Player>();
        actions = new PlayerActions();
        playerAnimations = GetComponent<PlayerAnimations>();
        rb2D = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        readMovement();
    }
    private void FixedUpdate()
    {
        // Eğer hedef pozisyon varsa → hedefe yürü
        if (targetPos.HasValue)
        {
            Vector2 dir = (targetPos.Value - rb2D.position);
            if (dir.magnitude < 0.1f)
            {
                Stop(); // hedefe ulaştı
                return;
            }

            rb2D.MovePosition(rb2D.position + dir.normalized * (moveSpeed * Time.fixedDeltaTime));
            playerAnimations.SetMoveBoolTransition(true);
            playerAnimations.SetMoveAnimation(dir.normalized);
        }
        else
        {
            // Eğer hedef yoksa → WASD inputuyla hareket et
            if (player.Stats.health <= 0f) return;

            rb2D.MovePosition(rb2D.position + moveDirection * (moveSpeed * Time.fixedDeltaTime));
            if (moveDirection == Vector2.zero)
            {
                playerAnimations.SetMoveBoolTransition(false);
            }
            else
            {
                playerAnimations.SetMoveBoolTransition(true);
                playerAnimations.SetMoveAnimation(moveDirection);
            }
        }
    }

  private void readMovement()
{
    moveDirection = actions.Movement.Move.ReadValue<Vector2>().normalized;

    // 🔽 Eğer oyuncu manuel input girerse → hedefi iptal et
    if (moveDirection != Vector2.zero && targetPos.HasValue)
    {
        Stop();
    }

    if (moveDirection == Vector2.zero)
    {
        playerAnimations.SetMoveBoolTransition(false);
        return; 
    }

    playerAnimations.SetMoveBoolTransition(true);
    playerAnimations.SetMoveAnimation(moveDirection);
}

    private void OnEnable()
    {
        actions.Enable();
    }
    private void OnDisable()
    {
        actions.Disable();
    }
    public void MoveTo(Vector2 pos)
{
    targetPos = pos;
}

public void Stop()
{
    targetPos = null;

    rb2D.linearVelocity = Vector2.zero;
    playerAnimations.SetMoveBoolTransition(false);
}

}
