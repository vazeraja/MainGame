using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Decision : IDecision {

    [HideInInspector] public SasukeController Player;
    
    public abstract bool Decide();
}