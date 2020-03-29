using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using Yeeter;

public class DialogueManager : MonoBehaviour
{
    // Name of the dialogue ink/json meme
    [SerializeField] private string _dialogueFileName = null;
    [SerializeField] private Text _dialogueSpeaker = null;
    [SerializeField] private Text _dialogueBox = null;
    [SerializeField] private GameObject _dialogueOptionButtonPrefab = null;
    [SerializeField] private GameObject _dialogueOptionBoxPrefab = null;
    [SerializeField] private float _dialogueCharacterDelay = .001f;
    [SerializeField] private GameObject _selectionArrowPrefab = null;

    private bool _isCurrentlyTyping = false;
    private bool _dialogueOptionBoxIsOpen = false;
    private int _dialogueOptionsSelectedIndex = 0;
    private GameObject _selectionArrow;
    private List<char> _vowels = new List<char> { 'A', 'E', 'I', 'O', 'U', 'a', 'e', 'i', 'o', 'u' };
    private List<GameObject> _dialogueOptions = new List<GameObject>();
    private Story _inkStory;

    private void Awake()
    {
        SetDialogue("story");

        BBInput.SetActiveProfile("Dialogue");
        BBInput.AddOnAxisPressed("ContinueDialogue", NextSentence);
        BBInput.AddOnAxisPressed("DialogueOptionMoveUp", MoveDialogueOptionUp);
        BBInput.AddOnAxisPressed("DialogueOptionMoveDown", MoveDialogueOptionDown);

        StartCoroutine(DisplayDialogue());
    }

    private void OpenDialogueOptionBox()
    {
        _dialogueOptionBoxIsOpen = true;

        GameObject dialogueOptionBox = Instantiate(_dialogueOptionBoxPrefab, transform);

        for (int i = 0; i < _inkStory.currentChoices.Count; i++)
        {
            GameObject go = Instantiate(_dialogueOptionButtonPrefab, dialogueOptionBox.transform.GetChild(0));
            go.GetComponentInChildren<Text>().text = _inkStory.currentChoices[i].text;

            _dialogueOptions.Add(go);
        }

        _selectionArrow = Instantiate(_selectionArrowPrefab, dialogueOptionBox.transform);
        _dialogueOptionsSelectedIndex = 0;
        _selectionArrow.transform.position = _dialogueOptions[_dialogueOptionsSelectedIndex].transform.position; // + offset
        _selectionArrow.transform.SetParent(null);

        Canvas.ForceUpdateCanvases();
        _selectionArrow.transform.SetParent(dialogueOptionBox.transform);
    }
    private void CloseDialogueOptionBox()
    {
        _dialogueOptionBoxIsOpen = false;
    }
    private void MoveDialogueOptionUp()
    {
        if (_dialogueOptionsSelectedIndex > 0)
        {
            _dialogueOptionsSelectedIndex--;
        }

        _selectionArrow.transform.position = _dialogueOptions[_dialogueOptionsSelectedIndex].transform.position; // + offset
    }
    private void MoveDialogueOptionDown()
    {
        if (_dialogueOptionsSelectedIndex < _dialogueOptions.Count - 1)
        {
            _dialogueOptionsSelectedIndex++;
        }

        _selectionArrow.transform.position = _dialogueOptions[_dialogueOptionsSelectedIndex].transform.position; // + offset
    }
    private void ChooseSelectedDialogueOption()
    {
        _inkStory.ChooseChoiceIndex(_dialogueOptionsSelectedIndex);
        StartCoroutine(DisplayDialogue());
        CloseDialogueOptionBox();
    }
    private void NextSentence()
    {
        if (!_isCurrentlyTyping)
        {
            if (!_inkStory.canContinue && !_dialogueOptionBoxIsOpen)
            {
                _dialogueOptionBoxIsOpen = true;
                OpenDialogueOptionBox();
            }
            else if (!_inkStory.canContinue && _dialogueOptionBoxIsOpen)
            {
                ChooseSelectedDialogueOption();
            }
            else if (_inkStory.canContinue && !_dialogueOptionBoxIsOpen)
            {
                _inkStory.Continue();
                StartCoroutine(DisplayDialogue());
            }
        }
        else
        {
            DisplayAllDialogue();
        }
    }
    private void DisplayAllDialogue()
    {
        // Makes all characteres appear at once.

        StopCoroutine(DisplayDialogue());
        _isCurrentlyTyping = false;
        _dialogueBox.text = _inkStory.currentText;
    }
    private IEnumerator DisplayDialogue()
    {
        _isCurrentlyTyping = true;
        _dialogueBox.text = "";

        foreach (char c in _inkStory.currentText)
        {
            yield return new WaitForSeconds(_dialogueCharacterDelay);
            // Suggestion: have the delay change on punctation(, . ! ?) or other special characters

            _dialogueBox.text += c;

            if (_vowels.Contains(c))
            {
                // Play sound meme

                // Don't know if doing this when a vowel is displayed is the best way to do it
                // Playing a sound every letter might be too much
                // Playing a sound every 3 to 5 letters might look weird - atleast sounds like it will to me
            }
        }
        _isCurrentlyTyping = false;
    }
    private void SetDialogue(string s)
    {
        _inkStory = new Story(StreamingAssetsDatabase.GetDialogue(s));
    }
}
