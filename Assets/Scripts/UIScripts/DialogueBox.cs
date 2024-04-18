using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public abstract class DialogueBox : MonoBehaviour
{

    public TMP_Text dialogueText;
    public string[] lines;
    [SerializeField] private float textSpeed;
    protected int index;
    protected virtual void Start()
    {
        dialogueText.text = string.Empty;
        
    }

    protected virtual void OnEnable()
    {
        dialogueText.text = string.Empty;
        StartDialogue();
    }


    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(dialogueText.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
            }
        }
    }

    protected virtual void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    protected virtual IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    protected virtual void NextLine()
    {
        if(index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
