using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public int id;
    public string description;
    public Sprite image;
    public float weight;
    public int value;
    public RarityType rarity;
    public ItemType type;
    public ActionType action;
    public bool stackable = true;
    public bool questItem = false;

    public enum RarityType
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public enum ItemType
    {
        Tool,
        Weapon,
        Consumable
    }

    public enum ActionType
    { 
        Unlock,
        Attack,
        Heal
    }
}
