using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Karakter düþtü mü kontrol et
        if (other.CompareTag("Player"))
        {
            // Karakterdeki PlayerHealth scriptine ulaþ
            PlayerHealth health = other.GetComponent<PlayerHealth>();

            if (health != null)
            {
                // Mevcut caný ne olursa olsun direkt ölüm rutinini baþlatmak için:
                // Ýstersen direkt canýný 0 yapýp TakeDamage(1) diyebilirsin 
                // ya da direkt Coroutine'i baþlatabilirsin:
                health.StartCoroutine(health.DeathRoutine());
            }
        }
    }
}
