using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour {

    [SerializeField] Player player;
    [SerializeField] SpriteRenderer[] hpCounter;

    [SerializeField] Color healthyColor;
    [SerializeField] Color hurtColor;

    int previousHP = -1;
    private void Start()
    {
        UpdateHP();
    }
    public void UpdateHP()
    {
        if(previousHP!= player.hp)
        {
            for(int h =0; h< hpCounter.Length;++h)
            {
                if(h>=player.maxHp)
                {
                    hpCounter[h].gameObject.SetActive(false);
                }
                else if(h<player.hp)
                {
                    hpCounter[h].color = healthyColor;
                }
                else
                {
                    hpCounter[h].color = hurtColor;
                }
            }
            previousHP = player.hp;
        }
    }
}
