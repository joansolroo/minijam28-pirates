using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cardRule", menuName = "Cards/rule", order = 1)]
public class CardRule : ScriptableObject {

    public string name;
    [TextArea(5, 5)]
    public string description;

    public Sprite art;
    public Color color;

    #region movement
    [Header("Movement properties")]
    public int movementAmount = 0;
    public AudioClip moveClip;
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
    public AudioClip attackClip;
    #endregion

    #region Heal
    [Header("Heal properties")]
    public int healAmount = 0;
    public bool recoverCard = false;
    public AudioClip healClip;
    #endregion

    #region Special
    [Header("Special properties")]
    public bool boarding = false;
    public AudioClip boardClip;
    public bool scan = false;
    public AudioClip scanClip;
    #endregion


    public AudioClip explosionClip;
}
