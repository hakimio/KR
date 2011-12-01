using UnityEngine;
using System;

public class Medication: Item
{
    private int restoredHP;

    public Medication(string name, string description, Texture2D image,
        int restoredHP): base(name, description, image)
    {
        this.restoredHP = restoredHP;
    }

    public int RestoredHP
    {
        get
        {
            return restoredHP;
        }
        set
        {
            if (value > 0)
                restoredHP = value;
        }
    }
}
