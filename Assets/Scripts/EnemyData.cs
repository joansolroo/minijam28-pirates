using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "enemyData", menuName = "Cards/enemy", order = 1)]
public class EnemyData : ScriptableObject
{
    [Header("Lore")]
    public string name;
    public Sprite ship;
    public Sprite face;
    public List<string> dialog;

    [Header("Cards")]
    public List<CardRule> deck;
    public List<int> preference;

    [Header("Cards")]
    public CardRule option1;
    public CardRule option2;

    [Header("Life")]
    public int maxLife;
    public int startLife;
}
