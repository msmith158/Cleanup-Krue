using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mitchel.UISystems
{
    public class DialogueTransitions : MonoBehaviour
    {
        public static event Action InteractionFieldReactivate;
        
        [Header("Transition In Effect Settings")]
        [SerializeField] private AnimationCurve panelSlideInCurve;
        [SerializeField] private float panelSlideInAmount;
        [SerializeField] private float panelFadeInTime;
        [Space(5)]
        [SerializeField] private AnimationCurve spriteSlideInCurve;
        [SerializeField] private float npcSpriteInDelay;
        [SerializeField] private float spriteSlideInAmount;
        [SerializeField] private float spriteFadeInTime;
        [Space(5)] 
        [SerializeField] private AnimationCurve headerPanelSlideInCurve;
        [SerializeField] private float headerPanelSlideInAmount;

        [Header("Transition Out Effect Settings")] 
        [SerializeField] private float panelFadeOutTime;
        [Space(5)]
        [SerializeField] private float npcSpriteOutDelay;
        [SerializeField] private float spriteFadeOutTime;

        [Header("Object References")] 
        [SerializeField] private DialogueSystem dialogueSystem;
        [SerializeField] private RectTransform dialoguePanel;
        [SerializeField] private RectTransform dialogueHeaderPanel;
        [SerializeField] private TextMeshProUGUI dialogueHeader;
        [SerializeField] private TextMeshProUGUI dialogueHeaderText;
        [SerializeField] private Image testSprite;

        public bool ReadyToProceed = false;
        private Color opaquePanelColour;
        private Color transparentPanelColour;
        private Color opaqueTestSpriteColour;
        private Color transparentTestSpriteColour;
        private Color opaqueTextColour;
        private Color transparentTextColour;
        private Color opaqueHeaderPanelColour;
        private Color transparentHeaderPanelColour;
        private Image dialoguePanelImage;
        private Image dialogueHeaderPanelImage;

        private void Start()
        {
            dialoguePanelImage = dialoguePanel.gameObject.GetComponent<Image>();
            dialogueHeaderPanelImage = dialogueHeaderPanel.gameObject.GetComponent<Image>();
            opaquePanelColour = dialoguePanelImage.color;
            transparentPanelColour = new Color(dialoguePanelImage.color.r, dialoguePanelImage.color.g,
                dialoguePanelImage.color.b, 0);
            opaqueTestSpriteColour = testSprite.color;
            transparentTestSpriteColour = new Color(testSprite.color.r, testSprite.color.g, testSprite.color.b, 0);
            opaqueTextColour = dialogueHeader.color;
            transparentTextColour =
                new Color(dialogueHeader.color.r, dialogueHeader.color.g, dialogueHeader.color.b, 0);
            opaqueHeaderPanelColour = dialogueHeaderPanelImage.color;
            transparentHeaderPanelColour = new Color(dialogueHeaderPanelImage.color.r, dialogueHeaderPanelImage.color.g,
                dialogueHeaderPanelImage.color.b, 0);
        }
        
        public void EnterDialogue()
        {
            dialoguePanel.gameObject.SetActive(true);
            StartCoroutine(BeginPanelTransitionIn());
            StartCoroutine(BeginCharacterTransitionIn());
        }

        public void ExitDialogue()
        {
            if (ReadyToProceed)
            {
                StartCoroutine(BeginPanelTransitionOut());
                StartCoroutine(BeginCharacterTransitionOut());
            }
        }

        private IEnumerator BeginPanelTransitionIn()
        {
            ReadyToProceed = false;
            float timeElapsed = 0;
            float endTime = panelSlideInCurve.keys[^1].time;

            // Initialise all the fade in stuff
            dialoguePanelImage.color = transparentPanelColour;

            // Initialise all the slide in stuff
            Vector3 oldPanelPos = new Vector3(dialoguePanel.position.x + panelSlideInAmount,
                dialoguePanel.position.y, dialoguePanel.position.z);
            Vector3 newPanelPos = dialoguePanel.position;
            dialoguePanel.position = oldPanelPos;
            
            while (timeElapsed < endTime)
            {
                // Fade effect
                if (timeElapsed < panelFadeInTime)
                {
                    dialoguePanelImage.color = Color.Lerp(transparentPanelColour, opaquePanelColour, timeElapsed / panelFadeInTime);
                }
                
                // Slide in effect
                dialoguePanel.position =
                    Vector3.Lerp(oldPanelPos, newPanelPos, panelSlideInCurve.Evaluate(timeElapsed));
                
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            dialoguePanelImage.color = opaquePanelColour;
            dialoguePanel.position = newPanelPos;
            dialogueSystem.PrintDialogue();
            Debug.Log("Panel transition in is finished.");
        }

        private IEnumerator BeginCharacterTransitionIn()
        {
            // General initialisation of variables
            float timeElapsed = 0;
            float endTime = spriteSlideInCurve.keys[^1].time;
            RectTransform spriteTransform = testSprite.GetComponent<RectTransform>();

            // Initialising all the slide in stuff
            Vector3 oldSpritePos = new Vector3(spriteTransform.localPosition.x + spriteSlideInAmount,
                spriteTransform.localPosition.y, spriteTransform.localPosition.z);
            Vector3 newSpritePos = spriteTransform.localPosition;
            Vector3 oldHeaderPanelPos = new Vector3(dialogueHeaderPanel.localPosition.x + headerPanelSlideInAmount,
                dialogueHeaderPanel.localPosition.y, dialogueHeaderPanel.localPosition.z);
            Vector3 newHeaderPanelPos = dialogueHeaderPanel.localPosition;
            spriteTransform.localPosition = oldSpritePos;
            dialogueHeaderPanel.localPosition = oldHeaderPanelPos;
            
            // Initialising all the fade in stuff
            testSprite.color = transparentTestSpriteColour;
            dialogueHeaderPanelImage.color = transparentHeaderPanelColour;
            dialogueHeaderText.color = transparentTextColour;

            // The delay between the panel transition and the sprite transition
            yield return new WaitForSeconds(npcSpriteInDelay);

            while (timeElapsed < endTime)
            {
                if (timeElapsed < spriteFadeInTime)
                {
                    testSprite.color = Color.Lerp(transparentTestSpriteColour, opaqueTestSpriteColour, timeElapsed / spriteFadeInTime);
                    dialogueHeaderPanelImage.color = Color.Lerp(transparentPanelColour, opaquePanelColour,
                        timeElapsed / spriteFadeInTime);
                    dialogueHeaderText.color = Color.Lerp(transparentTextColour, opaqueTextColour,
                        timeElapsed / spriteFadeInTime);
                }

                spriteTransform.localPosition =
                    Vector3.Lerp(oldSpritePos, newSpritePos, spriteSlideInCurve.Evaluate(timeElapsed));
                dialogueHeaderPanel.localPosition = Vector3.Lerp(oldHeaderPanelPos, newHeaderPanelPos,
                    headerPanelSlideInCurve.Evaluate(timeElapsed));
                
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            spriteTransform.localPosition = newSpritePos;
            testSprite.color = opaqueTestSpriteColour;

            ReadyToProceed = true;

            Debug.Log("Sprite transition in is done.");
        }

        private IEnumerator BeginPanelTransitionOut()
        {
            Debug.Log("Begin panel transition out");
            float timeElapsed = 0;

            while (timeElapsed < panelFadeOutTime)
            {
                dialoguePanelImage.color = Color.Lerp(opaquePanelColour, transparentPanelColour,
                    timeElapsed / panelFadeOutTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            
            dialoguePanelImage.color = transparentPanelColour;
            Debug.Log("Panel transition out is done.");
        }

        private IEnumerator BeginCharacterTransitionOut()
        {
            Debug.Log("Begin sprite transition out");
            float timeElapsed = 0;

            yield return new WaitForSeconds(npcSpriteOutDelay);
            
            while (timeElapsed < spriteFadeOutTime)
            {
                testSprite.color = Color.Lerp(opaqueTestSpriteColour, transparentTestSpriteColour,
                    timeElapsed / panelFadeOutTime);
                dialogueHeaderPanelImage.color = Color.Lerp(opaqueHeaderPanelColour, transparentHeaderPanelColour,
                    timeElapsed / panelFadeOutTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            testSprite.color = transparentTestSpriteColour;
            dialoguePanel.gameObject.SetActive(false);
            InteractionFieldReactivate?.Invoke(); // Send a message to the interaction field to let it know it can re-activate
            Debug.Log("Sprite transition out is done.");
        }
    }
}