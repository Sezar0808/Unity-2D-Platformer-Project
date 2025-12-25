using UnityEngine;
using UnityEngine.UI;
public class SliderHelper : MonoBehaviour
{
    void OnEnable() // Slider her aktif olduðunda (Panel açýldýðýnda) çalýþýr
    {
        if (MenuAudio.instance != null)
        {
            MenuAudio.instance.SlideriBagla(GetComponent<Slider>());
        }
    }
}
