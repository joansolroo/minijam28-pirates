using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    [SerializeField] Map map;
    [SerializeField] public Player owner;
    public CardRegion region;
    [SerializeField] public CardRule rule;

    public static Vector2 CARD_SIZE = new Vector2(3.2f, 4);
    
    private void OnValidate()
    {
        string name = "";
        if (owner != null)
        {
            name += owner.name;
            name += " ";
        }
        if (rule != null)
        {
            name += rule.name;
        }
        this.gameObject.name = name;
    }

    public void Play()
    {
        
    }

    public bool Move()
    {
        if (rule.movementAmount > 0)
        {
            map.Move(owner.ship, rule.movementAmount);

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Attack(bool enemyMoved)
    {
        if (rule.damageAmount > 0)
        {
            
            if(rule.attackMaxRange > Mathf.Abs(owner.ship.position - owner.enemy.ship.position))
            {
                bool hit = ((rule.attackTarget == CardRule.AttackTarget.All)
                           || (rule.attackTarget == CardRule.AttackTarget.notMoving && !enemyMoved)
                           || (rule.attackTarget == CardRule.AttackTarget.Moving && enemyMoved));

                if (!hit)
                {
                    Debug.Log("Attack: attack missed");
                }
                else
                {
                    Debug.Log("Attack: attack hit");
                }

                owner.enemy.Hurt(rule);

                return hit;
                
            }
            else
            {
                Debug.Log("Attack: Out of range");
            }
            
        }
        return false;
    }

    public void Repair()
    {

    }
}
