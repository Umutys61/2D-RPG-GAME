using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private PlayerStats stats;

    [Header("Bars")]
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image ManaBar;
    [SerializeField] private Image ExpBar;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI LevelTMP;
    [SerializeField] private TextMeshProUGUI HealthTMP;
    [SerializeField] private TextMeshProUGUI ManaTMP;
    [SerializeField] private TextMeshProUGUI ExpTMP;
    [SerializeField] private TextMeshProUGUI coinsTMP;

    [Header("Stat Panel")]
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private TextMeshProUGUI statLevelTMP;
    [SerializeField] private TextMeshProUGUI statDamageTMP;
    [SerializeField] private TextMeshProUGUI statCChanceTMP;
    [SerializeField] private TextMeshProUGUI statCDamageTMP;
    [SerializeField] private TextMeshProUGUI statTotalExpTMP;
    [SerializeField] private TextMeshProUGUI statCurrentExpTMP;
    [SerializeField] private TextMeshProUGUI statRequiredExpTMP;

    [Header("Attributes")]
    [SerializeField] private TextMeshProUGUI attributePointsTMP;
    [SerializeField] private TextMeshProUGUI strengthTMP;
    [SerializeField] private TextMeshProUGUI dexterityTMP;
    [SerializeField] private TextMeshProUGUI intelligenceTMP;

    [Header("Gathering")]
    [SerializeField] private TextMeshProUGUI woodCuttingTMP;
    [SerializeField] private TextMeshProUGUI miningTMP;
    [SerializeField] private TextMeshProUGUI fishingTMP;

    [Header("Combat Skills TMP")]
    [SerializeField] private TextMeshProUGUI meleeTMP;
    [SerializeField] private TextMeshProUGUI magicTMP;
    [SerializeField] private TextMeshProUGUI bowTMP;
    [SerializeField] private TextMeshProUGUI defenseTMP;

    [Header("Resist & Regen TMP")]
    [SerializeField] private TextMeshProUGUI magicResistTMP;
    [SerializeField] private TextMeshProUGUI healthRegenTMP;
    [SerializeField] private TextMeshProUGUI manaRegenTMP;

    [Header("Extra Panels")]
    [SerializeField] private GameObject npcQuestPanel;
    [SerializeField] private GameObject playerQuestPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject craftingPanel;

    private void Update()
    {
        UpdatePlayerUI();
    }

    public void OpenCloseStatsPanel()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
        if (statsPanel.activeSelf)
        {
            UpdateStatsPanel();
        }
    }

    public void OpenCloseNPCQuestPanel(bool value)
    {
        npcQuestPanel.SetActive(value);
    }

    public void OpenClosePlayerQuestPanel(bool value)
    {
        playerQuestPanel.SetActive(value);
    }

    public void OpenCloseShopPanel(bool value)
    {
        shopPanel.SetActive(value);
    }

    public void OpenCloseCraftingPanel(bool value)
    {
        craftingPanel.SetActive(value);
    }

    private void UpdatePlayerUI()
    {
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, stats.health / stats.maxHealth, Time.deltaTime * 10f);
        ManaBar.fillAmount = Mathf.Lerp(ManaBar.fillAmount, stats.mana / stats.maxMana, Time.deltaTime * 10f);
        ExpBar.fillAmount = Mathf.Lerp(ExpBar.fillAmount, stats.currentExp / stats.nextLevelExp, Time.deltaTime * 10f);

        LevelTMP.text = $"Level {stats.level}";
        HealthTMP.text = $"{Mathf.RoundToInt(stats.health)}/{Mathf.RoundToInt(stats.maxHealth)}";
        ManaTMP.text   = $"{Mathf.RoundToInt(stats.mana)}/{Mathf.RoundToInt(stats.maxMana)}";
        ExpTMP.text    = $"{Mathf.RoundToInt(stats.currentExp)}/{Mathf.RoundToInt(stats.nextLevelExp)}";

        coinsTMP.text = CoinManager.Instance.Coins.ToString();
    }

    private void UpdateStatsPanel()
    {
        statLevelTMP.text = stats.level.ToString();
        statDamageTMP.text = stats.TotalDamage.ToString();
        statCChanceTMP.text = stats.CriticalChance.ToString();
        statCDamageTMP.text = stats.CriticalDamage.ToString();
        statTotalExpTMP.text = stats.TotalExp.ToString();
        statCurrentExpTMP.text = stats.currentExp.ToString();
        statRequiredExpTMP.text = stats.nextLevelExp.ToString();

        attributePointsTMP.text = $"Points:{stats.AttributePoints}";
        strengthTMP.text = stats.Strength.ToString();
        dexterityTMP.text = stats.Dexterity.ToString();
        intelligenceTMP.text = stats.Intelligence.ToString();

        woodCuttingTMP.text = $"Lv {stats.woodcuttingLevel} ({stats.woodcuttingExp}/{stats.woodcuttingNextExp})";
        miningTMP.text = $"Lv {stats.miningLevel} ({stats.miningExp}/{stats.miningNextExp})";
        fishingTMP.text = $"Lv {stats.fishingLevel} ({stats.fishingExp}/{stats.fishingNextExp})";

        float meleePercent = (float)stats.meleeExp / stats.meleeNextExp * 100f;
        float magicPercent = (float)stats.magicExp / stats.magicNextExp * 100f;
        float bowPercent = (float)stats.bowExp / stats.bowNextExp * 100f;
        float defensePercent = (float)stats.defenseExp / stats.defenseNextExp * 100f;

        meleeTMP.text = $"Lv {stats.meleeLevel} ({meleePercent:0}%)";
        magicTMP.text = $"Lv {stats.magicLevel} ({magicPercent:0}%)";
        bowTMP.text = $"Lv {stats.bowLevel} ({bowPercent:0}%)";
        defenseTMP.text = $"Lv {stats.defenseLevel} ({defensePercent:0}%)";

        magicResistTMP.text = $"{stats.magicResist}%";
        healthRegenTMP.text = $"+{stats.healthRegenRate}/s";
        manaRegenTMP.text = $"+{stats.manaRegenRate}/s";

    }

    private void UpgradeCallBack()
    {
        UpdateStatsPanel();
    }

    private void ExtraInteractionCallback(InteractionType type)
    {
        switch (type)
        {
            case InteractionType.Quest:
                OpenCloseNPCQuestPanel(true);
                break;
            case InteractionType.Shop:
                OpenCloseShopPanel(true);
                break;
            case InteractionType.Crafting:
                OpenCloseCraftingPanel(true);
                break;
        }
    }

    private void OnEnable()
    {
        PlayerUpgrade.OnPlayerUpgradeEvent += UpgradeCallBack;
        DialogueManager.OnExtraInteractionEvent += ExtraInteractionCallback;
    }

    private void OnDisable()
    {
        PlayerUpgrade.OnPlayerUpgradeEvent -= UpgradeCallBack;
        DialogueManager.OnExtraInteractionEvent -= ExtraInteractionCallback;
    }
}
