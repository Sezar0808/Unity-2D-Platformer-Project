using UnityEngine;

public class Background : MonoBehaviour
{
    private float startPos;      // Objenin ilk yerini tutar
    private float camStartPos;   // Kameranýn ilk yerini tutar

    public Camera cam;           // Main Camera
    public float parallaxFactor; // 0 ile 1 arasý (Örn: 0.2)

    void Start()
    {
        // 1. Objenin sahnede senin koyduðun yerini kaydet
        startPos = transform.position.x;

        // 2. Kameranýn oyun baþladýðý andaki yerini kaydet
        camStartPos = cam.transform.position.x;
    }

    void Update()
    {
        // Kameranýn baþlangýç noktasýndan ne kadar uzaklaþtýðýný hesapla
        float relativeCamDist = cam.transform.position.x - camStartPos;

        // Bu mesafeyi parallax katsayýsýyla çarp
        float moveDist = relativeCamDist * parallaxFactor;

        // Objeyi baþlangýç konumuna bu farký ekleyerek taþý
        transform.position = new Vector3(startPos + moveDist, transform.position.y, transform.position.z);
    }
}
