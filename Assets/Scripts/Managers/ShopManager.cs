
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    
    //public int totalMoney;
    
    public UpgradeScriptableObj[] upgrades;
    public Transform shopContent;
    public GameObject itemPrefab;
    [SerializeField] private UI_manager uiManager;
    //reference
    //public TMP_Text moneytext;
    public GameObject ShopUI;
    [SerializeField] private GameObject ShopBackground;

        private void Awake()
            {
                if (instance == null)
                    {
                        instance = this;
                    }
                else 
                    {
                        Destroy(gameObject);
                    }
                DontDestroyOnLoad(gameObject);
            }
        
       
    
      // Start is called before the first frame update
    private void Start()
    {
        foreach (UpgradeScriptableObj upgrade in upgrades)
            {
                GameObject item = Instantiate(itemPrefab, shopContent);
                upgrade.itemRef = item;
                
                foreach (Transform child in item.transform)
                    {
                        if (child.gameObject.name == "Quantity" )
                            {
                                child.gameObject.GetComponent<Text>().text = "Quantity " + upgrade.quantity.ToString();

                            }
                        else if(child.gameObject.name == "Cost")
                            {
                                child.gameObject.GetComponent<Text>().text = "$" + upgrade.cost.ToString();

                            }
                        else if(child.gameObject.name == "Name")
                            {
                                child.gameObject.GetComponent<Text>().text =upgrade.upgradeName.ToString();

                            }
                        else if (child.gameObject.name == "Image")
                            {
                                child.gameObject.GetComponent<Image>().sprite = upgrade.upgradeIcon; 

                            }
                    }   
                    item.GetComponent<Button>().onClick.AddListener(() => {
                        BuyUpgrade(upgrade);
                    });
                    
            }
                   
    }
    public void BuyUpgrade(UpgradeScriptableObj upgrade)
        {
            if ( uiManager.totalMoney >= upgrade.cost)
                {
                    uiManager.totalMoney -= upgrade.cost;
                    upgrade.quantity++;
                    upgrade.itemRef.transform.GetChild(0).GetComponent<Text>().
                    text = "Quantity: " + upgrade.quantity.ToString();

                    
                }

        }
     
    public void ToggleShop()    
            {
              
                ShopUI.SetActive(!ShopUI.activeSelf);
                uiManager.isTimerRunning = true;
                
            }
    /*private void OnGUI()
        {
             uiManager.totalMoney = "Money: " + uiManager.totalMoney.ToString();
        }*/
}

/*
[System.Serializable]
    public class Upgrade 
        {
            public string name;
            public int cost;
            public Sprite image;
            public int quantity;
            [HideInInspector] public GameObject itemRef;
        }  
        */