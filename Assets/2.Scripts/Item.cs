using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type 
    {
        Weapon,
        Ammo,
        Bullet,

    }
    public Type type;
    public int value;

    

}
