using UnityEngine;
using UnityEngine.UI;

public class MarketItem : MonoBehaviour
{
    [Header("Main Script")]
    private MarketSystem marketSystem;
    [Space(20)]

    [Header("UI Elements")]
    private Image itemImage;
    private Button itemButton;
    public ItemMenu item;
    PlayerData playerData;


    void Start()
    {
        GameObject gameManager = GameObject.FindWithTag("GameManager");
        marketSystem = gameManager.GetComponent<MarketSystem>();
        playerData = gameManager.GetComponent<PlayerData>();
    }

    public void SetImage()
    {

        GameObject backGround = transform.Find("Background").GetChild(0).gameObject;
        itemImage = backGround.GetComponentInChildren<Image>();
        itemButton = backGround.GetComponent<Button>();
        itemButton.onClick.AddListener(() => ChangeSkates());
        itemImage.sprite = item.itemImage;
    }


    public void ChangeSkates()
    {
        playerData.SetDataInt(PlayerPrefEnum.Skates, item.id);
        marketSystem.ChangeShooes(item.itemPrefab, item.id);
    }

}
