using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class QuizManager : MonoBehaviour
{
    public SoruListesi soruListesi;
    public TMP_Text soruText;
    public TMP_Text zamanlayiciText;
    public Image healthBarP1;
    public Image healthBarP2;
    public GameObject timeBar;
    public Button[] secenekButonlari;    
    public Animator player1Animator; // Player 1 Animator
    public Animator player2Animator; // Player 2 Animator

    [SerializeField] private float damageAmount = 10f;
    [SerializeField,Range(0,1)] private float P1AnimSoundDelay = .55f;
    [SerializeField,Range(0,1)] private float P2AnimSoundDelay = .65f;


    private int mevcutSoruIndex = 0;
    private int healthP1 = 100;
    private int healthP2 = 100;
    private bool p1CevapVerdi = false;
    private bool p2CevapVerdi = false;
    private float kalanSure = 10f;
    private bool zamanBitti = false;
    

    void Start()
    {
        SoundManager.Instance.RandomMusic();
        YeniSoruGetir();
    }

    void Update()
    {
        if (!zamanBitti)
        {
            kalanSure -= Time.deltaTime;
            zamanlayiciText.text = "Süre: " + Mathf.Ceil(kalanSure);

            // Update the time bar scale dynamically
            timeBar.transform.localScale = new Vector3(kalanSure / 10f, 1, 1);

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
        for (int i = 0; i < secenekButonlari.Length; i++) 
        {
            secenekButonlari[i].GetComponentInChildren<TMP_Text>().text = aktifSoru.secenekler[i];
                
        }
        
        p1CevapVerdi = false;
        p2CevapVerdi = false;
        zamanBitti = false;
        kalanSure = 10f;
    }

    void CevapKontrol(int secilenIndex, int oyuncu)
{
    bool dogruMu = secilenIndex == soruListesi.sorular[mevcutSoruIndex].dogruCevapIndex;

    if (dogruMu)
    {
        float sectionLength = 10f / 3f; // Divide total time by 3
        float elapsedTime = 10f - kalanSure; // Time already passed

        float damageMultiplier = 1f;

        if (elapsedTime < sectionLength)
        {
            damageMultiplier = 2f; // First third: double damage
        }
        else if (elapsedTime < 2 * sectionLength)
        {
            damageMultiplier = 1f; // Second third: normal damage
        }
        else
        {
            damageMultiplier = 0.5f; // Last third: half damage
        }

        if (oyuncu == 1 && !p1CevapVerdi)
        {
            PlayAnimation(player1Animator);

            int damage = Mathf.RoundToInt(damageAmount * damageMultiplier);
            healthP2 -= damage;
            LeanTween.scaleX(healthBarP2.gameObject, healthP2 / 100f, 0.5f).setEase(LeanTweenType.easeOutElastic);
            StartCoroutine(PlaySfxAfterDelay(P1AnimSoundDelay));

            if (!p2CevapVerdi)
            {
                MoveTimeBarToNextSection(); // Move the timer forward
            }

            p1CevapVerdi = true;
        }

        if (oyuncu == 2 && !p2CevapVerdi)
        {
            PlayAnimation(player2Animator);

            int damage = Mathf.RoundToInt(damageAmount * damageMultiplier);
            healthP1 -= damage;
            LeanTween.scaleX(healthBarP1.gameObject, healthP1 / 100f, 0.5f).setEase(LeanTweenType.easeOutBounce);
            StartCoroutine(PlaySfxAfterDelay(P2AnimSoundDelay));


            if (!p1CevapVerdi)
            {
                MoveTimeBarToNextSection(); // Move the timer forward
            }

            p2CevapVerdi = true;
        }

        if (healthP1 <= 0 || healthP2 <= 0)
        {
            Debug.Log("Oyun Bitti!");
        }
    }
    else
    {
        // Handle incorrect answers if needed
        if (oyuncu == 1 && !p1CevapVerdi)
        {
            p1CevapVerdi = true;
        }

        if (oyuncu == 2 && !p2CevapVerdi)
        {
            p2CevapVerdi = true;
        }
    }
}
    private IEnumerator PlaySfxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.Instance.RandomSfx(); // Change index as needed
    }


    void PlayAnimation(Animator playerAnimator)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Anim", 1);
            Invoke(nameof(ResetToIdle), 1.5f);
            Debug.Log("Play animation");
        }
    }

    void ResetToIdle()
    {
        Debug.Log("reset to idle");
        if (player1Animator != null)
            player1Animator.SetFloat("Anim", 0);
    
        if (player2Animator != null)
            player2Animator.SetFloat("Anim", 0);
    }
    
    void MoveTimeBarToNextSection()
    {
        float sectionLength = 10f / 3f; // Divide total time by 3
        float elapsedTime = 10f - kalanSure; // Time already passed

        // Move to the next section
        if (elapsedTime < sectionLength)
            kalanSure = 10f - sectionLength;
        else if (elapsedTime < 2 * sectionLength)
            kalanSure = 10f - 2 * sectionLength;
        else
            kalanSure = 0; // Move to the end

        // Smoothly adjust the time bar
        float newScaleX = kalanSure / 10f;
        LeanTween.cancel(timeBar);
        LeanTween.scaleX(timeBar, newScaleX, 0.3f).setEase(LeanTweenType.easeOutQuad);
    }




    void SonrakiSoru()
    {
        mevcutSoruIndex++;
        YeniSoruGetir();
    }
}    