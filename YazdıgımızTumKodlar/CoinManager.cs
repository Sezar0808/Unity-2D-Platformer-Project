using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public Text coinText; // Inspector'dan CoinText'i buraya sürükle
    public int coinCount = 0;

    public void AddCoin()
    {
        coinCount++;
        coinText.text = coinCount.ToString();
    }
}
