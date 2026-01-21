using TMPro;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;
    public TMP_Text goldText;
    private int gold;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gold = 0;
        UpdateGoldUI();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldUI();
    }

    public void SetGoldAmount(int amount)
    {
        gold = amount;
        UpdateGoldUI();
    }

    public int GetGoldAmount()
    {
        return gold;
    }

    private void UpdateGoldUI()
    {
        goldText.text = "Gold: " + gold;
    }
}
