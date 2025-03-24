using TMPro;
using UnityEngine;

public class UI_Player : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private TextMeshProUGUI textThrowForce;
    
    void Start()
    {
        
    }

    void Update()
    {
        //if (player.currentDart) 
        textThrowForce.text = $"- แรงโยน: {player.throwForce} +";
        //else textThrowForce.text = "";
    }
}
