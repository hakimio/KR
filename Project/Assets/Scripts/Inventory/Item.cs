using System;
using UnityEngine;

[Serializable]
public class Item
{
    private string name, description;
    private int quantity;
    private ItemState state;
    private Texture2D image;

    public Item(string name, string description, Texture2D image)
    {
        this.name = name;
        this.description = description;
        this.image = image;
        quantity = 1;
        state = ItemState.Positioned;
    }

    public string Name
    {
        get
        {
            return name;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }
    }

    public int Quantity
    {
        get
        {
            return quantity;
        }
        set
        {
            if (value > 0)
                quantity = value;
        }
    }

    public ItemState State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
        }
    }

    public Texture2D Image
    {
        get
        {
            return image;
        }
        set
        {
            image = value;
        }
    }

    public Item Clone()
    {
        return (Item)this.MemberwiseClone();
    }
}

public enum ItemState
{
    Floating,
    Positioned
}