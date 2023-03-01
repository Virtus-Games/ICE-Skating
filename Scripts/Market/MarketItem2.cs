using UnityEngine;
using UnityEngine.UI;

public class MarketItem2 : MonoBehaviour
{
    [Header("Main Script")]
    private MarketSystem2 marketSystem;

    [Header("UI Elements")]
    private Image itemImage;
    private Button itemButton;
    public ItemMenu item;
    PlayerData playerData;


    void Start()
    {
        GameObject gameManager = GameObject.FindWithTag("GameManager");
        marketSystem = gameManager.GetComponent<MarketSystem2>();
        playerData = gameManager.GetComponent<PlayerData>();

        GameObject backGround = transform.Find("Background").GetChild(0).gameObject;
        itemImage = backGround.GetComponent<Image>();
        itemButton = backGround.GetComponent<Button>();
        itemImage.sprite = item.itemImage;

        if (item.itemType == ItemType.Dress)
            itemButton.onClick.AddListener(() => ChangeDresses());
        else
            itemButton.onClick.AddListener(() => ChangeSkates());
    }

    public void ChangeDresses()
    {
        playerData.SetDataInt(PlayerPrefEnum.Player, item.id);
        marketSystem.ChangeDress(item.itemPrefab);
        marketSystem.SelectItemDress(item.id);
    }

    public void ChangeSkates()
    {
        playerData.SetDataInt(PlayerPrefEnum.Skates, item.id);
        marketSystem.ChangeShooes(item.itemPrefab, item.id);
    }


}
