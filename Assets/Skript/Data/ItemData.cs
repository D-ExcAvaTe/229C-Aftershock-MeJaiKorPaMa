using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Data/Items",fileName = "New Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public int sellPrice = 0;
    public GameObject itemPrefab;

    public ItemData(string _itemName,int _sellPrice, GameObject _itemPrefab)
    {
        itemName = _itemName;
        sellPrice = _sellPrice;
        itemPrefab = _itemPrefab;
    }
}
