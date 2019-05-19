using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickBonus : MonoBehaviour {

    [SerializeField] Player player;
    [SerializeField] Card card1;
    [SerializeField] Card card2;


    private void Awake()
    {
        this.gameObject.SetActive(false);
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void OnOption1()
    {
        player.deck.AddCardLast(card1);
        Hide();
    }

    public void OnOption2()
    {
        player.deck.AddCardLast(card2);
        Hide();

    }
}
