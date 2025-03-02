using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public SoruListesi soruListesi;
    public TMP_Text soruText;
    public TMP_Text zamanlayiciText;
    public Image healthBarP1;
    public Image healthBarP2;
    
    public Animator player1Animator; // Player 1 Animator
    public Animator player2Animator; // Player 2 Animator

    private int mevcutSoruIndex = 0;
    private int healthP1 = 100;
    private int healthP2 = 100;
    private bool p1CevapVerdi = false;
    private bool p2CevapVerdi = false;
    private float kalanSure = 10f;
    private bool zamanBitti = false;

    void Start()
    {
        YeniSoruGetir();
    }

    void Update()
    {
        if (!zamanBitti)
        {
            kalanSure -= Time.deltaTime;
            zamanlayiciText.text = "Süre: " + Mathf.Ceil(kalanSure);

            if (kalanSure <= 0)
            {
                zamanBitti = true;
                SonrakiSoru();
            }
        }

        KlavyeGirisKontrol();
    }

    void KlavyeGirisKontrol()
    {
        if (!p1CevapVerdi)
        {
            if (Input.GetKeyDown(KeyCode.W)) CevapKontrol(0, 1);
            if (Input.GetKeyDown(KeyCode.A)) CevapKontrol(1, 1);
            if (Input.GetKeyDown(KeyCode.S)) CevapKontrol(2, 1);
            if (Input.GetKeyDown(KeyCode.D)) CevapKontrol(3, 1);
        }

        if (!p2CevapVerdi)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) CevapKontrol(0, 2);
            if (Input.GetKeyDown(KeyCode.Alpha2)) CevapKontrol(1, 2);
            if (Input.GetKeyDown(KeyCode.Alpha3)) CevapKontrol(2, 2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) CevapKontrol(3, 2);
        }

        if (p1CevapVerdi && p2CevapVerdi)
        {
            SonrakiSoru();
        }
    }

    void YeniSoruGetir()
    {
        if (healthP1 <= 0 || healthP2 <= 0)
        {
            Debug.Log("Oyun Bitti!");
            return;
        }

        if (mevcutSoruIndex >= soruListesi.sorular.Count)
        {
            Debug.Log("Tüm Sorular Bitti!");
            return;
        }

        Soru aktifSoru = soruListesi.sorular[mevcutSoruIndex];
        soruText.text = aktifSoru.soruMetni;
        p1CevapVerdi = false;
        p2CevapVerdi = false;
        zamanBitti = false;
        kalanSure = 10f;
    }

    void CevapKontrol(int secilenIndex, int oyuncu)
    {
        bool dogruMu = secilenIndex == soruListesi.sorular[mevcutSoruIndex].dogruCevapIndex;

        if (oyuncu == 1 && !p1CevapVerdi)
        {
            PlayAnimation(player1Animator); // Play animation for Player 1
            
            if (dogruMu) 
            {
                healthP2 -= 10;
                LeanTween.scaleX(healthBarP2.gameObject, healthP2 / 100f, 0.5f).setEase(LeanTweenType.easeOutBounce);
            }
            p1CevapVerdi = true;
        }
        else if (oyuncu == 2 && !p2CevapVerdi)
        {
            PlayAnimation(player2Animator); // Play animation for Player 2
            
            if (dogruMu) 
            {
                healthP1 -= 10;
                LeanTween.scaleX(healthBarP1.gameObject, healthP1 / 100f, 0.5f).setEase(LeanTweenType.easeOutBounce);
            }
            p2CevapVerdi = true;
        }

        if (healthP1 <= 0 || healthP2 <= 0)
        {
            Debug.Log("Oyun Bitti!");
        }
    }

    void PlayAnimation(Animator playerAnimator)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("GuessAnim");
            Invoke(nameof(ResetToIdle), 1.5f); // Return to idle after animation
        }
    }

    void ResetToIdle()
    {
        player1Animator.SetTrigger("Idle");
        player2Animator.SetTrigger("Idle");
    }

    void SonrakiSoru()
    {
        mevcutSoruIndex++;
        YeniSoruGetir();
    }
}
