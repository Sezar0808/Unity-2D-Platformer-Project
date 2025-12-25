using UnityEngine;

public class AIscript : MonoBehaviour
{
    public float speed = 2f;

    public float leftX;
    public float rightX;

    private Rigidbody2D rb;

    // Ayý þu anda saða mý gidiyor?
    private bool movingRight = true;

    void Start()
    {
        // Rigidbody componentini çaðýrýyoruz
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //saða sola yürüme kodu
        rb.linearVelocity = new Vector2(
            (movingRight ? speed : -speed),
            rb.linearVelocity.y
        );

        // Eðer ayý saða gidiyorsa ve sað sýnýra ulaþtýysa
        // yönünü sola çevir
        if (movingRight && transform.position.x >= rightX)
        {
            Flip(false);
        }
        // Eðer ayý sola gidiyorsa ve sol sýnýra ulaþtýysa
        // yönünü saða çevir
        else if (!movingRight && transform.position.x <= leftX)
        {
            Flip(true);
        }
    }

    // Ayýnýn yönünü deðiþtiren fonksiyon
    void Flip(bool goRight)
    {
        movingRight = goRight;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (goRight ? 1 : -1);
        transform.localScale = scale;
    }
}
