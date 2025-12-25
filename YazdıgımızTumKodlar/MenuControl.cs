using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{


    public GameObject optionsPanel;

    public void OpenOptions()
    {
        optionsPanel.SetActive(true); // Ayarlar panelini aç
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false); // Ayarlar panelini kapat
    }

    public void StartButton()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1.0f;
    }


    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
