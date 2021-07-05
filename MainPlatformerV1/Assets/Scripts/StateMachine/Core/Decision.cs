using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : ScriptableObject {

    [HideInInspector] public SasukeController player;
    
    public abstract bool Decide();
}