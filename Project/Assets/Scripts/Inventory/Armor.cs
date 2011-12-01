using UnityEngine;
using System;

public class Armor: Item
{
    private int defense;

    public Armor(string name, string description, Texture2D image, int defense)
        : base(name, description, image)
    {
        this.defense = defense;
    }

    public int Defense
    {
        get
        {
            return defense;
        }
    }
}
