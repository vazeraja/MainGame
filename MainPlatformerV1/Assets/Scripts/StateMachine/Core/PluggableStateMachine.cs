using System.Collections.Generic;
using System.Linq;
using MainGame;
using UnityEngine;

public class PluggableStateMachine : MonoBehaviour {
    
    [SerializeField] public List<BaseState<SasukeController, SasukeStateSO>> states;
    
    
    
}