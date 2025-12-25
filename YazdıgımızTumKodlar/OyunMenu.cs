using UnityEngine;
using UnityEngine.SceneManagement;

public class OyunMenu : MonoBehaviour
{
    public GameObject ekran;
    public GameObject otherýmage;
    public GameObject oyunmenu;

    public void ContuineButton()
    {
        oyunmenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void AnaMenuDon()
    {
        Time.timeScale = 1.0f; // Menüye gitmeden zamaný akýt
        SceneManager.LoadScene("MainMenu");
    }


    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Eðer menü þu an aktifse kapat, kapalýysa aç
            if (oyunmenu.activeSelf)
            {
                ContuineButton(); // Zaten yazdýðýn kapatma fonksiyonunu çaðýrýyoruz
            }
            else
            {
                oyunmenu.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
}