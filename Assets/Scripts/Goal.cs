using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Goal
{
    [SerializeField]
    public List<ActiveGoalCondition> winConditions;
    public Goal()
    {
        winConditions = new List<ActiveGoalCondition>();
    }
    public bool complete()
    {
        foreach(var cond in winConditions)
        {
            if(!cond.Met())
            {
                return false;
            }
        }
        return true;
    }
}
[Serializable]
public class ActiveGoalCondition
{
    [SerializeField]
    public GoalCondition cond;
    public int currentAmount;
    public HashSet<GoalCondition> incompatibleSet;
    public int requiredAmount;
    public void Init(GoalCondition condition, int level)
    {
        cond = condition;
        incompatibleSet = new HashSet<GoalCondition>();
        foreach(var incCond in cond.incompatibleConditions)
        {
            incompatibleSet.Add(incCond);
        }
        requiredAmount = UnityEngine.Random.Range(cond.minRequiredAmount, cond.maxRequiredAmount) + (level > 1 ? Mathf.FloorToInt(Mathf.Sqrt(level)) : 0);
    }
    public void Reset()
    {
        currentAmount = 0;
    }
    public string GenerateDescription()
    {
        return String.Format(cond.description, Math.Min(currentAmount,requiredAmount), requiredAmount);
    }
    
    public virtual bool Met()
    {
        return cond.Met(currentAmount,requiredAmount);
    }

    public virtual void increaseProgress()
    {
        currentAmount++;
    }
}
[Serializable]
public abstract class GoalCondition : ScriptableObject
{
    public int minLevel;
    public int minRequiredAmount;
    public int maxRequiredAmount;
    public List<GoalCondition> incompatibleConditions;
    public string description;
    public bool hidden;
    public List<GoalCondition> requiredConditions;

    public virtual bool Met(int currentAmount, int requiredAmount)
    {
        return currentAmount >= requiredAmount;
    }

}