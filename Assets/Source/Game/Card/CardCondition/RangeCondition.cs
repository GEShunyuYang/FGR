using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Range Condition", menuName = "Card Conditions/Range")]
public class RangeCondition : CardCondition
{
    [SerializeField] private int MaxRange = 1;

    public int MaxRangeValue => MaxRange;

    public override bool IsSatisfied(CardEffectContext context)
    {
        if (context.Caster == null || context.Target == null)
        {
            return false;
        }

        Vector2Int casterPos = context.Caster.GridPos;
        Vector2Int targetPos = context.Target.GridPos;

        int distance = Mathf.Abs(casterPos.x - targetPos.x)
                     + Mathf.Abs(casterPos.y - targetPos.y);

        return distance <= MaxRange;
    }
}