using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mitchel.UISystems;
using UnityEngine;

public class DialogueUtils : MonoBehaviour
{
    [Header("Dialogue Box Colours")]
    [SerializeField] private Color quinnColour;
    [SerializeField] private Color quinnHeaderColour;
    [Space(5)]
    [SerializeField] private Color caspianColour;
    [SerializeField] private Color CaspianHeaderColour;
    
    [Header("Character Sprites")]
    [Tooltip("Insert in the following order: Neutral, Happy.")] [SerializeField] private Sprite[] quinnSprites;
    [Tooltip("Insert in the following order: Neutral, Happy.")] [SerializeField] private Sprite[] caspianSprites;

    // =============== Private value variables ===============
    [HideInInspector] public enum SelectedCharacter { Quinn, Caspian }
    public SelectedCharacter SelectedCharacterEnum { get; private set; } = SelectedCharacter.Quinn;
    private Dictionary<(SelectedCharacter, string), Sprite> expressionSprites;
    private List<string> savedTag = new List<string>();
    private string selectedEmotion = "Neutral";
    private bool characterSwitchReady = false;

    // =========== Private object reference variables ===========
    private DialogueSystem dialogueSys;
    private DialogueTransitions dialogueTransitions;

    // Declaring the dictionary of different expressions for each character
    private void Start()
    {
        dialogueSys = GetComponent<DialogueSystem>();
        dialogueTransitions = GetComponent<DialogueTransitions>();

        expressionSprites = new Dictionary<(SelectedCharacter, string), Sprite>
        {
            {(SelectedCharacter.Quinn, "Neutral"), quinnSprites[0]},
            {(SelectedCharacter.Quinn, "Happy"), quinnSprites[1]},
            {(SelectedCharacter.Caspian, "Neutral"), caspianSprites[0]},
            {(SelectedCharacter.Caspian, "Happy"), caspianSprites[1]}
        };
    }

    private void Awake()
    {
        DialogueTransitions.SpriteFadeInFinish += ReadyCharacterSwitch;
        DialogueTransitions.SpriteFadeOutFinish += UnreadyCharacterSwitch;
    }

    private void OnDisable()
    {
        DialogueTransitions.SpriteFadeInFinish -= ReadyCharacterSwitch;
        DialogueTransitions.SpriteFadeOutFinish -= UnreadyCharacterSwitch;
    }

    /// <summary>
    /// Process in-line arguments presented in square brackets to give special instructions
    /// to the dialogue system (e.g. to change character, character expression or text effect).
    /// </summary>
    /// <param name="tag">The in-line argument captured by the dialogue system, enclosed in square brackets.</param>
    public void ProcessInlineArgument(string tag)
    {
        if (savedTag != null) savedTag.Clear();

        savedTag = tag.Split(new[] { '[', '=', ']' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
        for (int i = 0; i < savedTag.Count; i+=2)
        {
            switch (savedTag[i])
            {
                case "character":
                    ChangeCharacter(savedTag[++i]);
                    break;
                case "sprite":
                    ChangeExpression(savedTag[++i]);
                    break;
                case "texteffect":
                    ChangeTextEffect(savedTag[++i]);
                    break;
                case "paneleffect":
                    ChangePanelEffect(savedTag[++i]);
                    break;
            }
        }
    }

    private void ChangeCharacter(string input)
    {
        switch (input)
        {
            case "Quinn":
                SelectedCharacterEnum = SelectedCharacter.Quinn;
                dialogueSys.dialogueHeader.text = "Quinn";
                if (characterSwitchReady && expressionSprites.TryGetValue((SelectedCharacterEnum, selectedEmotion), out Sprite quinnSprite))
                {
                    dialogueTransitions.QueuedSprite = quinnSprite;
                    dialogueTransitions.CurrentColour = quinnColour;
                    dialogueTransitions.CurrentHeaderColour = quinnHeaderColour;
                    dialogueTransitions.ChangeCharacter();
                }
                Debug.Log("Chosen character is Quinn");
                break;
            case "Caspian":
                SelectedCharacterEnum = SelectedCharacter.Caspian;
                dialogueSys.dialogueHeader.text = "Caspian";
                if (characterSwitchReady && expressionSprites.TryGetValue((SelectedCharacterEnum, selectedEmotion), out Sprite caspianSprite))
                {
                    dialogueTransitions.QueuedSprite = caspianSprite;
                    dialogueTransitions.CurrentColour = caspianColour;
                    dialogueTransitions.ChangeCharacter();
                }
                Debug.Log("Chosen character is Caspian");
                break;
        }
    }

    private void ChangeExpression(string input)
    {
        // This will try and get whatever sprite is applicable depending on the character and expression detected
        if (expressionSprites.TryGetValue((SelectedCharacterEnum, input), out Sprite sprite))
        {
            if (!dialogueTransitions.ReadyToProceed)
            {
                dialogueTransitions.QueuedSprite = sprite;
            }
            else dialogueSys.dialogueCharacterImage.sprite = sprite;
            selectedEmotion = input;
            Debug.Log($"Character {SelectedCharacterEnum} is {input}");
        }
        else
        {
            Debug.LogWarning($"No sprite found for {SelectedCharacterEnum} with expression {input}");
        }
    }

    private void ChangeTextEffect(string input)
    {
        Debug.Log(input);
    }

    private void ChangePanelEffect(string input)
    {
        Debug.Log(input);
    }

    private void ReadyCharacterSwitch()
    {
        characterSwitchReady = true;
    }

    private void UnreadyCharacterSwitch()
    {
        characterSwitchReady = false;
    }
}