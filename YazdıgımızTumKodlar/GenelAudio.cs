using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuAudio : MonoBehaviour
{
    public AudioMixer mikser;
    public AudioClip menuMuzigi;  // Menüde çalacak müzik
    public AudioClip oyunMuzigi;  // Oyun sahnesinde çalacak müzik

    private AudioSource sesKaynagi;
    private Slider sesSlider;
    public static MenuAudio instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        SetupAudioSource();
    }

    void SetupAudioSource()
    {
        sesKaynagi = GetComponent<AudioSource>();
        if (sesKaynagi == null) sesKaynagi = gameObject.AddComponent<AudioSource>();
        sesKaynagi.loop = true;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Sahne adýna göre müziði deðiþtir
        if (scene.name == "MainMenu") // Menü sahnesinin adý
        {
            MuzikCal(menuMuzigi);
            Invoke("FindAndSetupSlider", 0.2f);
        }
        else if (scene.name == "SampleScene") // Oyun sahnesinin adý
        {
            MuzikCal(oyunMuzigi);
        }
    }

    void MuzikCal(AudioClip klip)
    {
        if (klip == null) return;

        // Eðer zaten ayný müzik çalýyorsa baþtan baþlatma
        if (sesKaynagi.clip == klip && sesKaynagi.isPlaying) return;

        sesKaynagi.Stop();
        sesKaynagi.clip = klip;
        sesKaynagi.Play();
    }

    void FindAndSetupSlider()
    {
        sesSlider = Object.FindFirstObjectByType<Slider>();
        if (sesSlider != null)
        {
            float kaydedilenSes = PlayerPrefs.GetFloat("SesSeviyesi", 1f);
            sesSlider.value = kaydedilenSes;
            sesSlider.onValueChanged.RemoveAllListeners();
            sesSlider.onValueChanged.AddListener(SesAyarla);
            SesAyarla(kaydedilenSes);
        }
    }

    public void SlideriBagla(Slider gelenSlider)
    {
        float kaydedilenSes = PlayerPrefs.GetFloat("SesSeviyesi", 1f);
        gelenSlider.value = kaydedilenSes;
        gelenSlider.onValueChanged.RemoveAllListeners();
        gelenSlider.onValueChanged.AddListener(SesAyarla);
        SesAyarla(kaydedilenSes);
    }

    public void SesAyarla(float deger)
    {
        if (mikser != null)
        {
            float dB = Mathf.Log10(Mathf.Max(0.0001f, deger)) * 20;
            mikser.SetFloat("MusicVol", dB);
            PlayerPrefs.SetFloat("SesSeviyesi", deger);
            PlayerPrefs.Save();
        }
    }
}