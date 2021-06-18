using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainGame.DeveloperConsole {

    [CreateAssetMenu(fileName = "New Switch Scene Command", menuName = "DeveloperConsole/Commands/Switch Scene Command", order = 1)]
    public class SwitchSceneCommand : ConsoleCommand {

        private readonly List<string> sceneNames = new List<string>();
        public override bool Process(string[] args){
            GetSceneNames();
            
            string input = string.Join(" ", args);
            foreach (string s in sceneNames.Where(s => input == s)) {
                SceneManager.LoadScene(input);
                return true;
            }

            int sceneNum;
            int.TryParse(input, out sceneNum);
            SceneManager.LoadScene(sceneNum);
            return true;
        }
        private void GetSceneNames(){
            var regex = new Regex(@"([^/]*/)*([\w\d\-]*)\.unity");
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string name = regex.Replace(path, "$2");
                sceneNames.Add(name);
            }
        }

    }
}
