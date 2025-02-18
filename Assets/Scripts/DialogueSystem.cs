using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private List<DialogueData> dialogueData;
    
    [System.Serializable]
    public class DialogueData
    {
        [Tooltip("Use a label to keep track of dialogue entries in the inspector.")] public string Label;
        [TextArea]
        [Tooltip("Import the text file to print the dialogue.")] public TextAsset dialogueLine;
        [Space(10)]
        [Tooltip("This is for the timing of each dialogue line. Example: for fast, set 0.02f; for slow, set 0.2f.\n\nMake sure this is in sequence with the order of the dialogue lines. If list is empty, system will use default timing.")] public float charDelayTime;
        [Tooltip("This is for what will play when each character of a dialogue line is being printed out.\n\nMake sure this is in sequence with the order of the dialogue lines.")] public AudioClip dialogueLineSfx;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
