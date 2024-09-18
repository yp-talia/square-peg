        // Modifying script to enable Dialogues which are not user advancable
        // Add changes/additions are marked with NOTE:

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HeneGames.DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        private int currentSentence;
        private float coolDownTimer;
        private bool dialogueIsOn;
        private DialogueTrigger dialogueTrigger;

        public enum TriggerState
        {
            Collision,
            Input
        }

        [Header("References")]
        [SerializeField] private AudioSource audioSource;

        [Header("Events")]
        public UnityEvent startDialogueEvent;
        public UnityEvent nextSentenceDialogueEvent;
        public UnityEvent endDialogueEvent;

        [Header("Dialogue")]
        [SerializeField] private TriggerState triggerState;
        [SerializeField] private List<NPC_Centence> sentences = new List<NPC_Centence>();
        
        private void Update()
        {
            //Timer
            if(coolDownTimer > 0f)
            {
                coolDownTimer -= Time.deltaTime;
            }
            // NOTE: Adding automatic advancement to next sentence if timer expires and...
            // Dialogue is on and we've disabled Input
            else if (dialogueIsOn && sentences[currentSentence].disableInput == true)
            {
                NextSentence(out bool lastSentence);
            }
            //NOTE: Trying to protect against intermittent errors when pause/unpausing scene in Editor
            if (DialogueUI.instance != null && sentences.Count > 0)
            {
                //Start dialogue by input
                if (Input.GetKeyDown(DialogueUI.instance.actionInput) && dialogueTrigger != null && !dialogueIsOn)
                {
                    //Trigger event inside DialogueTrigger component
                    if (dialogueTrigger != null)
                    {
                        dialogueTrigger.startDialogueEvent.Invoke();
                    }

                    startDialogueEvent.Invoke();

                    //If component found start dialogue
                    DialogueUI.instance.StartDialogue(this);

                    //Hide interaction UI
                    DialogueUI.instance.ShowInteractionUI(false);

                    dialogueIsOn = true;
                }
                // NOTE: the sibling of the else if above, allow manual advance if disable input is false
                if (dialogueIsOn && sentences[currentSentence].disableInput == false && Input.GetKeyDown(DialogueUI.instance.actionInput))
                {
                    NextSentence(out bool lastSentence);
                }
            }
            else
            //NOTE: Adding in Debug logging, not integrating with Data Manager as I hope to not touch this file again.
            // FUTURE ME: do integrate with DataManager.debugOnWarn if we see this comment!
            {
                Debug.LogWarning("Either there are no sentences, or DialogueUI.instance is/has become unassigned");
            }
        }

        //Start dialogue by trigger
        private void OnTriggerEnter(Collider other)
        {
            if (triggerState == TriggerState.Collision && !dialogueIsOn)
            {
                //Try to find the "DialogueTrigger" component in the crashing collider
                if (other.gameObject.TryGetComponent<DialogueTrigger>(out DialogueTrigger _trigger))
                {
                    //Trigger event inside DialogueTrigger component and store refenrece
                    dialogueTrigger = _trigger;
                    dialogueTrigger.startDialogueEvent.Invoke();

                    startDialogueEvent.Invoke();

                    //If component found start dialogue
                    DialogueUI.instance.StartDialogue(this);

                    dialogueIsOn = true;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (triggerState == TriggerState.Collision && !dialogueIsOn)
            {
                //Try to find the "DialogueTrigger" component in the crashing collider
                if (collision.gameObject.TryGetComponent<DialogueTrigger>(out DialogueTrigger _trigger))
                {
                    //Trigger event inside DialogueTrigger component and store refenrece
                    dialogueTrigger = _trigger;
                    dialogueTrigger.startDialogueEvent.Invoke();

                    startDialogueEvent.Invoke();

                    //If component found start dialogue
                    DialogueUI.instance.StartDialogue(this);

                    dialogueIsOn = true;
                }
            }
        }

        //Start dialogue by pressing DialogueUI action input
        private void OnTriggerStay(Collider other)
        {
            if (dialogueTrigger != null)
                return;

            if (triggerState == TriggerState.Input && dialogueTrigger == null)
            {
                //Try to find the "DialogueTrigger" component in the crashing collider
                if (other.gameObject.TryGetComponent<DialogueTrigger>(out DialogueTrigger _trigger))
                {
                    //Show interaction UI
                    DialogueUI.instance.ShowInteractionUI(true);

                    //Store refenrece
                    dialogueTrigger = _trigger;
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (dialogueTrigger != null)
                return;

            if (triggerState == TriggerState.Input && dialogueTrigger == null)
            {
                //Try to find the "DialogueTrigger" component in the crashing collider
                if (collision.gameObject.TryGetComponent<DialogueTrigger>(out DialogueTrigger _trigger))
                {
                    //Show interaction UI
                    DialogueUI.instance.ShowInteractionUI(true);

                    //Store refenrece
                    dialogueTrigger = _trigger;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //Try to find the "DialogueTrigger" component from the exiting collider
            if (other.gameObject.TryGetComponent<DialogueTrigger>(out DialogueTrigger _trigger))
            {
                //Hide interaction UI
                DialogueUI.instance.ShowInteractionUI(false);

                //Stop dialogue
                StopDialogue();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            //Try to find the "DialogueTrigger" component from the exiting collider
            if (collision.gameObject.TryGetComponent<DialogueTrigger>(out DialogueTrigger _trigger))
            {
                //Hide interaction UI
                DialogueUI.instance.ShowInteractionUI(false);

                //Stop dialogue
                StopDialogue();
            }
        }

        public void StartDialogue()
        {
            //Start event
            if(dialogueTrigger != null)
            {
                dialogueTrigger.startDialogueEvent.Invoke();
            }

            //Reset sentence index
            currentSentence = 0;

            //Show first sentence in dialogue UI
            ShowCurrentSentence();

            //Play dialogue sound
            PlaySound(sentences[currentSentence].sentenceSound);

            //Cooldown timer
            coolDownTimer = sentences[currentSentence].skipDelayTime;
        }

        public void NextSentence(out bool lastSentence)
        {   
            //The next sentence cannot be changed immediately after starting
            if (coolDownTimer > 0f)
            {
                lastSentence = false;
                return;
            }

            //Add one to sentence index
            currentSentence++;

            //Next sentence event
            if (dialogueTrigger != null)
            {
                dialogueTrigger.nextSentenceDialogueEvent.Invoke();
            }

            nextSentenceDialogueEvent.Invoke();

            //If last sentence stop dialogue and return
            if (currentSentence > sentences.Count - 1)
            {
                StopDialogue();

                lastSentence = true;

                return;
            }

            //If not last sentence continue...
            lastSentence = false;

            //Play dialogue sound
            PlaySound(sentences[currentSentence].sentenceSound);

            //Show next sentence in dialogue UI
            ShowCurrentSentence();

            //Cooldown timer
            coolDownTimer = sentences[currentSentence].skipDelayTime;
        }

        public void StopDialogue()
        {
            //Stop dialogue event
            if (dialogueTrigger != null)
            {
                dialogueTrigger.endDialogueEvent.Invoke();
            }

            endDialogueEvent.Invoke();

            //Hide dialogue UI
            DialogueUI.instance.ClearText();

            //Stop audiosource so that the speaker's voice does not play in the background
            if(audioSource != null)
            {
                audioSource.Stop();
            }

            //Remove trigger refence
            dialogueIsOn = false;
            dialogueTrigger = null;
        }

        private void PlaySound(AudioClip _audioClip)
        {
            //Play the sound only if it exists
            if (_audioClip == null || audioSource == null)
                return;

            //Stop the audioSource so that the new sentence does not overlap with the old one
            audioSource.Stop();

            //Play sentence sound
            audioSource.PlayOneShot(_audioClip);
        }

        private void ShowCurrentSentence()
        {
            if (sentences[currentSentence].dialogueCharacter != null)
            {
                //Show sentence on the screen
                DialogueUI.instance.ShowSentence(sentences[currentSentence].dialogueCharacter, sentences[currentSentence].sentence);

                //Invoke sentence event
                sentences[currentSentence].sentenceEvent.Invoke();
            }
            else
            {
                DialogueCharacter _dialogueCharacter = new DialogueCharacter();
                _dialogueCharacter.characterName = "";
                _dialogueCharacter.characterPhoto = null;

                DialogueUI.instance.ShowSentence(_dialogueCharacter, sentences[currentSentence].sentence);

                //Invoke sentence event
                sentences[currentSentence].sentenceEvent.Invoke();
            }
        }

        public int CurrentSentenceLenght() //sic?
        {
            if(sentences.Count <= 0)
                return 0;

            return sentences[currentSentence].sentence.Length;
        }

        //NOTE: Function to publicly check if DisabledInput is true on the current sentence
        public bool CurrentSentenceDisabledInput()
        {
            if(sentences != null && sentences.Count > 0 && currentSentence < sentences.Count)
            {
                return sentences[currentSentence].disableInput;
            }
            else
            {
                return false;
            }
        }
    }
    

    [System.Serializable]
    public class NPC_Centence //sic?
    {
        [Header("------------------------------------------------------------")]

        public DialogueCharacter dialogueCharacter;

        [TextArea(3, 10)]
        public string sentence;

        public float skipDelayTime = 0.5f;

        // Toggle to force sentence to advance at the end of skipDelayTime...
        // and ignore user input (e.g. when additively loading a scene and also)...
        // wanting the character to interject at the same time
        //NOTE: Added tooltip and disableInput bool
        [Tooltip("If enabled, then Player Interaction disabled and automatically advances to next Sentence after Skip Delay Time")]
        public bool disableInput = false; 

        public AudioClip sentenceSound;

        public UnityEvent sentenceEvent;
    }
}