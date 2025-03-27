using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemPrefab : MonoBehaviour
{
    public ItemData thisItemData;
    public KeyCode collectKey;
    [SerializeField] private GameObject collectPanel, highlightObject;
    [SerializeField] private TextMeshProUGUI collectText;
    [SerializeField] private Transform prefabParent;

    public bool canCollect;
    protected virtual void Start()
    {
        //Init(thisItemData);
    }

    public void Init(ItemData _itemData)
    {
        thisItemData = _itemData;
        
        ResetGame();
        
        GameObject item = Instantiate(_itemData.itemPrefab, prefabParent.position, Quaternion.identity, prefabParent);
        item.AddComponent<Outline>();

    }
    
    protected virtual void Update()
    {
        if (canCollect)
        {
            collectPanel.SetActive(true);
            if (Input.GetKey(collectKey)) CollectItem();
        }
        else collectPanel.SetActive(false);
        
    }

    protected virtual void ResetGame()
    {
        canCollect = false;
        collectText.text = collectKey + "";
    }

    public void CanCollect(bool _canCollect)
    {
        canCollect = _canCollect;
        highlightObject.SetActive(_canCollect);
    }

    protected virtual void CollectItem()
    {
        canCollect = false;
        
        PlayerController.instance.GetItem(thisItemData);
        Debug.Log($"Collected Items {thisItemData.itemName}");

        Destroy(this.gameObject);
    }

}
