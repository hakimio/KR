using UnityEngine;
using System;

public class Weapon: Item
{
    private int range, damage, bullets;
    private WeaponType type;

    public Weapon(string name, string description, Texture2D image, int range,
        int damage, int bullets, WeaponType type)
        : base(name, description, image)
    {
        this.range = range;
        this.damage = damage;
        this.bullets = bullets;
        this.type = type;
    }

    public int Range
    {
        get
        {
            return range;
        }
        set
        {
            if (value > 0)
                range = value;
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            if (value > 0)
                damage = value;
        }
    }

    public int Bulets
    {
        get
        {
            return bullets;
        }
        set
        {
            if (value > 0)
                bullets = value;
        }
    }

    public WeaponType Type
    {
        get
        {
            return type;
        }
    }
}

public enum WeaponType
{
    Ranged,
    Melee
}
