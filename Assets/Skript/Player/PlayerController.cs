using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeedMultiplier = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;

    [Space]
    [Header("Mainhand Items")]
    public ItemPrefab itemPrefab;
    [SerializeField] public ItemData mainHandItem;
    [SerializeField] public GameObject mainHandPrefab;
    [SerializeField] private Transform handParent, playerCamera;
    [Space]
    
    [Header("Dart Throwing")]
    public float throwForce = 0;
    [SerializeField] private float minThrowForce = 0f, maxThrowForce = 1000f, scrollSensitivity = 100f;
    [Space]
    
    [Header("UI")]
    [SerializeField] private GameObject throwDartPanel;
    [SerializeField] private Slider sliderScore;
    [SerializeField] private bool isSliderReachTop = false;
    [SerializeField] private float sliderSpeed, minsliderSpeed = 3f, maxSliderSpeed = 6f; 
    [Space]

    [Header("Throw Score")]
    [SerializeField] private int[] throwScores;
    [SerializeField] private int minThrowScores = -100, maxThrowScore = 100; 
    [SerializeField] private TextMeshProUGUI[] textThrowScores; 
    [Space]
    
    public int dartForceAdded, maxDartForceAdd = 3;
    
    [Space]
    
    [Header("Interaction")]
    [SerializeField] private float collectCooldown = 0.2f;
    private bool canCollect = true;
    
    private float runSpeed;
    private Vector3 velocity;
    private Dart mainhandDart;
    
    public static PlayerController instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        throwDartPanel.SetActive(false);
    }

    void Update()
    {
        UpdateHandRotation();
        MainHandHandle();
        MovementHandle();
        GravityAndJumpHandle();
        
        if(MouseLook.instance.lastLookedItem == null) return;
        if (Input.GetKeyDown(MouseLook.instance.lastLookedItem.collectKey) && canCollect && MouseLook.instance != null)
        {
            CollectItem(MouseLook.instance.lastLookedItem.thisItemData);
            StartCoroutine(CollectCooldownRoutine());
        }
    }

    #region Mainhand
    void MainHandHandle()
    {
        if (mainHandPrefab == null || mainHandItem == null)
        {
            throwDartPanel.SetActive(false);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q)) DropItem();

        HandleDart();
    }

    public void GetItem(ItemData _newItem)
    {
        if (mainHandPrefab != null) DropItem();
        mainHandItem = _newItem;

        mainHandPrefab = Instantiate(mainHandItem.itemPrefab, handParent.transform.position,
            handParent.transform.rotation, handParent);

        AudioManager.instance.PlaySFX(2);
        InitDart();
    }

    void CollectItem(ItemData _itemData) // Separate method for handling the actual collection
    {
        GetItem(_itemData);
        // Potentially destroy the item in the world here, or handle its removal
        if (MouseLook.instance != null && MouseLook.instance.lastLookedItem != null)
        {
            Destroy(MouseLook.instance.lastLookedItem.gameObject); // Assuming ItemPrefab has the GameObject reference
            MouseLook.instance.lastLookedItem = null; // Clear the looked-at item
        }
    }

    void DropItem()
    {
        if (mainHandPrefab == null || mainHandItem == null) return;
        
        ItemPrefab dropItem = Instantiate(itemPrefab, this.transform.position, quaternion.identity);
        dropItem.Init(mainHandItem);
        dropItem.transform.position = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);
        
        Destroy(mainHandPrefab);
        mainHandItem = null;
    }
    #endregion

    #region Dart
    private void InitDart()
    {
        mainhandDart = mainHandPrefab.GetComponent<Dart>();
        if (mainhandDart == null)
        {
            throwDartPanel.SetActive(false);
            return;
        }

        sliderSpeed = Random.Range(minsliderSpeed, maxSliderSpeed);
        dartForceAdded = 0;
        throwForce = 0;

        InitThrowScore();
    }

    void InitThrowScore()
    {
        throwScores = new[] { 0, 0, 0, 0 };
        
        for (int i = 0; i < throwScores.Length; i++)
            throwScores[i] = Random.Range(minThrowScores, maxThrowScore);

        for (int i = 0; i < textThrowScores.Length; i++)
        {
            textThrowScores[i].text = throwScores[i] > 0 ? "+" : "";
            textThrowScores[i].text += $"{throwScores[i]}";
        }
    }

    private IEnumerator DartCoroutine()
    {
        yield return new WaitForSeconds(1f);
    }
    private void HandleDart()
    {
        if (mainhandDart == null) return;
     
        throwDartPanel.SetActive(true);
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        throwForce += scroll * scrollSensitivity;
        throwForce = Mathf.Clamp(throwForce, minThrowForce, maxThrowForce);

        if (sliderScore.value >= sliderScore.maxValue) isSliderReachTop = true; else if (sliderScore.value <= 0) isSliderReachTop = false;
        if (isSliderReachTop) sliderScore.value -= Time.deltaTime * sliderSpeed; else sliderScore.value += Time.deltaTime * sliderSpeed;
        
        if (Input.GetButtonDown("Fire1"))
        {
            dartForceAdded++;

            switch (sliderScore.value)
            {
                case >= 0.75f:
                    throwForce += throwScores[3];
                    break;
                case >= 0.50f:
                    throwForce += throwScores[2];
                    break;
                case >= 0.25f:
                    throwForce += throwScores[1];
                    break;
                default:
                    throwForce += throwScores[0];
                    break;
            }
            
            InitThrowScore();
            
            if (dartForceAdded >= maxDartForceAdd) ThrowDart();
        }
    }

    private void ThrowDart()
    {
        Dart projectileItem = mainHandPrefab.GetComponent<Dart>();
        if (projectileItem != null)
        {
            projectileItem.ThrowDart(playerCamera.forward, throwForce);
            MinigameBalloon.instance.AddDart();
            mainHandPrefab = null;
            
            AudioManager.instance.PlaySFX(8);
        }
    }

    #endregion

    #region movement
    void UpdateHandRotation()
    {
        if (handParent != null && playerCamera != null) handParent.rotation = Quaternion.LookRotation(playerCamera.forward, playerCamera.up);
    }
    void MovementHandle()
    {
        runSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeedMultiplier : 1f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * (moveSpeed * runSpeed) * Time.deltaTime);
    }
    void GravityAndJumpHandle()
    {
        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f;
            
            if (Input.GetButtonDown("Jump"))
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    #endregion
    
    private IEnumerator CollectCooldownRoutine()
    {
        canCollect = false;
        yield return new WaitForSeconds(collectCooldown);
        canCollect = true;
    }
}