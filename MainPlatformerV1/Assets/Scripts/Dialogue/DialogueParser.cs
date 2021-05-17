using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame.DialogueGraph {

    public class DialogueParser : MonoBehaviour {
        [SerializeField] private DialogueContainer dialogue;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Button choicePrefab;
        [SerializeField] private Transform buttonContainer;

        private void Start(){
            var narrativeData = dialogue.NodeLinks.First(); //Entrypoint node
            ProceedToNarrative(narrativeData.TargetNodeGUID);
        }

        private void ProceedToNarrative(string narrativeDataGUID){
            string text = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).DialogueText;
            var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID);
            // dialogueText.text = ProcessProperties(text);
            dialogueText.text = text;
            var buttons = buttonContainer.GetComponentsInChildren<Button>();
            foreach (var button in buttons) {
                Destroy(button.gameObject);
            }

            foreach (var choice in choices) {
                var button = Instantiate(choicePrefab, buttonContainer);
                // button.GetComponentInChildren<Text>().text = ProcessProperties(choice.PortName);
                button.GetComponentInChildren<Text>().text = choice.PortName;
                button.onClick.AddListener(() => ProceedToNarrative(choice.TargetNodeGUID));
            }
        }

        // private string ProcessProperties(string text){
        //     foreach (var exposedProperty in dialogue.ExposedProperties) {
        //         text = text.Replace($"[{exposedProperty.PropertyName}]", exposedProperty.PropertyValue);
        //     }
        //     return text;
        // }
    }
}
