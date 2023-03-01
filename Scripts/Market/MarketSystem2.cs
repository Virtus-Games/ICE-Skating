using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MarketSystem2 : MonoBehaviour
{
    PlayerData playerData;
    LevelManager levelManager;

    #region Values Settings

    [Space(20)]
    [Header("Player")]
    [SerializeField] private Transform PlayerPoint;
    [SerializeField] private GameObject PatentPoint;
    private GameObject Player;
    private GameObject Patent;
    private PlayerMarketManager playerMarketManager;


    [Header("Panels")]
    [SerializeField] private ImageButton LeftButtonImage;
    [SerializeField] private ImageButton RightButtonImage;
    [SerializeField] private GameObject[] panels;
    [SerializeField] private Button LeftButton;
    [SerializeField] private Button RightButton;
    [SerializeField] private Button BuyButton;
    [SerializeField] private Button AdWatching;

    [Header("Items")]

    [SerializeField] private ItemSCRP DressData;
    [SerializeField] private ItemSCRP PatentsData;
    [SerializeField] private GameObject[] DressItemsUi;
    [SerializeField] private GameObject[] SkatesItemsUi;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI MarketInformationText;
    [SerializeField] private TextMeshProUGUI moneyText;

    private int PlayerId;
    private int SkatesId;

    private int charackterMoney;
    private int PatentMoney;
    private int money;

    #endregion
    bool isStatus = false;

    private void Awake()
    {

        levelManager = GetComponent<LevelManager>();
        playerData = GetComponent<PlayerData>();
        PlayerPrefsController();
        ButtonController();
        SetList();

    }

    void Start()
    {
        ChangePanel(ButtonType.Dress);
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    #region ListController
    void PlayerPrefsController()
    {
        PlayerId = playerData.GetDataInt(PlayerPrefEnum.Player);
        SkatesId = playerData.GetDataInt(PlayerPrefEnum.Skates);
        GetMoneyController();

    }
    void ButtonController()
    {
        LeftButton.onClick.RemoveAllListeners();
        RightButton.onClick.RemoveAllListeners();
        LeftButton.onClick.AddListener(() => ChangePanel(ButtonType.Dress));
        RightButton.onClick.AddListener(() => ChangePanel(ButtonType.Skates));
        BuyButton.onClick.AddListener(() => BuyButtonController(ButtonType.Dress));
    }
    void SetList()
    {
        ValuesController(PlayerId, DressData.itemsCount, DressItemsUi, DressData.items);
    }
    void ValuesController(int id, int itemCounts, GameObject[] itemsUi, ItemMenu[] menu)
    {
        for (int i = 0; i < itemCounts; i++)
        {
            MarketItem2 item2 = itemsUi[i].GetComponent<MarketItem2>();

            item2.item = menu[i];

            if (item2.item.id == id)
            {
                if (menu[i].itemType == ItemType.Dress)
                    ChangeDress(item2.item.itemPrefab);
                else
                    ChangeShooes(item2.item.itemPrefab, i);
            }

            if (PlayerPrefs.GetInt(item2.item.itemName) == 2)
                itemsUi[i].gameObject.transform.Find("Closed").GetComponent<Image>().enabled = false;
        }


    }
    void GetMoneyController()
    {
        money = playerData.GetMoneyUser();
        moneyText.text = money.ToString() + "$";
        charackterMoney = playerData.GetDataInt(PlayerPrefEnum.CharackerSellMoney);
        PatentMoney = playerData.GetDataInt(PlayerPrefEnum.PatentSellMoney);
    }

    #endregion

    #region Market Panels
    public void ChangePanel(ButtonType type)
    {
        GetMoneyController();

        if (type == ButtonType.Dress)
        {
            SetPanels(1, 0, LeftButton, RightButton, LeftButtonImage.OpenSprite, RightButtonImage.CloseSprite);
            ButtonTypeController(ButtonType.Dress, charackterMoney);
            AdWatching.onClick.RemoveAllListeners();
            AdWatching.onClick.AddListener(() =>
            {
                if (levelManager.ShowReward())
                {
                    BuyItem(true, PlayerPrefEnum.DressCount, charackterMoney,
                    DressItemsUi.Length, DressItemsUi, DressData, charackterMoney, 0, PlayerPrefEnum.CharackerSellMoney, "Dress");
                }
            }
            );

        }

        if (type == ButtonType.Skates)
        {
            SetPanels(0, 1, LeftButton, RightButton, LeftButtonImage.CloseSprite, RightButtonImage.OpenSprite);
            ButtonTypeController(ButtonType.Skates, PatentMoney);
            AdWatching.onClick.RemoveAllListeners();
            AdWatching.onClick.AddListener(() =>
            {
                if (levelManager.ShowReward())
                {
                    BuyItem(true, PlayerPrefEnum.PatentCount, PatentMoney,
                        PatentsData.itemsCount, SkatesItemsUi, PatentsData, PatentMoney, 0, PlayerPrefEnum.PatentSellMoney, "Skates");
                }
            });
        }
    }


    void ButtonTypeController(ButtonType type, int prefItemValue)
    {
        BuyButton.onClick.RemoveAllListeners();
        BuyButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Buy " + prefItemValue.ToString() + "$";
        BuyButton.onClick.AddListener(() => BuyButtonController(type));
    }
    void SetPanels(
        int firts, int seconds,
        Button firtsButton, Button secondsButton,
        Sprite firtsSprite, Sprite secondsSprite)
    {
        panels[firts].SetActive(false);
        firtsButton.GetComponent<Image>().sprite = firtsSprite;
        secondsButton.GetComponent<Image>().sprite = secondsSprite;
        panels[seconds].SetActive(true);
    }

    #endregion

    #region ItemsController

    public void SelectItemDress(int index)
    {

        ObjSelect(index, DressItemsUi);

    }
    public void SelectItemSkates(int index)
    {
        ObjSelect(index, SkatesItemsUi);
    }

    public GameObject ObjSelect(int index, GameObject[] items)
    {
        Elements(items);
        GameObject obj = items[index].gameObject;
        obj.GetComponentInChildren<Outline>().enabled = true;
        return obj;
    }

    void Elements(GameObject[] objs)
    {
        foreach (GameObject item in objs)
        {
            item.GetComponentInChildren<Outline>().enabled = false;
        }
    }

    #endregion

    #region BuyButton
    public void BuyButtonController(ButtonType type)
    {
        GetMoneyController();

        if (type == ButtonType.Dress)
        {
            BuyItem(false, PlayerPrefEnum.DressCount, charackterMoney,
                DressItemsUi.Length, DressItemsUi, DressData, charackterMoney, 400, PlayerPrefEnum.CharackerSellMoney, "Dress");
            BuyButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Buy " + charackterMoney.ToString() + "$";
        }
        if (type == ButtonType.Skates)
        {
            BuyItem(false, PlayerPrefEnum.PatentCount, PatentMoney,
             PatentsData.itemsCount, SkatesItemsUi, PatentsData, PatentMoney, 100, PlayerPrefEnum.PatentSellMoney, "Skates");
            BuyButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Buy " + PatentMoney.ToString() + "$";
        }

    }


    void BuyItem(bool isFree, PlayerPrefEnum playerPrefNameSpace, int itemMoney, int itemCount, GameObject[]
            items, ItemSCRP itemData, int prefValue,
            int incerementItemMoney, PlayerPrefEnum playerPrefCItemSell, string itemName)
    {

        int haveItem = playerData.GetDataInt(playerPrefNameSpace);

        if (haveItem <= 9)
        {
            if (money >= itemMoney || isFree)
            {
                for (int i = 0; i < itemCount; i++)
                {
                    if (PlayerPrefs.GetInt(itemData.items[i].itemName) != 2)
                    {
                        items[i].transform.Find("Closed").GetComponent<Image>().enabled = false;
                        PlayerPrefs.SetInt(itemData.items[i].itemName, 2);

                        if (!isFree)
                        {
                            money -= prefValue;
                            playerData.SetDataInt(PlayerPrefEnum.money, money);
                            GetMoneyController();
                        }

                        playerData.SetDataInt(playerPrefNameSpace, haveItem + 1);
                        playerData.SetDataInt(playerPrefCItemSell, prefValue + incerementItemMoney);
                        prefValue += incerementItemMoney;
                        BuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Buy " + prefValue.ToString() + "$";
                        break;
                    }
                }
            }
            else
                moneyText.GetComponentInParent<Animator>().SetTrigger("isOpen");
        }
        else
            StartCoroutine(InfoTextENUM(itemName));
    }


    IEnumerator InfoTextENUM(string itemName)
    {
        MarketInformationText.text = "You have all" + itemName;
        MarketInformationText.enabled = true;
        yield return new WaitForSeconds(2);
        MarketInformationText.enabled = false;

    }

    #endregion

    public void ChangeDress(GameObject newPlayer)
    {
        Destroy(Player);
        GameObject p = Instantiate(newPlayer, PlayerPoint);
        p.gameObject.tag = "Player";
        p.transform.position = PlayerPoint.transform.position;
        Player = p;
        playerMarketManager = Player.GetComponent<PlayerMarketManager>();
        Player.transform.SetParent(null);

        ValuesController(SkatesId, PatentsData.itemsCount, SkatesItemsUi, PatentsData.items);
    }
    public void ChangeShooes(GameObject newPlayer, int val)
    {
        PatentPoint = playerMarketManager.Shoses_Point.gameObject;

        if (Patent != null)
            Destroy(Patent);

        GameObject p = Instantiate(newPlayer, PatentPoint.transform);
        playerMarketManager.EquippingManager.ApplyShoes(p, val);
        PatentPoint.gameObject.tag = "Patent";
        Patent = p;

    }
    public void ChangeSkatesInt(int val)
    {
        Debug.Log(playerMarketManager.PatentParent.name);
        Transform[] childs = playerMarketManager.PatentParent.GetComponentsInChildren<Transform>();
        foreach (Transform item in childs)
        {
            item.gameObject.SetActive(false);
        }

        Debug.Log(val);
        playerMarketManager.PatentParent.SetActive(true);
        Player.
            transform.
                Find(playerMarketManager.PatentParent.name).
                    transform.Find(val.ToString()).gameObject.SetActive(true);

        Debug.Log(playerMarketManager.PatentParent.name);
    }

}


[System.Serializable]
public enum ButtonType
{
    Dress,
    Skates,
    ADWATCHING
}

[System.Serializable]
public struct ImageButton
{
    public Sprite CloseSprite;
    public Sprite OpenSprite;
}
