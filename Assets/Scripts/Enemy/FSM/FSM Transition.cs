using System;
using UnityEngine;

[Serializable]
public class FSMTransition 
{
    public FSMDecision decision;
    public string TrueState;
    public string FalseState;

    
}
