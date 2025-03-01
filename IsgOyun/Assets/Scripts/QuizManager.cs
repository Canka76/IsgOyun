using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public SoruListesi soruListesi;
    public TMP_Text soruText;
    public TMP_Text skorText;
    public Button[] secenekButonlari;

    private int mevcutSoruIndex = 0;
    private int skor = 0;

    void Start()
    {
        YeniSoruGetir();
    }

    void YeniSoruGetir()
    {
        if (mevcutSoruIndex >= soruListesi.sorular.Count)
        {
            Debug.Log("Oyun Bitti!");
            return;
        }

        Soru aktifSoru = soruListesi.sorular[mevcutSoruIndex];
        soruText.text = aktifSoru.soruMetni;

        for (int i = 0; i < secenekButonlari.Length; i++)
        {
            secenekButonlari[i].GetComponentInChildren<TMP_Text>().text = aktifSoru.secenekler[i];

            int index = i;  
            secenekButonlari[i].onClick.RemoveAllListeners();
            secenekButonlari[i].onClick.AddListener(() => CevapKontrol(index));
        }
    }

    void CevapKontrol(int secilenIndex)
    {
        if (secilenIndex == soruListesi.sorular[mevcutSoruIndex].dogruCevapIndex)
        {
            skor += 10;
        }

        skorText.text = "Skor: " + skor;
        mevcutSoruIndex++;
        YeniSoruGetir();
    }
}