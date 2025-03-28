using System;
using UnityEngine;

public class SellBooth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemPrefab hitItem = other.GetComponent<ItemPrefab>();
       
        if(hitItem==null) return;
        
        ItemData items = hitItem.thisItemData;
        if (items.sellPrice <= 0) return;
        
        PlayerData.instance.AddMoney(items.sellPrice);
        Destroy(other.gameObject);
    }
}
