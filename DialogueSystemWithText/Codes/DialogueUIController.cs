using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DialogueSystemWithText;

namespace DialogueSystemWithText
{
    public class DialogueUIController : MonoBehaviour
    {

        [SerializeField]
        private float _fontTypingSpeed = 0.05f;

        [SerializeField, Tooltip("This KeyCode is to skip the dialogue or show next dialogue.")]
        private KeyCode _keyCodeSkipDialogue;

        [SerializeField]
        private Font _dialogueFont;

        [SerializeField]
        private Color _fontColor;

        [SerializeField, Tooltip("The DialogueContent that is created is automatically added to this list to make it easier to find. Note: This list is not used for dialog system logic so it does not need to allocate all references in the list.")]
        private List<DialogueContent> _dialogueContents = new List<DialogueContent>();

        [SerializeField]
        private DialogueContent _firstDialogueContent;

        [SerializeField, ReadOnlyField, Tooltip("Dialogue that is currently displayed. This field cannot be assigned from the inspector.")]
        private DialogueContent _currentDialogueContent;

        [SerializeField, PrefabModeOnlyField]
        private Text _dialogueTextWithoutImage;

        [SerializeField, PrefabModeOnlyField]
        private Text _dialogueTextForLeftImage;

        [SerializeField, PrefabModeOnlyField]
        private Text _dialogueTextForRightImage;

        [SerializeField, PrefabModeOnlyField]
        private Image _characterLeftImage;

        [SerializeField, PrefabModeOnlyField]
        private Image _characterRightImage;

        [SerializeField, PrefabModeOnlyField]
        private RectTransform _dialogueOptionsLayout;
        
        [SerializeField, PrefabModeOnlyField]
        private GameObject _dialogueOptionButton;

        private float _fontTypingOriginalSpeed;
        private bool _skippableTypeDialogue = true;
        private bool _canNextDialogue = false;
        private Text _dialogueText;
        private Canvas _canvas;

        public float FontTypingSpeed { get => _fontTypingSpeed; set => _fontTypingSpeed = value; }
        public KeyCode KeyCodeSkipDialogue { get => _keyCodeSkipDialogue; set => _keyCodeSkipDialogue = value; }
        public Font DialogueFont { get => _dialogueFont; set => _dialogueFont = value; }
        public Color FontColor { get => _fontColor; set => _fontColor = value; }
        public List<DialogueContent> DialogueContents { get => _dialogueContents; set => _dialogueContents = value; }
        public DialogueContent FirstDialogueContent { get => _firstDialogueContent; set => _firstDialogueContent = value; }

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            if(FontTypingSpeed < 0f)
                FontTypingSpeed = 0f;

            _fontTypingOriginalSpeed = FontTypingSpeed;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCodeSkipDialogue) && _canvas.enabled && KeyCodeSkipDialogue != KeyCode.None && _skippableTypeDialogue)
            {
                SkipTypingDialogue();
            }

            if(Input.GetKeyDown(KeyCodeSkipDialogue) && _canvas.enabled && KeyCodeSkipDialogue != KeyCode.None && _canNextDialogue)
            {
                NextDialogue();
            }
        }

        /// <summary>Method to show DialogueUI(DialogueUICanvas) on the screen.</summary>
        public void ShowDialogueUI()
        {
            if(FirstDialogueContent == null)
            {
                Debug.LogError($"Conflict in GameObject {gameObject.name}. The dialogue cannot be displayed. In the script {this.GetType().ToString()} the First Dialogue Content field the reference must be assigned.");
                return;
            }

            _canvas.enabled = true;
            StartDialogue(FirstDialogueContent);
        }

        /// <summary>Method to hide DialogueUI(DialogueUICanvas) on the screen.</summary>
        internal void HideDialogueUI()
        {
            _canvas.enabled = false;
            ClearDialogueOptions();
        }

        /// <summary>Method to skip the dialogue typing.</summary>
        public void SkipTypingDialogue()
        {
            if(!_skippableTypeDialogue)
                return;

            _skippableTypeDialogue = false;
            FontTypingSpeed = 0.0f;
            _canNextDialogue = false;
        }

        /// <summary> Method to start the dialogue.</summary>
        /// <param name="dialogueContent">The dialogue that will be displayed in the DialogueUICanvas.</param>
        private void StartDialogue(DialogueContent dialogueContent)
        {
            dialogueContent.InvokeDialogueStartEvent();
            ClearDialogueOptions();
            SetDialogue(dialogueContent);
        }

        /// <summary>Method to disables some of the UIs components of the DialogueUICanvas.</summary>
        private void DisabledDialogueUIComponents()
        {
            _dialogueTextWithoutImage.enabled = false;
            _dialogueTextForLeftImage.enabled = false;
            _dialogueTextForRightImage.enabled = false;
            _characterLeftImage.enabled = false;
            _characterRightImage.enabled = false;
        }
        
        /// <summary>Method to set data and specifications of the dialogue in the DialogueUICanvas.</summary>
        /// <param name="dialogueContent">DialogueContent which has the data to display and the specifications.</param>
        private void SetDialogue(DialogueContent dialogueContent)
        {
            DisabledDialogueUIComponents();
            EnableCorrespondingDialogueUIComponents(dialogueContent);
            SetDialogueFont(dialogueContent);
            SetDialogueFontColor(dialogueContent);
            StartCoroutine(TypeDialogueCoroutine(dialogueContent));
        }

        /// <summary>Method to enable the corresponding UI components of the DialogueUICanvas.</summary>
        /// <param name="dialogueContent">DialogueContent that has the specifications to know which UI components to enable.</param>
        private void EnableCorrespondingDialogueUIComponents(DialogueContent dialogueContent)
        {
            if(dialogueContent.CharacterImageSprite != null && dialogueContent.CharacterImagePosition == CharacterImagePosition.Right)
            {
                _dialogueText = _dialogueTextForRightImage;
                _dialogueText.enabled = true;
                _characterRightImage.sprite = dialogueContent.CharacterImageSprite;
                _characterRightImage.enabled = true;
                return;
            }
            if(dialogueContent.CharacterImageSprite != null && dialogueContent.CharacterImagePosition == CharacterImagePosition.Left)
            {
                _dialogueText = _dialogueTextForLeftImage;
                _dialogueText.enabled = true;
                _characterLeftImage.sprite = dialogueContent.CharacterImageSprite;
                _characterLeftImage.enabled = true;
                return;
            }
            if(dialogueContent.CharacterImageSprite == null)
            {
                _dialogueText = _dialogueTextWithoutImage;
                _dialogueText.enabled = true;
                return;
            }
        }

        /// <summary>Method to set the Font in the dialogue Text component.</summary>
        /// <param name="dialogueContent">DialogueContent that has the font specifications to set.</param>
        private void SetDialogueFont(DialogueContent dialogueContent)
        {
            if(dialogueContent.CustomizeFont && dialogueContent.DialogueFont != null)
            {
                _dialogueText.font = dialogueContent.DialogueFont;
                return;
            }

            if(!dialogueContent.CustomizeFont && DialogueFont != null)
            {
                _dialogueText.font = DialogueFont;
                return;
            }
        }

        /// <summary>Method to set the Color in the dialogue Text component.</summary>
        /// <param name="dialogueContent">DialogueContent that has the color specifications to set.</param>
        private void SetDialogueFontColor(DialogueContent dialogueContent)
        {
            if(dialogueContent.CustomizeFont)
            {
                _dialogueText.color = dialogueContent.FontColor;
            }
            else
            {
                _dialogueText.color = FontColor;
            }
        }

        /// <summary>Method to get the name of the character with its specifications as Rich Text.</summary>
        /// <param name="dialogueContent">DialogueContent that has the specifications to set for the character's name.</param>
        /// <returns>The name of the character plus its specifications as Rich Text</returns>
        private string GetCharacterName(DialogueContent dialogueContent)
        {
            if(String.IsNullOrEmpty(dialogueContent.CharacterName))
                return "";
            
            string characterName  = dialogueContent.CharacterName;

            if(dialogueContent.CustomizeCharacterName && dialogueContent.WithLineBreak) {
                characterName = $"{characterName}\n";
            } else {
                characterName = $"{characterName}: ";
            }

            if(dialogueContent.CustomizeCharacterName)
                characterName = $"<color=#{ColorUtility.ToHtmlStringRGB(dialogueContent.CharacterNameFontColor)}>{characterName}</color>";

            if(dialogueContent.CustomizeCharacterName && dialogueContent.CharacterNameFontBold)
                characterName = $"<b>{characterName}</b>";
            
            if(dialogueContent.CustomizeCharacterName && dialogueContent.CharacterNameFontItalic)
                characterName = $"<i>{characterName}</i>";
            
            if(dialogueContent.CustomizeCharacterName && dialogueContent.CharacterNameFontSize > 0)
                characterName = $"<size={dialogueContent.CharacterNameFontSize}>{characterName}</size>";
            
            return characterName;
        }

        /// <summary>Method to place the option buttons in DialogueUICanvas if the dialogue is with options.</summary>
        /// <param name="dialogueContent">DialogueContent that has the specification if it is a dialogue with options.</param>
        private void SetDialogueOptions(DialogueContent dialogueContent)
        {
            if(dialogueContent.DialogueOptions.Count == 0)
            {
                _dialogueOptionsLayout.gameObject.SetActive(false);
                return;
            }

            _dialogueOptionsLayout.gameObject.SetActive(true);

            foreach(var dialogueOption in dialogueContent.DialogueOptions)
            {
                var dialogueOptionButton = Instantiate(_dialogueOptionButton, new Vector3(0, 0, 0), Quaternion.identity);
                dialogueOptionButton.transform.SetParent(_dialogueOptionsLayout.transform);
                dialogueOptionButton.transform.localScale = Vector3.one;
                dialogueOptionButton.GetComponent<DialogueOptionButton>().DialogueOption = dialogueOption;
                dialogueOptionButton.GetComponent<DialogueOptionButton>().DialogueUIController = this;
            }

            return;
        }

        /// <summary>Method to delete the option buttons in DialogueUICanvas.</summary>
        private void ClearDialogueOptions()
        {
            foreach(Transform child in _dialogueOptionsLayout.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        /// <summary>Coroutine to type the letters of the dialog in DialogueUICanvas.</summary>
        /// <param name="dialogueContent">DialogueContent that has the dialogue to type.</param>
        private IEnumerator TypeDialogueCoroutine(DialogueContent dialogueContent)
        {
            _currentDialogueContent = dialogueContent;

            var dialogue = dialogueContent.Dialogue;

            _dialogueText.text = GetCharacterName(dialogueContent);

            if(_fontTypingSpeed <= 0)
            {
                _dialogueText.text += dialogue;
            }
            else
            {
                foreach (char c in dialogue.ToCharArray())
                {
                    _dialogueText.text += c;
                    yield return new WaitForSeconds(FontTypingSpeed);
                }
            }

            SetDialogueOptions(dialogueContent);
            _skippableTypeDialogue = false;
            _canNextDialogue = true;
        }

        /// <summary>Method to go to the next dialogue to show in DialogueUICanvas.</summary>
        public void NextDialogue()
        {
            if(!_canNextDialogue)
                return;
            
            if( _currentDialogueContent.DialogueOptions.Count > 0)
                return;

            _skippableTypeDialogue = true;
            _canNextDialogue = false;
            FontTypingSpeed = _fontTypingOriginalSpeed;

            _currentDialogueContent.InvokeDialogueEndEvent();

            if(_currentDialogueContent.NextDialogueContent != null)
            {
                _dialogueText.text = string.Empty;
                StartDialogue(_currentDialogueContent.NextDialogueContent);
            }
            else
            {
                HideDialogueUI();
            }
        }

        /// <summary>Method to go to the next dialogue to show in DialogueUICanvas. This method is calling from the DialogueOptionButton script.</summary>
        /// <param name="dialogueContent">DialogueContent that has the next dialogue to show.</param>
        public void NextDialogue(DialogueContent dialogueContent)
        {
            if(!_canNextDialogue)
                return;

            _skippableTypeDialogue = true;
            _canNextDialogue = false;
            FontTypingSpeed = _fontTypingOriginalSpeed;

            _currentDialogueContent.InvokeDialogueEndEvent();

            _dialogueText.text = string.Empty;
            StartDialogue(dialogueContent);
        }

        /// <summary>Method to create DialogueContent in DialogueUICanvas. This method cannot be used in Play Mode.</summary>
        [ContextMenu("Create DialogueContent")]
        public void CreateDialogueContent()
        {
            if(Application.isPlaying) {
                Debug.LogError("This method cannot be called when in Play Mode.");
                return;
            }

            GameObject dialogueContent = new GameObject();
            dialogueContent.name = "DialogueContent";
            dialogueContent.AddComponent<DialogueContent>();
            dialogueContent.transform.SetParent(transform);
            DialogueContents.Add(dialogueContent.GetComponent<DialogueContent>());

            if(FirstDialogueContent == null)
                FirstDialogueContent = dialogueContent.GetComponent<DialogueContent>();
        }
    }
}
