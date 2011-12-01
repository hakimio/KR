using UnityEngine;
using System.Collections;

public static class GUIHelper 
{
    public static string getInfo(Item item)
    {
        string info = item.Name + "\n";

        if (item is Weapon)
        {
            Weapon weapon = (Weapon)item;
            info += "Range  " + weapon.Range + "\n";
            info += "Damage " + weapon.Damage;
            if (weapon.Type == WeaponType.Ranged)
            {
                info += "\nNo of Shots " + weapon.Bulets;
            }
        }
        else if (item is Medication)
        {
            Medication drug = (Medication)item;
            info += "Restores " + drug.RestoredHP + " HP";
        }
        else if (item is Armor)
        {
            Armor armor = (Armor)item;
            info += "Defense " + armor.Defense;
        }
        else if (item is Key)
        {
            info += item.Description;
        }

        return info;
    }
}
