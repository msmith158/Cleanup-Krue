using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueHeader;
    [SerializeField] private TextMeshProUGUI dialogueText;

    public void PrintDialogue(TextAsset dialogueBundle)
    {
        Debug.Log("Called PrintDialogue");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
