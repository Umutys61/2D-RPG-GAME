using System;
using UnityEngine;

[Serializable]
public class FSMState 
{
    public string ID;
    public FSMAction[] actions;
    public FSMTransition[] transitions;

    public void UpdateState(EnemyBrain enemyBrain)
    {
        ExecuteActions();
        ExecuteTransitions(enemyBrain);

    }
    private void ExecuteActions()
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act();
        }
    }
    private void ExecuteTransitions(EnemyBrain enemyBrain)
    {
        if (transitions == null || transitions.Length == 0)
        {
            return;
        }
        for (int i = 0; i < transitions.Length; i++)
        {
            bool value = transitions[i].decision.Decide();
            if (value)
            {
                enemyBrain.ChangeState(transitions[i].TrueState);
            }
            else
            {
                enemyBrain.ChangeState(transitions[i].FalseState);
            }
        }

    }
}
