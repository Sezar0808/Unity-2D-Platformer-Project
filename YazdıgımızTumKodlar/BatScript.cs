using UnityEngine;

public class BatScript : MonoBehaviour
{
    public YarasaAlan YA;

    [Header("Pozisyonlar")]
    public float suzulX;
    public float suzulY;

    public float flyX;
    public float flyY;

    [Header("Hızlar")]
    public float suzulSpeed = 2f;
    public float flySpeed = 3f;

    private Vector2 startPos;
    private Vector2 suzulPos;
    private Vector2 flyPos;
    private Vector2 currentTarget;

    private Animator animator;

    enum BatState
    {
        Returning,
        GoingToSuzul,
        FlyingBetweenPoints
    }

    private BatState state = BatState.Returning;

    void Start()
    {
        startPos = transform.position;
        suzulPos = new Vector2(suzulX, suzulY);
        flyPos = new Vector2(flyX, flyY);

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        StateMachine();
        UpdateAnimation();
    }

    void StateMachine()
    {
        if (!YA.playerInside)
        {
            state = BatState.Returning;
        }

        switch (state)
        {
            case BatState.Returning:
                MoveTo(startPos, suzulSpeed);

                if (YA.playerInside)
                    state = BatState.GoingToSuzul;
                break;

            case BatState.GoingToSuzul:
                MoveTo(suzulPos, suzulSpeed);

                if (Vector2.Distance(transform.position, suzulPos) < 0.05f)
                {
                    currentTarget = flyPos;
                    state = BatState.FlyingBetweenPoints;
                }
                break;

            case BatState.FlyingBetweenPoints:
                MoveTo(currentTarget, flySpeed);

                if (Vector2.Distance(transform.position, currentTarget) < 0.05f)
                {
                    currentTarget = (currentTarget == flyPos) ? suzulPos : flyPos;
                }
                break;
        }
    }

    void MoveTo(Vector2 target, float speed)
    {
        FlipToTarget(target); // 🔥 yönü hedefe göre ayarla

        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );
    }


    void UpdateAnimation()
    {
        bool isFlying = state != BatState.Returning ||
                        Vector2.Distance(transform.position, startPos) > 0.05f;

        animator.SetBool("isFlying", isFlying);
    }
    void FlipToTarget(Vector2 target)
    {
        if (target.x > transform.position.x)
            transform.localScale = new Vector3(3, 3, 1);   // sağa bak
        else if (target.x < transform.position.x)
            transform.localScale = new Vector3(-3, 3, 1);  // sola bak
    }
}