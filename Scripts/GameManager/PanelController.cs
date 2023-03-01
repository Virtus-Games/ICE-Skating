using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public enum MenuType
{
    MARKET,
    EXIT,
    SETTINGS,
    WIN,
    OVER,
    RUN
}

[System.Serializable]
public struct ItemHome
{
    public Button onClickButton;
    public GameObject Ad;
    public TextMeshProUGUI Level;
    public TextMeshProUGUI MoneyText;

}

public class PanelController : MonoBehaviour
{
    #region Envoriments
    [Header("Animated Coints")]
    [SerializeField] private GameObject[] AnimatedCoinPrefab;
    [SerializeField] private Transform Target;
    [SerializeField] private Ease ease;
    public TextMeshProUGUI MoneyText;

    [Space]
    [Header("Money And Player Data")]
    private TextMeshProUGUI moneyText;
    private int __moneyUser;
    PlayerData playerData;
    [Space]
    [Header("Panels")]
    [SerializeField] private GameObject[] Panels;
    [SerializeField] private GameObject MoneyBox;
    LevelManager levelManager;




    #endregion

    #region Home Menu Item
    [Header("Home Item")]
    [SerializeField] private Button CloseButtonSett;
    [SerializeField] private Sprite AdsVideo;
    [SerializeField] private ItemHome SpeedItem;
    [SerializeField] private ItemHome GemsItem;
    private const string __speed = "speed";
    private const string __gems = "gems";
    private string __level = "level";
    private int __lastItemMoney;
    private int __LevelSize;
    private int __notZero;
    private float minDuration;
    #endregion

    private void Start()
    {
        levelManager = GetComponent<LevelManager>();
        playerData = GetComponent<PlayerData>();
        moneyText = MoneyBox.transform.Find("Money").GetComponent<TextMeshProUGUI>();
        GetUserMoneyController();
        ItemSettings(__speed, SpeedItem);
        ItemSettings(__gems, GemsItem);
        HomeItemController();
        OpenPanel(0);

    }

    #region Money Controller

    public void GetUserMoneyController()
    {
        __moneyUser = playerData.GetMoneyUser();
        moneyText.text = __moneyUser.ToString() + "$";
        __moneyUser = 0;
    }

    public void SetUserMoneyController(int value)
    {
        __moneyUser += value;
        playerData.SetDataInt(PlayerPrefEnum.money, playerData.GetDataInt(PlayerPrefEnum.money) + __moneyUser);
        GetUserMoneyController();
    }

    public void CoinsController()
    {
        MoneyText.text = "Clamp X2 " + (GameManager.Current.GetScore() * 2).ToString() + "$";
        SetUserMoneyController(GameManager.Current.GetScore());

        foreach (GameObject item in AnimatedCoinPrefab)
        {
            minDuration = Random.Range(1f, 2f);
            item.transform.
                DOMove(Target.position, minDuration).SetEase(ease).
                OnComplete(() =>
                {
                    MoneyBox.GetComponent<Animator>().SetTrigger("isOpen");
                    Destroy(item);
                });
        }
    }

    #endregion

    #region Panels Controller
    public void OpenPanel(int index)
    {
        foreach (GameObject item in Panels)
        {
            item.gameObject.SetActive(false);
        }

        Panels[index].SetActive(true);
        MoneyBox.SetActive(true);
        Time.timeScale = 1f;

        if (index == 2)
            Time.timeScale = 0f;


    }

    public void CloseSettingsButton(int type)
    {
        if (type == 0)
        {
            CloseButtonCon(0);
            MoneyBox.SetActive(false);
        }
        else
        {
            CloseButtonCon(6);
            MoneyBox.SetActive(false);
        }

    }
    void CloseButtonCon(int value)
    {
        CloseButtonSett.onClick.RemoveAllListeners();
        CloseButtonSett.onClick.AddListener(() => OpenPanel(value));
    }

    #endregion

    #region Home Item Controller
    public void HomeItemController()
    {
        int speedValue = PlayerPrefs.GetInt(__speed + "control");
        int gemsValue = PlayerPrefs.GetInt(__gems + "control");
        SpeedItem.onClickButton.onClick.AddListener(() =>
           SetItemSettings(__speed, SpeedItem, speedValue, PowerType.POWER));
        GemsItem.onClickButton.onClick.AddListener(() =>
           SetItemSettings(__gems, GemsItem, gemsValue, PowerType.HEALTH));
    }

    void SetItemSettings(string PrefName, ItemHome itemHome, int lastVal, PowerType type)
    {

        if (GetMoneyAndLevelSize(PrefName, itemHome))
        {
            PlayerPrefs.SetInt(PrefName, __lastItemMoney + 400);
            PlayerPrefs.SetInt(PrefName + __level, __LevelSize + 1);
            PlayerPrefs.SetInt(PrefName + "control", lastVal + 2);
            GetUserMoneyController();
            __moneyUser -= __lastItemMoney;
            playerData.SetDataInt(PlayerPrefEnum.money, __moneyUser);
            ItemSettings(PrefName, itemHome);
        }
        else
        {
            if (levelManager.ShowReward())
            {
                PlayerPrefs.SetInt(PrefName, __lastItemMoney + 400);
                PlayerPrefs.SetInt(PrefName + __level, __LevelSize + 1);
                PlayerPrefs.SetInt(PrefName + "control", lastVal + 2);
                ItemSettings(PrefName, itemHome);
            }
        }

        GameObject.FindWithTag("Player").GetComponent<CharacterControl>().PowerUpdate(type);

    }

    void ItemSettings(string PrefName, ItemHome itemHome)
    {
        GetMoneyAndLevelSize(PrefName, itemHome);
        itemHome.Level.text = "Level " + __LevelSize.ToString();
        itemHome.MoneyText.text = __lastItemMoney.ToString() + "$";
    }

    bool GetMoneyAndLevelSize(string PrefName, ItemHome itemHome)
    {
        GetUserMoneyController();
        __lastItemMoney = PlayerPrefs.GetInt(PrefName);
        __LevelSize = PlayerPrefs.GetInt(PrefName + __level);
        __notZero = __moneyUser - __lastItemMoney;
        if (__moneyUser >= __lastItemMoney && __notZero >= 0)
        {
            ItemMenuStatus(itemHome, true);
            return true;
        }
        else
        {
            ItemMenuStatus(GemsItem, false); ItemMenuStatus(SpeedItem, false);
            return false;
        }
    }

    void ItemMenuStatus(ItemHome itemHome, bool isStatus)
    {
        itemHome.Ad.gameObject.SetActive(!isStatus);
        itemHome.MoneyText.gameObject.SetActive(isStatus);
    }

    #endregion

}