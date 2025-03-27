using TMPro;
using UnityEngine;

public class UI_Player : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private TextMeshProUGUI textThrowForce, textMoney,textThrowLeft,textMainHand;
    
    
    void Start()
    {
        
    }

    void Update()
    {
        //if (player.currentDart) 
        textThrowForce.text = $"{player.throwForce}";
        textMoney.text = $"{PlayerData.instance.GetMoney} บาท";
        textThrowLeft.text = $"คลิกซ้าย ({PlayerController.instance.dartForceAdded}/{PlayerController.instance.maxDartForceAdd})";
        //else textThrowForce.text = "";

        if (PlayerController.instance.mainHandPrefab != null)
        {
            textMainHand.gameObject.SetActive(true);
            textMainHand.text = $"{PlayerController.instance.mainHandItem.itemName}\n\"Q\" ดรอป";
        } else textMainHand.gameObject.SetActive(false);

    }
}
