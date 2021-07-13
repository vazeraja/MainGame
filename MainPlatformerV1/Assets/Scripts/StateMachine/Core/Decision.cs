using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDecision {
    bool Decide();
}

[Serializable]
public abstract class Decision : IDecision {

    [HideInInspector] public SasukeController player;
    public abstract bool Decide();
}