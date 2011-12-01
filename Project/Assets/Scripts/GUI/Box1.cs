using UnityEngine;
using System.Collections;

public class Box1: BoxGUITemplate
{
    protected override void addItems()
    {
        Item medicKit = Items.MedicKit;
        medicKit.Quantity = 2;
        Item[] items = new Item[]{Items.AhevArmor, Items.LMRifle, 
            Items.Pistol3, medicKit};
        storedItems.AddRange(items);
    }
}
