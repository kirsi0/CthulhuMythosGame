using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Null,
    Bottle,      //酒瓶
    HealthPotion, //生命药水
    Bomb          //炸弹
}

public class ItemComponent : BasicComponent {
    
    public List<ItemType> item;
    public int numLimit;
    public ItemType current=ItemType.Null;
}
