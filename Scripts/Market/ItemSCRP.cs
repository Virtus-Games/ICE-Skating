using UnityEngine;


[CreateAssetMenu(fileName = "ItemSCRP", menuName = "Item/item")]
public class ItemSCRP : ScriptableObject
{
    public ItemMenu[] items;
    public int itemsCount {get{ return items.Length;}}
    public int haveItemCount = 1;
}

[System.Serializable]
public struct ItemMenu
{
    public int id;
    public int statusNumber;
    public string itemName;
    public float itemPrice;
    public bool isPurchased;
    public Sprite itemImage;
    public GameObject itemPrefab;
    public ItemType itemType;
}

public enum ItemType
{
    Dress,
    Skates
}