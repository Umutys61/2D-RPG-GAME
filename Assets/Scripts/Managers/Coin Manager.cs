using BayatGames.SaveGameFree;
using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
    [SerializeField] private float CoinTest=1000;
    public float Coins { get; private set; }
    private const string COIN_KEY = "Coins";

    private void Start()
    {
        Coins = SaveGame.Exists(COIN_KEY) 
            ? SaveGame.Load<float>(COIN_KEY) 
            : CoinTest;

        Debug.Log($"Coins yüklendi: {Coins}");
    }

    public void AddCoins(float amount)
    {
        Coins += amount;
        SaveGame.Save(COIN_KEY, Coins);
        Debug.Log($"+ {amount} coin eklendi! Toplam: {Coins}");
    }

    public bool RemoveCoins(float amount)
    {
        if (Coins >= amount)
        {
            Coins -= amount;
            SaveGame.Save(COIN_KEY, Coins);
            Debug.Log($"- {amount} coin harcandı! Kalan: {Coins}");
            return true;
        }
        else
        {
            Debug.LogWarning("Yetersiz coin!");
            return false;
        }
    }
}
