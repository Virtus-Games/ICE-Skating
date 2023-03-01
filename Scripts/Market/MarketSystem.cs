using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketSystem : MonoBehaviour
{
    PlayerData playerData;
    LevelManager levelManager;
    [Space(20)]
    [Header("Player")]
    [SerializeField] private Transform PlayerPoint;
    [SerializeField] private GameObject PlayerPrefab;
    private GameObject Player;
    private GameObject Patent;
    [SerializeField] private Button BuyButton;
    [SerializeField] private Button AdWatching;
    [SerializeField] private GameObject[] SkatesItemsUi;
    [SerializeField] private ItemSCRP PatentsData;

    [Space(20)]

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI MarketInformationText;
    [SerializeField] private TextMeshProUGUI moneyText;
    private int SkatesId;
    private int PatentMoney;
    private int money;
    private PlayerMarketManager playerMarketManager;
    private GameObject PatentPoint;

    [SerializeField] private GameObject[] Shoeses;
    private void Awake()
    {
        playerData = GetComponent<PlayerData>();
        SkatesId = playerData.GetDataInt(PlayerPrefEnum.Skates);
        Player = Instantiate(PlayerPrefab, PlayerPoint.position, PlayerPoint.rotation);
        Player.transform.SetParent(PlayerPoint);
        Player.transform.SetParent(null);
        GetMoneyController();
        levelManager = GetComponent<LevelManager>();
        playerMarketManager = Player.GetComponent<PlayerMarketManager>();
        BuyButton.onClick.RemoveAllListeners();
        BuyButton.onClick.AddListener(() => BuyButtonController());
        StartControllerItemsOfItem(SkatesId);
        AdWatching.onClick.RemoveAllListeners();

        AdWatching.onClick.AddListener(() =>
        {
            if (levelManager.ShowReward())
            {
                //BuyItem(false, PatentMoney, 250, PatentMoney);
                BuyItem(true, PatentMoney, 400, 0);
            }

        });

    }



    public void BuyButtonController()
    {
        BuyItem(false, PatentMoney, 350, PatentMoney);
    }
    void GetMoneyController()
    {
        money = playerData.GetMoneyUser();
        moneyText.text = money.ToString() + "$";
        PatentMoney = playerData.GetDataInt(PlayerPrefEnum.PatentSellMoney);
        if(PatentMoney == 0)
            PatentMoney = 500;

        BuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Buy " + PatentMoney.ToString() + "$";

    }


    void StartControllerItemsOfItem(int id)
    {
        for (int i = 0; i < PatentsData.items.Length; i++)
        {
            MarketItem item = SkatesItemsUi[i].GetComponent<MarketItem>();

            item.item = PatentsData.items[i];

            if (item.item.id == id)
                ChangeShooes(item.item.itemPrefab, i);

            if (PlayerPrefs.GetInt(item.item.itemName) == 2)
                SkatesItemsUi[i].gameObject.transform.Find("Closed").GetComponent<Image>().enabled = false;

            item.SetImage();

        }
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

    void BuyItem(bool isFree, int prefValue, int incerementItemMoney, int itemMoney)
    {

        int haveItem = playerData.GetDataInt(PlayerPrefEnum.Skates);

        if (haveItem <= 9)
        {
            if (money >= itemMoney || isFree)
            {
                for (int i = 0; i < PatentsData.items.Length; i++)
                {
                    if (PlayerPrefs.GetInt(PatentsData.items[i].itemName) != 2)
                    {
                        SkatesItemsUi[i].transform.Find("Closed").GetComponent<Image>().enabled = false;
                        PlayerPrefs.SetInt(PatentsData.items[i].itemName, 2);

                        if (!isFree)
                        {
                            money -= prefValue;
                            playerData.SetDataInt(PlayerPrefEnum.money, money);
                            GetMoneyController();
                        }

                        playerData.SetDataInt(PlayerPrefEnum.Skates, haveItem + 1);
                        prefValue += incerementItemMoney;
                        playerData.SetDataInt(PlayerPrefEnum.PatentSellMoney, prefValue);
                        GetMoneyController();
                        break;
                    }
                }
            }
            else
                moneyText.GetComponentInParent<Animator>().SetTrigger("isOpen");
        }
        else
            StartCoroutine(InfoTextENUM("Skates"));
    }


    public void ChangeShoes(GameObject obj)
    {
        SkinnedMeshRenderer sourceMesh = obj.GetComponent<SkinnedMeshRenderer>();
        foreach (GameObject item in Shoeses)
        {
            SkinnedMeshRenderer mesh = item.GetComponentInChildren<SkinnedMeshRenderer>();
            mesh.sharedMesh = sourceMesh.sharedMesh;
            mesh.sharedMaterial = sourceMesh.sharedMaterial;
            sourceMesh.bones = sourceMesh.bones;
            sourceMesh.rootBone = mesh.rootBone;
        }

    }


    IEnumerator InfoTextENUM(string itemName)
    {
        MarketInformationText.text = "You have all" + itemName;
        MarketInformationText.enabled = true;
        yield return new WaitForSeconds(2);
        MarketInformationText.enabled = false;

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
        ChangeShoes(Patent);

    }
}        // for (int i = 0; i < dataVal.Count; i++)
         // {

//     if (sens[i].Length > sens[i + 1].Length)
//         result = sens[i];

//     return result;

// }

