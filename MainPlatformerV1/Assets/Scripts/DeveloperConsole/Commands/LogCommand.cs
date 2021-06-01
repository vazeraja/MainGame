using UnityEngine;
namespace MainGame.DeveloperConsole {
    
    [CreateAssetMenu(fileName = "New Log Command", menuName = "DeveloperConsole/Commands/Log Command", order = 0)]
    public class LogCommand : ConsoleCommand {
        public override bool Process(string[] args){
            string logText = string.Join(" ", args);
            Debug.Log(logText);
            return true;
        }
    }
}
