using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MinigameBalloon : Minigame
{
    [SerializeField] private Balloon balloon;
    [SerializeField] private Material[] balloonMaterial;
    [SerializeField] private Transform balloonSpawnPosParent, dropItemPosParent;
    [SerializeField] private List<Transform> balloonSpawnPos,dropItemSpawnPos;
    [Space]
    
    [Header("Dart")]
    [SerializeField] private ItemData dartItemData;
    public int ballonPopped=0,dartShooted, maxDart = 3;
    
    public static MinigameBalloon instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    protected override void Start()
    {
        base.Start();
        Init();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void Init()
    {
        foreach (Transform child in balloonSpawnPosParent) balloonSpawnPos.Add(child);
        foreach (Transform child in dropItemPosParent) dropItemSpawnPos.Add(child);
    }

    protected override void StartGame()
    {
        if (PlayerData.instance.TakeMoney(playCost))
        {
            base.StartGame();

            foreach (Transform child in balloonSpawnPosParent)
            foreach (Transform _child in child)
                Destroy(_child.gameObject);
            foreach (Transform child in dropItemPosParent)
            foreach (Transform _child in child)
                Destroy(_child.gameObject);

            for (int i = 0; i < balloonSpawnPos.Count; i++)
            {
                Balloon _ballon = Instantiate(balloon, balloonSpawnPos[i].position, Quaternion.identity,
                    balloonSpawnPos[i]);
                _ballon.Init(balloonMaterial[Random.Range(0, balloonMaterial.Length)], this);
            }

            for (int i = 0; i < dropItemSpawnPos.Count; i++)
            {
                ItemPrefab item = Instantiate(PlayerController.instance.itemPrefab,
                    dropItemSpawnPos[i].transform.position,
                    Quaternion.identity, dropItemSpawnPos[i]);
                item.Init(dartItemData);
            }

            ballonPopped = 0;
            ShowInstruction("ใช้ลูกดอก 3 ลูกปาลูกโป่ง\nให้แตก 3 ลูก เพื่อรับรางวัล");
        }
    }

    public void AddDart()
    {
        dartShooted++;
        if (dartShooted >= maxDart)
        {
            dartShooted = 0;
            
            ResetGame();
            
            canStart = true;
        }
    }

    protected override void ResetGame()
    {
        if (ballonPopped >= 3) DropRewards();
        ballonPopped = 0;
        base.ResetGame();
    }
}
