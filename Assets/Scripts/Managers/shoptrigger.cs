
using UnityEngine;
using System.Collections;
public class shoptrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))    
                {
                    ShopManager.instance.ToggleShop();
                    Debug.Log("player in");
                }
        }
    private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))    
                {
                    ShopManager.instance.ToggleShop();
                    Debug.Log("player out");
                }
        }
    // Update is called once per frame
    void Update()
    {
        
    }
}
