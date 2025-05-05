using UnityEngine;

public class TestController : MonoBehaviour
{

    private int skor;
    [SerializeField] private int skorLimit;
    /// <summary>
    /// Oyun başlamadan önce 1 kereliğine çalışan fonksiyon
    /// </summary>
    void Awake()
    {
        ClearSkore();
        SetPlayerGameBefore();
    }

    //Oyun ile açıldığında çalışan method
    void Start()
    {
        Debug.Log("Oyun başladı");
    }

    //Oyun içinde her frame çalışan method
    void Update()
    {
        Debug.Log("Çalışmaya devam ediyor");
    }

    //Fiziksel objelerin hareketlerini bu alanda kontrol ediyoruz
    void FixedUpdate()
    {
        
    }

    void ClearSkore(){
        Debug.Log("Skorlar sıfırlandı");
    }
    void SetPlayerGameBefore()
    {
        print("Böyle bişi var");
        Debug.LogWarning("Oyuncu oyuna başlamadan önceki ayarlarını yap");
        Debug.LogError("Oyuncu oyuna başlamadan önceki ayarlarını yap");
        Debug.Log("Oyuncu oyuna başlamadan önceki ayarlarını yap");
    }
}
