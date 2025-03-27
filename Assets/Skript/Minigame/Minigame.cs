using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Minigame : MonoBehaviour
{
    public int playCost = 50;
    [SerializeField] private KeyCode holdStartKey;
    [Space]
    [SerializeField] private GameObject startGamePanel;

    [SerializeField] private Slider holdSlider;
    [SerializeField] private TextMeshProUGUI holdText,costText,instructionText;
    private Coroutine instructionCoroutine;
    
    public bool isStarted;

    [SerializeField] protected bool canStart = false;
    private float holdStartTimer, holdStartDuration = 1f;

    [Space] [SerializeField] protected Transform rewardDisplayParent, rewardDropParent;
    [SerializeField] protected List<Transform> rewardDisplayPos;
    [SerializeField] protected ItemData[] rewards;
    protected virtual void Start()
    {
        ResetGame();

        foreach (Transform child in rewardDisplayParent)
            rewardDisplayPos.Add(child);
        
        for (int i = 0; i < rewards.Length; i++)
        {
            if (rewardDisplayPos[i] == null) continue;
            
            ItemPrefab rew = Instantiate(PlayerController.instance.itemPrefab, rewardDisplayPos[i].transform.position,
                Quaternion.identity, rewardDisplayPos[i]);

            rew.Init(rewards[i]);
        }
    }

    protected virtual void Update()
    {
        holdSlider.value = holdStartTimer / holdStartDuration;
        
        if (!PlayerData.instance.HaveEnoughMoney(playCost))
        {
            costText.color = Color.red;
            costText.text = playCost + " บาท (เงินไม่เพียงพอ)";
        }
        else
        {
            costText.color = Color.green;
            costText.text = playCost + " บาท";
        }
        
        if (canStart && !isStarted)
        {
            startGamePanel.SetActive(true);
            
            if (Input.GetKey(holdStartKey))
            {
                holdStartTimer += Time.deltaTime;
                if (holdStartTimer >= holdStartDuration) StartGame();
            } else if (holdStartTimer > 0) holdStartTimer -= Time.deltaTime;
            return;
        }
        
        if (holdStartTimer > 0) holdStartTimer -= Time.deltaTime;
        else startGamePanel.SetActive(false);
        
    }

    protected virtual void ResetGame()
    {
        Debug.Log("Reset Game");
        isStarted = false;
        canStart = false;
        holdText.text = holdStartKey + "";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isStarted) return;
        canStart = other.GetComponent<PlayerController>() != null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (isStarted) return;
        if (holdStartTimer > 0) holdStartTimer -= Time.deltaTime;
        canStart = false;
    }

    protected virtual void StartGame()
    {
        isStarted = true;
        holdStartTimer = 0;
        canStart = false;
        Debug.Log("(base) Minigame Started");
    }

    protected void ShowInstruction(string _instruction)
    {
        if(instructionCoroutine!=null) StopCoroutine(instructionCoroutine);
        StartCoroutine(ShowInstructionDelay(_instruction));
    }

    IEnumerator ShowInstructionDelay(string _instruction)
    {
        instructionText.gameObject.SetActive(true);
        instructionText.text = _instruction;
        yield return new WaitForSeconds(_instruction.Length * 0.2f);
        instructionText.gameObject.SetActive(false);
    }

    protected void DropRewards()
    {
        int randomRewardIndex = Random.Range(0, rewards.Length);
        
        ItemPrefab reward = Instantiate(PlayerController.instance.itemPrefab, rewardDropParent.position,
            Quaternion.identity, rewardDropParent).GetComponent<ItemPrefab>();
        
        reward.Init(rewards[randomRewardIndex]);

    }
}
