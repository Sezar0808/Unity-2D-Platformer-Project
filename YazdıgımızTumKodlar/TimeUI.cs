using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private float timeCounter = 0f;
    private int totalSeconds = 0;
    private bool isRunning = false;

    void Start()
    {
        timerText.text = "00:00";
    }

    void Update()
    {
        // Herhangi bir tuþa basýnca baþla
        if (Input.anyKeyDown && !isRunning)
        {
            isRunning = true;
        }

        if (isRunning)
        {
            timeCounter += Time.deltaTime;

            if (timeCounter >= 1f)
            {
                totalSeconds++;
                timeCounter = 0f;
                UpdateTimerText();
            }
        }
    }

    void UpdateTimerText()
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public int GetTotalSeconds()
    {
        return totalSeconds;
    }
}
