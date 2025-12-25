using UnityEngine;
using System.Collections;


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public UiManager uiManager;         //ui manager için. kalp,coin




    public Animator animator;
    public Rigidbody2D rb;
    public Transform respawnPoint;      //geri dönmesi için.


    public float invincibilityDuration = 2f; // Ne kadar süre dokunulmaz olacak?
    private bool isInvincible = false;

    private SpriteRenderer spriteRenderer; //sprite ý yakýp açmak için.

    private Player movementScript;  // PlayerMovement scriptini durdurmak için

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movementScript = GetComponent<Player>();

        // BUG ÇÖZÜMÜ: Sahne her baþladýðýnda çarpýþmalarý zorla aktif et!
        Physics2D.IgnoreLayerCollision(7, 8, false);
        isInvincible = false;
        spriteRenderer.enabled = true;
    }

   
   public void TakeDamage(int damage)         //hasar alamsý için fonksiyon
    {
        if (isInvincible) return;

        currentHealth -= damage;        //can azaltmasý için

        if (uiManager != null) uiManager.UpdateHearts(currentHealth); //ui da güncellemesi için hasar alýnca

              //yaralanma animasyonu için.

        if (currentHealth <= 0)
        {
            StartCoroutine(DeathRoutine());
        }
        else
        {
            animator.SetTrigger("Hurt");
            
            StartCoroutine(BecomeInvincible());
        }

    }

    public IEnumerator DeathRoutine()
    {
        // 1. Karakteri dondur ve animasyonu baþlat
        movementScript.enabled = false;
        rb.linearVelocity = Vector2.zero;

        animator.SetTrigger("Hurt"); // Yaralanma animasyonunu baþlat

        isInvincible = true; // Ölürken hasar almasýn

        // 2. Bekle (Animasyon süresine göre burayý 0.5f veya 1f yapabilirsin)
        yield return new WaitForSeconds(0.30f);

        animator.SetTrigger("Death?");


        yield return new WaitForSeconds(0.35f);

        // 3. Iþýnla
        Vector3 spawnPosition = new Vector3(respawnPoint.position.x, respawnPoint.position.y + 1f, respawnPoint.position.z);
        transform.position = spawnPosition;

        // 4. Resetle
        isInvincible = false;
        spriteRenderer.enabled = true;
        currentHealth = maxHealth;
        if (uiManager != null) uiManager.UpdateHearts(currentHealth); //kalpleri tekrar 3 yapmak için.
        rb.linearVelocity = Vector2.zero;
        movementScript.enabled = true;  
    }
    IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        


        // Player (6. katman) ve Enemy (7. katman) arasýndaki çarpýþmayý kapat
        // (Katman numaralarýn farklýysa o numaralarý yazmalýsýn)
        Physics2D.IgnoreLayerCollision(7, 8, true);



        // Görsel efekt: Karakter yanýp sönsün (Mario'daki gibi)
        float timer = 0;
        while (timer < invincibilityDuration)
        {
            // Sprite'ý kapat/aç (Yanýp sönme efekti)
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }
        // Süre bittiðinde çarpýþmayý tekrar aç
        Physics2D.IgnoreLayerCollision(7, 8, false);

        spriteRenderer.enabled = true; // Sonunda görünür olduðundan emin ol
        isInvincible = false;
        
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))       //enemy isimli biriyle tetiklesin.
        {
            TakeDamage(1);
        }
    }

    
    
}
