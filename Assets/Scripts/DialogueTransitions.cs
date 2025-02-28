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
        public static event Action SpriteFadeInFinish;
        public static event Action SpriteFadeOutFinish;
        
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
        [SerializeField] private TextMeshProUGUI dialogueHeaderText;
        [SerializeField] private Image primarySprite;

        public bool ReadyToProceed = false;
        private Color opaquePanelColour;
        private Color transparentPanelColour;
        private Color opaqueTestSpriteColour;
        private Color transparentTestSpriteColour;
        private Color opaqueTextColour;
        private Color transparentTextColour;
        private Color opaqueHeaderPanelColour;
        private Color transparentHeaderPanelColour;
        
        // =========== Private object reference variables ===========
        private Image dialoguePanelImage;
        private Image dialogueHeaderPanelImage;

        private void Start()
        {
            dialoguePanelImage = dialoguePanel.gameObject.GetComponent<Image>();
            dialogueHeaderPanelImage = dialogueHeaderPanel.gameObject.GetComponent<Image>();
            opaquePanelColour = dialoguePanelImage.color;
            transparentPanelColour = new Color(dialoguePanelImage.color.r, dialoguePanelImage.color.g,
                dialoguePanelImage.color.b, 0);
            opaqueTestSpriteColour = primarySprite.color;
            transparentTestSpriteColour = new Color(primarySprite.color.r, primarySprite.color.g, primarySprite.color.b, 0);
            opaqueTextColour = dialogueHeaderText.color;
            transparentTextColour =
                new Color(dialogueHeaderText.color.r, dialogueHeaderText.color.g, dialogueHeaderText.color.b, 0);
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

        public void ChangeCharacter(Sprite newSprite, Color newColour)
        {
            StartCoroutine(ChangeCharacterTransition(newSprite, newColour));
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
            var spriteTransform = primarySprite.GetComponent<RectTransform>();

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
            primarySprite.color = transparentTestSpriteColour;
            dialogueHeaderPanelImage.color = transparentHeaderPanelColour;
            dialogueHeaderText.color = transparentTextColour;

            // The delay between the panel transition and the sprite transition
            yield return new WaitForSeconds(npcSpriteInDelay);

            while (timeElapsed < endTime)
            {
                if (timeElapsed < spriteFadeInTime)
                {
                    primarySprite.color = Color.Lerp(transparentTestSpriteColour, opaqueTestSpriteColour, timeElapsed / spriteFadeInTime);
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
            primarySprite.color = opaqueTestSpriteColour;

            ReadyToProceed = true;
            SpriteFadeInFinish?.Invoke();
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
                primarySprite.color = Color.Lerp(opaqueTestSpriteColour, transparentTestSpriteColour,
                    timeElapsed / panelFadeOutTime);
                dialogueHeaderPanelImage.color = Color.Lerp(opaqueHeaderPanelColour, transparentHeaderPanelColour,
                    timeElapsed / panelFadeOutTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            primarySprite.color = transparentTestSpriteColour;
            dialoguePanel.gameObject.SetActive(false);
            InteractionFieldReactivate?.Invoke(); // Send a message to the interaction field to let it know it can re-activate
            SpriteFadeOutFinish?.Invoke();
            Debug.Log("Sprite transition out is done.");
        }

        // TODO: When the time is available, make this possible with the BeginSpriteTransitionIn and BeginSpriteTransitionOut coroutines themselves.
        private IEnumerator ChangeCharacterTransition(Sprite newSprite, Color newColour)
        {
            // General initialisation of variables
            dialogueSystem.GoodToGo = false;
            float timeElapsed = 0;
            float endTime = spriteSlideInCurve.keys[^1].time;

            // Initialising the colour fade in stuff
            Color oldPanelColour = dialoguePanelImage.color;
            Color oldPanelHeaderColour = dialogueHeaderPanelImage.color;

            // Initialising the slide in stuff
            RectTransform spriteTransform = primarySprite.GetComponent<RectTransform>();
            Vector3 oldSpritePos = new Vector3(spriteTransform.localPosition.x + spriteSlideInAmount,
                spriteTransform.localPosition.y, spriteTransform.localPosition.z);
            Vector3 newSpritePos = spriteTransform.localPosition;

            while (timeElapsed < spriteFadeOutTime)
            {
                primarySprite.color = Color.Lerp(opaqueTestSpriteColour, transparentTestSpriteColour, timeElapsed / spriteFadeOutTime);
                Debug.Log($"Fade out timeElapsed: {timeElapsed}");
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            primarySprite.color = transparentTestSpriteColour;
            primarySprite.sprite = newSprite;
            spriteTransform.localPosition = oldSpritePos;
            timeElapsed = 0;
            
            while (timeElapsed < endTime)
            {
                spriteTransform.localPosition =
                    Vector3.Lerp(oldSpritePos, newSpritePos, spriteSlideInCurve.Evaluate(timeElapsed));
                dialoguePanelImage.color = Color.Lerp(oldPanelColour, newColour, timeElapsed / spriteFadeInTime);
                dialogueHeaderPanelImage.color = Color.Lerp(oldPanelHeaderColour, newColour, timeElapsed / spriteFadeInTime);
                Debug.Log($"Slide in timeElapsed: {timeElapsed}");
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            primarySprite.color = opaqueTestSpriteColour;
            dialoguePanelImage.color = newColour;
            dialogueHeaderPanelImage.color = newColour;
            spriteTransform.localPosition = newSpritePos;

            dialogueSystem.GoodToGo = true;
        }
    }
}