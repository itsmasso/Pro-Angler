using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_LineCount : MonoBehaviour
{
     public int max_line_count;
        public int current_line_count;

        public Image[] fishinglines; //SIZE OF image array
                                        //may be changed as a progression mechanic
        
        public Sprite fishing_line;  //sprite for available fish count
        public Sprite fishing_line_dark; //sprite for missing fish count
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (current_line_count > max_line_count) // in case max amount  is greater than current amount
            {
                current_line_count = max_line_count;
            }
            
        for (int i = 0; i < fishinglines.Length; i++)
            {
                if (i < max_line_count)
                    {
                        fishinglines[i].sprite = fishing_line; 
                    }
                else    
                    {
                        fishinglines[i].sprite = fishing_line_dark;
                    }
                if (i < current_line_count)
                    {
                        fishinglines[i].enabled = true;
                    }
                else    
                    {
                        fishinglines[i].enabled = false;
                    }
            }
        //cheats/ debugging
            if (Input.GetKeyDown("8"))
                {
                    current_line_count++;
                }
             if (Input.GetKeyDown("9"))
                {
                    current_line_count--;
                }
    }
    }

