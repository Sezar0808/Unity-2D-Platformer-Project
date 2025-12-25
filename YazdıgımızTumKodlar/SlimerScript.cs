using UnityEngine;
using System.Collections;
public class SlimerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float jumpForceY = 5f;
    public float jumpForceX = 2f;
                                                                                                                //GROUND TAG I LAZIM UNUTMA
                                                                                                              
    public int jumps = 3;
    private int jumpCount = 0;

    private bool yerdeMi = false;   // Yön deðiþtirme zamaný geldi mi?
    private bool isGrounded = true; // Slime þu anda yere basýyor mu?

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        StartCoroutine(PrintEveryTwoSeconds());
    }

    // Sürekli çalýþan ve 2 saniyede bir Jump() çaðýran coroutine
    IEnumerator PrintEveryTwoSeconds()
    {
        while (true) // Sonsuz döngü
        {
            yield return new WaitForSeconds(2f); // 1 saniye bekle
            Jump(); // Slime’ý zýplat
        }
    }

    // Slime'ýn zýplamasýný saðlayan fonksiyon
    void Jump()
    {
        // Eðer slime havadaysa tekrar zýplamasýn
        if (!isGrounded) return;

        isGrounded = false;
        anim.SetBool("isJumping", true);

        float direction = -Mathf.Sign(transform.localScale.x);

        rb.linearVelocity = Vector2.zero;

        rb.AddForce(
            new Vector2(direction * jumpForceX, jumpForceY),
            ForceMode2D.Impulse
        );

        jumpCount++;


        if (jumpCount >= jumps)
        {
            yerdeMi = true;
            jumpCount = 0;
        }
    }

    // Slime'ýn yönünü tersine çevirir
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Slime bir collider'a çarptýðýnda çalýþýr
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Eðer çarpýlan obje Ground tag’ine sahipse
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("isJumping", false);

            // Eðer yön deðiþtirme zamaný geldiyse
            if (yerdeMi)
            {
                Flip();        // Yön deðiþtir
                yerdeMi = false;
            }
        }
    }
}
