
using UnityEngine;
using UnityEngine.UI;
public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    
    public int money = 300;
    
    public UpgradeScriptableObj[] upgrades;
    public Transform shopContent;
    public GameObject itemPrefab;
    
    //reference
    public Text moneytext;
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
            if (money >= upgrade.cost)
                {
                    money -= upgrade.cost;
                    upgrade.quantity++;
                    upgrade.itemRef.transform.GetChild(0).GetComponent<Text>().
                    text = "Quantity: " + upgrade.quantity.ToString();

                    
                }

        }
     
    public void ToggleShop()    
            {
                ShopUI.SetActive(!ShopUI.activeSelf);
            }
    private void OnGUI()
        {
            moneytext.text = "Money: " + money.ToString();
        }
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