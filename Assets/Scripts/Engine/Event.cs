using System;
using System.Collections.Generic;

public class EngineEvent
{

    private List<Action> actions;

    public EngineEvent()
    {
        actions = new List<Action>();
    }

    public void AddAction(Action action)
    {
        actions.Add(action);
    }

    public void RemoveAction(Action action)
    {
        actions.Remove(action);
    }

    public void Execute()
    {
        for (int i = 0; i < this.actions.Count; i++)
        {
            actions[i]();
        }
    }

    public int GetCount()
    {
        return actions.Count;
    }

    public void ClearActions()
    {
        actions.Clear();
    }

    public static EngineEvent operator +(EngineEvent targetEvent, Action targetAction)
    {
        targetEvent.AddAction(targetAction);
        return targetEvent;
    }

    public static EngineEvent operator -(EngineEvent targetEvent, Action targetAction)
    {
        targetEvent.RemoveAction(targetAction);
        return targetEvent;
    }

}
