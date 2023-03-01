using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippingManager : MonoBehaviour
{
    [SerializeField] private ItemSCRP shoesItems;

    [SerializeField] private ItemSCRP Dress;

    [SerializeField] private ShoesCustomization shoesCustomization;

    [SerializeField] private ClothCustomization clothCustomization;


    public void ApplyCloth(GameObject cloth, int val)
    {
        // foreach (var item in clothes.costumes)
        // {
        //     if (cloth.name.Equals(item.obj.name))
        //     {
        //         clothCustomization.ChangCloth(item.obj);
        //     }
        // }
        //shoesCustomization.ChangeShoes(shoes.costumes[val].obj);
        
        if (shoesItems.items[val].id == val)
            shoesCustomization.ChangeShoes(Dress.items[val].itemPrefab);


    }


    public void ApplyShoes(GameObject shoe, int val)
    {
        // foreach (var item in shoes.costumes)
        // {
        //     Debug.Log(shoe.name);
        //     Debug.Log(item.obj.name);

        //     if (item.obj.name.StartsWith(shoe.name + "(Clone)"))
        //     {
        //         shoesCustomization.ChangeShoes(item.obj);
        //         break;
        //     }
        // }

        //shoesCustomization.ChangeShoes(shoes.costumes[val].obj);

        if (shoesItems.items[val].id == val)
            shoesCustomization.ChangeShoes(shoesItems.items[val].itemPrefab);

    }
}
