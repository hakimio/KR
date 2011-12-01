using UnityEngine;
using System;

public static class Items
{
    public static Medication MedicKit
    {
        get
        {
            return new Medication("Medic Kit", "",
                Helper.getImage("Items/Medication/Medic Kit"), 50);
        }
    }
    public static Armor AhevArmor
    {
        get
        {
            return new Armor("AHEV Armor", "",
                Helper.getImage("Items/Armor/AHEV"), 30);
        }
    }
    public static Armor MarineArmor
    {
        get
        {
            return new Armor("Marine Armor", "",
               Helper.getImage("Items/Armor/Marine"), 20);
        }
    }
    public static Armor RangerArmor
    {
        get
        {
            return new Armor("Ranger Armor", "",
                Helper.getImage("Items/Armor/Ranger"), 15);
        }
    }
    public static Weapon BladeStaff
    {
        get
        {
            return new Weapon("Blade Staff", "",
                Helper.getImage("Items/Weapons/Blade Staff"), 1, 15, 1, 
                WeaponType.Melee);
        }
    }
    public static Weapon CombatKnife
    {
        get
        {
            return new Weapon("Combat Knife", "",
                Helper.getImage("Items/Weapons/Combat Knife"), 1, 10, 1, 
                WeaponType.Melee);
        }
    }
    public static Weapon FlameThrower
    {
        get
        {
            return new Weapon("Flame Thrower", "",
                Helper.getImage("Items/Weapons/Flame thrower"), 3, 20, 4, 
                WeaponType.Ranged);
        }
    }
    public static Weapon LaserCarbine
    {
        get
        {
            return new Weapon("Laser Carbine Rifle", "",
                Helper.getImage("Items/Weapons/Laser Carbine"), 10, 30, 6,
                WeaponType.Ranged);
        }
    }
    public static Weapon LMRifle
    {
        get
        {
            return new Weapon("Linear Magnetic Rifle", "",
                Helper.getImage("Items/Weapons/LM Rifle"), 12, 35, 6,
                WeaponType.Ranged);
        }
    }
    public static Weapon Pistol1
    {
        get
        {
            return new Weapon("Pistol 1", "",
                Helper.getImage("Items/Weapons/pistol1"), 7, 20, 6,
                WeaponType.Ranged);
        }
    }
    public static Weapon Pistol2
    {
        get
        {
            return new Weapon("Pistol 2", "",
                Helper.getImage("Items/Weapons/pistol2"), 7, 23, 5,
                WeaponType.Ranged);
        }
    } 
    public static Weapon Pistol3
    {
        get
        {
            return new Weapon("Pistol 3", "",
                Helper.getImage("Items/Weapons/pistol3"), 7, 25, 4,
                WeaponType.Ranged);
        }
    }
    public static Weapon PlasmaRifle
    {
        get
        {
            return new Weapon("Plasma Rifle", "",
                Helper.getImage("Items/Weapons/Plasma Rifle"), 20, 45, 4,
                WeaponType.Ranged);
        }
    }
}
