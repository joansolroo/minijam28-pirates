using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cardRule", menuName = "Cards/Action", order = 1)]
public class CardAction : ScriptableObject
{

    public string name;
    public AudioClip clip;

    // TODO: change this for polymorphism
    public enum RuleType
    {
        move = 2, attack = 4, scan = 8, heal = 16, boarding = 32, None = 0, Any = 64
    }
    public RuleType ruleType;
    public enum AttackTarget
    {
        All, notMoving, Moving
    }
    public AttackTarget target;

    [SerializeField] public int amount = 1;
    [SerializeField] public int range = 1;
    [SerializeField] float resolutionDuration = 1;

    public bool Resolve(Card container)
    {
        Player owner = container.owner;
        Player enemy = owner.enemy;
        Ship ownerShip = owner.ship;
        Ship enemyShip = enemy.ship;
        Map map = Map.current;
        if (ruleType == RuleType.attack)
        {
            if (amount > 0)
            {
                int pos1 = ownerShip.position;
                int pos1prev = ownerShip.previousPosition;
                int pos2 = enemyShip.position;
                int pos2prev = enemyShip.previousPosition;

                int aimedPos = -1;
                if (target == AttackTarget.All)
                {
                    aimedPos = pos2;
                }
                else if (target == AttackTarget.Moving)
                {
                    if (enemyShip.moved)
                    {
                        aimedPos = pos2;
                    }
                    else
                    {
                        aimedPos = pos2 - enemyShip.direction;
                    }
                }
                else if (target == AttackTarget.notMoving)
                {
                    aimedPos = pos2prev;
                }

                if (range > Mathf.Abs(pos1 - pos2))
                {
                    bool hit = aimedPos != -1 && pos2 == aimedPos;

                    if (!hit)
                    {
                        Debug.Log("Attack: attack missed");
                        ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                        parameter.startColor = Color.gray;
                        map.tiles[aimedPos].emission.Emit(parameter, Random.Range(5, 15));
                        ownerShip.audioSource.PlayOneShot(clip);
                    }
                    else
                    {
                        Debug.Log("Attack: attack hit");

                        owner.enemy.Hurt(amount);

                        ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                        parameter.startColor = owner.enemy.color;
                        map.tiles[aimedPos].emission.Emit(parameter, Random.Range(30, 50));
                        ownerShip.audioSource.PlayOneShot(clip);
                        //enemyShip.audioSource.clip = this.rule.explosionClip;
                        //enemyShip.audioSource.PlayDelayed(0.25f);
                    }

                    return hit;

                }
                else
                {
                    Debug.Log("Attack: Out of range");
                }

            }
            return false;
        }
        else if (ruleType == RuleType.move)
        {
            if (this.amount > 0)
            {
                ownerShip.MoveTo(ownerShip.position + ownerShip.direction * this.amount);
                ownerShip.audioSource.PlayOneShot(this.clip);
                return true;
            }
        }
        else if (ruleType == RuleType.scan)
        {

            foreach (Card c in owner.enemy.hand.cards)
            {

                c.SetVisible(true);

                ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                parameter.startColor = container.rule.color;
                container.map.tiles[enemyShip.position].emission.Emit(parameter, Random.Range(5, 15));
                ownerShip.audioSource.PlayOneShot(this.clip);
            }
            return true;
        }
        else if (ruleType == RuleType.heal)
        {
            if (this.amount > 0)
            {
                owner.Heal(this.amount, true);

                ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                parameter.startColor = container.rule.color;
                container.map.tiles[ownerShip.position].emission.Emit(parameter, Random.Range(5, 15));
                ownerShip.audioSource.PlayOneShot(this.clip);
            }
        }
        else if (ruleType == RuleType.boarding)
        {
            if (Mathf.Abs(ownerShip.position - enemyShip.position) <= this.range)
            {
                Fight.current.OnBoard(owner, owner.enemy);

                ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                parameter.startColor = owner.color;
                Map.current.tiles[enemyShip.position].emission.Emit(parameter, Random.Range(5, 15));
                ownerShip.audioSource.PlayOneShot(clip);
            }
        }
        return false;
    }

    public void Preview(Card card)
    {

    }
}
