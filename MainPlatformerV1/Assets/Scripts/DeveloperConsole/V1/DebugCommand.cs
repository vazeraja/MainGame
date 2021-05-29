using System;
using UnityEngine;

namespace MainGame {
    public class DebugCommandBase {
        private readonly string _commandId;
        private readonly string _commandDescription;
        private readonly string _commandFormat;
        
        public string commandId => _commandId;
        public string commandDescription => _commandDescription;
        public string commandFormat => _commandFormat;
        
        public DebugCommandBase(string id, string description, string format){
            _commandId = id;
            _commandDescription = description;
            _commandFormat = format;
        }
    }
    public class DebugCommand : DebugCommandBase {

        private readonly Action _command;

        public DebugCommand(string id, string description, string format, Action command) : base(id, description, format){
            _command = command;
        }
        public void Invoke() => _command.Invoke();
    }
    public class DebugCommand<T1> : DebugCommandBase {

        private readonly Action<T1> _command;

        public DebugCommand(string id, string description, string format, Action<T1> command) : base(id, description, format){
            _command = command;
        }
        public void Invoke(T1 value) => _command.Invoke(value);
    }
}
