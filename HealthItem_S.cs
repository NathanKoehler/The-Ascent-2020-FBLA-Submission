using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Item", menuName = "Health Item")]
[System.Serializable]
public class HealthItem_S : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public int order;
}
