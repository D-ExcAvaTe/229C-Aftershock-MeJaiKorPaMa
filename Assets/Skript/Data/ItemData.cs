using UnityEngine;

[CreateAssetMenu(menuName = "Data/Items",fileName = "New Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public int itemAmount;
    public GameObject itemPrefab;

    public ItemData(string _itemName,int _itemAmount, GameObject _itemPrefab)
    {
        itemName = _itemName;
        itemAmount = _itemAmount;
        itemPrefab = _itemPrefab;
    }
}
