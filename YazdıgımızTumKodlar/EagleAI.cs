using UnityEngine;

public class EagleAI : MonoBehaviour
{
    public EagleAlan eagleAlan;

    [Header("Uçuş Alanı")]
    public Transform flyPointA;
    public Transform flyPointB;
    public float flySpeed = 2f;

    [Header("Dalma")]
    public float diveSpeed = 6f;
    public float hitDuration = 1.5f;

    [Header("Geri Dönüş")]
    public float returnSpeed = 4f;

    [Header("Dalış Bekleme Süresi")]
    public float diveCooldown = 4f;

    private float diveTimer = 0f;

    private Vector2 currentFlyTarget;
    private Vector2 diveTarget;
    private Vector2 returnTarget;

    private Animator anim;

    private float baseScaleX;


    private enum State
    {
        Fly,
        Dive,
        Hit,
        Return
    }

    private State state;

    void Start()
    {
        anim = GetComponent<Animator>();

        currentFlyTarget = flyPointA.position;
        returnTarget = transform.position;
        baseScaleX = Mathf.Abs(transform.localScale.x);
        ChangeState(State.Fly);
    }

    void Update()
    {
        if (diveTimer > 0)
            diveTimer -= Time.deltaTime;

        StateMachine();
    }

    void StateMachine()
    {
        switch (state)
        {
            case State.Fly:
                FlyBetweenPoints();

                if (eagleAlan.playerInside && diveTimer <= 0f)
                {
                    diveTarget = eagleAlan.lastPlayerPos;
                    ChangeState(State.Dive);
                }
                break;

            case State.Dive:
                DiveToTarget();
                break;

            case State.Hit:
                // bekleme (Invoke ile çıkılıyor)
                break;

            case State.Return:
                ReturnToSky();
                break;
        }
    }

    // ---------------- FLY ----------------
    void FlyBetweenPoints()
    {
        LookDirection(currentFlyTarget);

        transform.position = Vector2.MoveTowards(
            transform.position,
            currentFlyTarget,
            flySpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, currentFlyTarget) < 0.1f)
        {
            currentFlyTarget =
                currentFlyTarget == (Vector2)flyPointA.position
                ? flyPointB.position
                : flyPointA.position;
        }
    }

    // ---------------- DIVE ----------------
    void DiveToTarget()
    {
        LookDirection(diveTarget);

        transform.position = Vector2.MoveTowards(
            transform.position,
            diveTarget,
            diveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, diveTarget) < 0.1f)
        {
            ChangeState(State.Hit);
            Invoke(nameof(StartReturn), hitDuration);
        }
    }

    // ---------------- RETURN ----------------
    void StartReturn()
    {
        returnTarget = new Vector2(transform.position.x, flyPointA.position.y);
        ChangeState(State.Return);
    }

    void ReturnToSky()
    {
        LookDirection(returnTarget);

        transform.position = Vector2.MoveTowards(
            transform.position,
            returnTarget,
            returnSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, returnTarget) < 0.1f)
        {
            diveTimer = diveCooldown;
            ChangeState(State.Fly);
        }
    }

    // ---------------- STATE CHANGE ----------------
    void ChangeState(State newState)
    {
        if (state == newState) return;

        state = newState;

        anim.ResetTrigger("Fly");
        anim.ResetTrigger("Dive");
        anim.ResetTrigger("Hit");

        // 🔑 Return = Fly animasyonu
        if (state == State.Fly || state == State.Return)
            anim.SetTrigger("Fly");

        if (state == State.Dive)
            anim.SetTrigger("Dive");

        if (state == State.Hit)
            anim.SetTrigger("Hit");
    }

    void LookDirection(Vector2 target)
    {
        float dir = target.x - transform.position.x;

        if (Mathf.Abs(dir) < 0.01f) return;

        if (dir > 0)
            transform.localScale = new Vector3(baseScaleX, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-baseScaleX, transform.localScale.y, transform.localScale.z);
    }

}
