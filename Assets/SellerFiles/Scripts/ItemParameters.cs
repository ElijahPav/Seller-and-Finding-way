using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ItemParameters")]
public class ItemParameters : ScriptableObject
{
    public int PlayerPrice;
    public int SellerPrice;
    public Sprite Image;
}
