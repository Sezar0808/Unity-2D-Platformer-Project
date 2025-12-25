using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [Header("UI Panelleri")]
    public GameObject finishPanel;
    public TextMeshProUGUI finalCoinText;
    public TextMeshProUGUI finalTimeText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Sahnedeki TimeUI scriptine ve Player scriptine ulaþalým
            TimeUI timerScript = FindObjectOfType<TimeUI>();
            Player playerScript = other.GetComponent<Player>();

            if (playerScript != null && timerScript != null)
            {
                ShowFinishScreen(playerScript, timerScript);
            }
        }
    }

    void ShowFinishScreen(Player player, TimeUI timer)
    {
        // 1. Oyunu durdur
        Time.timeScale = 0f;
        finishPanel.SetActive(true);

        // 2. Altýn sayýsýný al
        int totalCoins = player.CoinManager.coinCount;
        finalCoinText.text = "Toplanan Elmas: " + totalCoins;

        // 3. Süreyi TimeUI'dan çek (Zaten TimeUI içinde formatlanmýþ hali olabilir ama garantileyelim)
        // TimeUI scriptindeki totalSeconds deðiþkenini kullanýyoruz
        int minutes = timer.GetTotalSeconds() / 60;
        int seconds = timer.GetTotalSeconds() % 60;

        finalTimeText.text = "Geçirilen Süre: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}