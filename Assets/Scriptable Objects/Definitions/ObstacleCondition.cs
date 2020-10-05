using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable][CreateAssetMenu(fileName = "New Obstacle Condition", menuName = "Custom/Goal Conditions/Obstacle Condition")]
public class ObstacleCondition : GoalCondition
{
    [SerializeField]
    public string desiredTag;
    [SerializeField]
    public bool collect;
    [SerializeField]
    public bool avoid;
    [SerializeField]
    public bool shoot;
    [SerializeField]
    public bool exactly;
}