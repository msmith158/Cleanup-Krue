using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUtils : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private Image characterImage;
    [Space(5)]
    [Tooltip("Insert in the following order: Neutral, Happy.")] [SerializeField] private Sprite[] quinnSprites;
    [Tooltip("Insert in the following order: Neutral, Happy.")] [SerializeField] private Sprite[] caspianSprites;

    // =============== Private value variables ===============
    private enum SelectedCharacter { Quinn, Caspian }
    private SelectedCharacter selectedCharacter = SelectedCharacter.Quinn;
    private Dictionary<(SelectedCharacter, string), Sprite> expressionSprites;
    private List<string> savedTag = new List<string>();
    private int charIndex = 0;

    // Declaring the dictionary of different expressions for each character
    private void Start()
    {
        expressionSprites = new Dictionary<(SelectedCharacter, string), Sprite>
        {
            {(SelectedCharacter.Quinn, "Neutral"), quinnSprites[0]},
            {(SelectedCharacter.Quinn, "Happy"), quinnSprites[1]},
            {(SelectedCharacter.Caspian, "Neutral"), caspianSprites[0]},
            {(SelectedCharacter.Caspian, "Happy"), caspianSprites[1]}
        };
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
            }
        }
    }

    private void ChangeCharacter(string input)
    {
        switch (input)
        {
            case "Quinn":
                selectedCharacter = SelectedCharacter.Quinn;
                Debug.Log("Chosen character is Quinn");
                break;
            case "Caspian":
                selectedCharacter = SelectedCharacter.Caspian;
                Debug.Log("Chosen character is Caspian");
                break;
        }
    }

    // TODO: Find a better method for this. Maybe generic types?
    private void ChangeExpression(string input)
    {
        /*switch (input)
        {
            case "Neutral":
                switch (selectedCharacter)
                {
                    case SelectedCharacter.Quinn:
                        characterImage.sprite = quinnSprites[0];
                        Debug.Log("Character Quinn is Neutral");
                        break;
                    case SelectedCharacter.Caspian:
                        characterImage.sprite = caspianSprites[0];
                        Debug.Log("Character Caspian is Neutral");
                        break;
                }
                break;
            case "Happy":
                switch (selectedCharacter)
                {
                    case SelectedCharacter.Quinn:
                        characterImage.sprite = quinnSprites[1];
                        Debug.Log("Character Quinn is Happy");
                        break;
                    case SelectedCharacter.Caspian:
                        characterImage.sprite = caspianSprites[1];
                        Debug.Log("Character Caspian is Happy");
                        break;
                }
                break;
        }*/

        if (expressionSprites.TryGetValue((selectedCharacter, input), out Sprite sprite))
        {
            characterImage.sprite = sprite;
            Debug.Log($"Character {selectedCharacter} is {input}");
        }
        else
        {
            Debug.LogWarning($"No sprite found for {selectedCharacter} with expression {input}");
        }
    }
}
