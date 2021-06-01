using System;
using System.Collections.Generic;
using System.Linq;
namespace MainGame.DeveloperConsole {
    
    public class DeveloperConsole {
        
        private readonly string prefix;
        private readonly IEnumerable<IConsoleCommand> commands;

        public DeveloperConsole(string prefix, IEnumerable<IConsoleCommand> commands){
            this.prefix = prefix;
            this.commands = commands;
        }
        public void ProcessCommand(string inputValue){
            if (!inputValue.StartsWith(prefix)) { return; }
            
            inputValue = inputValue.Remove(0, prefix.Length);

            string[] inputSplit = inputValue.Split(' ');
            string commandInput = inputSplit[0];
            string[] args = inputSplit.Skip(1).ToArray();

            ProcessCommand(commandInput, args);
        }

        private void ProcessCommand(string commandInput, string[] args){
            if (commands.Where(command => commandInput.Equals(command.CommandWord, StringComparison.OrdinalIgnoreCase)).Any(command => command.Process(args))) {
                return;
            }
        }
    }
}
