using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuestManager : MonoBehaviour
{
     public static QuestManager instance;
     public GameObject QuestUI;
     public QFishScriptableObj[] SpecialFish;
     public GameObject FishPrefab;
     public Transform FishContent;
    

    // Start is called before the first frame update
    void Start()
    {
        foreach (QFishScriptableObj Sfish in SpecialFish)
            {
                    GameObject item = Instantiate(FishPrefab, FishContent);
                    Sfish.itemref = item;
                    
                    foreach (Transform child in item.transform)
                        {
                            if (child.gameObject.name == "Name" )
                                {
                                    child.gameObject.GetComponent<Text>().text = Sfish.FishName.ToString();

                                }
                            else if(child.gameObject.name == "Profit")
                                {
                                    child.gameObject.GetComponent<Text>().text = "$" + Sfish.bonus.ToString();

                                }
                    
                            else if (child.gameObject.name == "Image")
                                {
                                    child.gameObject.GetComponent<Image>().sprite = Sfish.FishIcon; 

                                }
            }
    }
            }
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

    public void ToggleQuest()    
            {
                QuestUI.SetActive(!QuestUI.activeSelf);
            }
    // Update is called once per frame
    void Update()
    {
        
    }
}
