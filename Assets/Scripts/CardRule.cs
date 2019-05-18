using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cardRule", menuName = "Cards/rule", order = 1)]
public class CardRule : ScriptableObject {


    public Sprite art;
    public Color color;

    #region movement
    [Header("Movement properties")]
    public int movementAmount = 0;
    #endregion

    #region attack
    [Header("Attack properties")]
    public int damageAmount = 0;
    public enum AttackTarget
    {
        All, notMoving, Moving
    }
    public AttackTarget attackTarget;
    public static int ATTACK_NO_RANGE = -1;
    public int attackMaxRange = ATTACK_NO_RANGE;
    #endregion

    #region Heal
    [Header("Heal properties")]
    public int healAmount = 0;
    public bool recoverCard = false;
    #endregion

}
