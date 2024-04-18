using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_manager : Singleton<UI_manager>
{
    public TMP_Text dayText;
    public TMP_Text moneyText;
    public TMP_Text time;
    public TMP_Text bucketText;

    public int totalMoney;
    public PlayerScriptableObject playerScriptable;
    [SerializeField] private BucketScript bucket;

    // Start is called before the first frame update
    void Start()
    {

        totalMoney = playerScriptable.money;
    }

    // Update is called once per frame
    void Update()
    {
       moneyText.text = string.Format("Money: {0}", totalMoney);
        bucketText.text = string.Format("Bucket: {0}", bucket.currentFishAmount);
    }
}
