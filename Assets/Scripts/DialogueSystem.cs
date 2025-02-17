using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mitchel.UISystems
{
    public class DialogueSystem : MonoBehaviour
    {
        [Header("Transition In Effect Settings")]
        [SerializeField] private AnimationCurve panelSlideInCurve;
        [SerializeField] private float panelSlideInAmount;
        [SerializeField] private float panelFadeInTime;
        [SerializeField] private AnimationCurve spriteSlideInCurve;
        [SerializeField] private float npcSpriteInDelay;
        [SerializeField] private float spriteSlideInAmount;
        [SerializeField] private float spriteFadeInTime;

        [Header("Transition Out Effect Settings")] 
        [SerializeField] private float panelFadeOutTime;
        [SerializeField] private float npcSpriteOutDelay;
        [SerializeField] private float spriteFadeOutTime;
        
        [Header("Object References")]
        [SerializeField] private RectTransform dialoguePanel;
        [SerializeField] private TextMeshProUGUI dialogueHeader;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private AudioClip defaultDialogueSfx;
        [SerializeField] private Image testSprite;

        private bool readyToProceed = false;
        private Color opaquePanelColour;
        private Color transparentPanelColour;
        private Color opaqueTestSpriteColour;
        private Color transparentTestSpriteColour;
        private Image dialoguePanelImage;

        private void Start()
        {
            dialoguePanelImage = dialoguePanel.gameObject.GetComponent<Image>();
            opaquePanelColour = dialoguePanelImage.color;
            transparentPanelColour = new Color(dialoguePanelImage.color.r, dialoguePanelImage.color.g,
                dialoguePanelImage.color.b, 0);
            opaqueTestSpriteColour = testSprite.color;
            transparentTestSpriteColour = new Color(testSprite.color.r, testSprite.color.g, testSprite.color.b, 0);
        }
        
        public void InitiateDialogue()
        {
            dialoguePanel.gameObject.SetActive(true);
            StartCoroutine(BeginPanelTransitionIn());
            StartCoroutine(BeginSpriteTransitionIn());
        }

        public void ExitDialogue()
        {
            if (readyToProceed)
            {
                StartCoroutine(BeginPanelTransitionOut());
                StartCoroutine(BeginSpriteTransitionOut());
            }
        }

        private IEnumerator BeginPanelTransitionIn()
        {
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
            
            Debug.Log("Panel transition in is finished.");
        }

        private IEnumerator BeginSpriteTransitionIn()
        {
            // General initialisation of variables
            float timeElapsed = 0;
            float endTime = spriteSlideInCurve.keys[^1].time;
            RectTransform spriteTransform = testSprite.GetComponent<RectTransform>();

            // Initialising all the slide in stuff
            Vector3 oldSpritePos = new Vector3(spriteTransform.localPosition.x + spriteSlideInAmount,
                spriteTransform.localPosition.y, spriteTransform.localPosition.z);
            Vector3 newSpritePos = spriteTransform.localPosition;
            spriteTransform.localPosition = oldSpritePos;
            
            // Initialising all the fade in stuff
            testSprite.color = transparentPanelColour;

            // The delay between the panel transition and the sprite transition
            yield return new WaitForSeconds(npcSpriteInDelay);

            while (timeElapsed < endTime)
            {
                if (timeElapsed < spriteFadeInTime)
                {
                    testSprite.color = Color.Lerp(transparentTestSpriteColour, opaqueTestSpriteColour, timeElapsed / spriteFadeInTime);
                }

                spriteTransform.localPosition =
                    Vector3.Lerp(oldSpritePos, newSpritePos, spriteSlideInCurve.Evaluate(timeElapsed));
                
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            spriteTransform.localPosition = newSpritePos;
            testSprite.color = opaqueTestSpriteColour;

            readyToProceed = true;

            Debug.Log("Sprite transition in is done.");
        }

        private IEnumerator BeginPanelTransitionOut()
        {
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

        private IEnumerator BeginSpriteTransitionOut()
        {
            float timeElapsed = 0;

            yield return new WaitForSeconds(npcSpriteOutDelay);
            
            while (timeElapsed < spriteFadeOutTime)
            {
                testSprite.color = Color.Lerp(opaqueTestSpriteColour, transparentTestSpriteColour,
                    timeElapsed / panelFadeOutTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            testSprite.color = transparentTestSpriteColour;
            Debug.Log("Sprite transition out is done.");
        }
    }
}
