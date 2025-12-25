using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiManager : MonoBehaviour
{
    public Image[] hearts;

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                // Can varsa kalp aktif olsun ve þeffaflýðý tam olsun
                hearts[i].enabled = true;
                hearts[i].color = new Color(hearts[i].color.r, hearts[i].color.g, hearts[i].color.b, 1f);
            }
            else
            {
                // Eðer kalp þu an görünürse (enabled) ve can gitmiþse soldurma iþlemini baþlat
                if (hearts[i].enabled)
                {
                    StartCoroutine(FadeOutHeart(hearts[i], 0.5f));
                }
            }
        }
    }
    IEnumerator FadeOutHeart(Image heartImage, float duration)          //kalplerin yavaþça yok olmasý
    {
        float currentTime = 0;
        Color originalColor = heartImage.color;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            // Alpha deðerini 1'den 0'a doðru yumuþatýr (Lerp)
            float newAlpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            heartImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // Animasyon bittiðinde kalbi tamamen devre dýþý býrak
        heartImage.enabled = false;
    }



}
