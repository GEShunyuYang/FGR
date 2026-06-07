using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Effect", menuName = "Card Effects/Damage")]
public class DamageEffect : CardEffect
{
    [SerializeField] private float Damage = 10f;

    public override void BuildActions(CardEffectContext context, BattleActionQueue queue)
    {
        if (context.Target == null) return;

        queue.Enqueue(new DamageAction(context.Target, Damage));
    }
}
