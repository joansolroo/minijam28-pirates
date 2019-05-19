using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour {

    [SerializeField] Player player1;
    [SerializeField] Player player2;
    [SerializeField] EnemyController enemyController;
    [SerializeField] Map map;
    [SerializeField] Game game;

    [SerializeField] UnityEngine.UI.Button confirmButton;
    // Use this for initialization

    public static Fight current;
    public void Setup(List<CardRule> player1deck, List<CardRule> player2deck)
    {
        current = this;
        player1.deckbuild = player1deck;
        player1.RemakeDeck();
        player1.hp = player1.maxHp;
        player1.enemy = player2;
        player1.ship.MoveTo(0,false);

        player2.deckbuild = player2deck;
        player2.RemakeDeck();
        player2.hp = player2.maxHp;
        player2.enemy = player1;
        player2.ship.MoveTo(12,false);

        for (int c = 0; c < 3; ++c)
        {
            player1.Draw();
            player2.Draw();
        }
        BeginTurn();
    }

    private void Update()
    {
        if (confirmButton)
        {
            confirmButton.interactable = player1.selected.cards.Count>0;
            confirmButton.gameObject.SetActive(player1.selected.cards.Count > 0 && !isSolving);
        }
    }
    public void BeginTurn()
    {
       // Debug.Log("Turn begins");
        DummyAI();
    }

    public void OnResolveRequest()
    {
        if (player1.selected.cards.Count > 0)
        {
            StartCoroutine(SolveTurn());
        }
        else
        {
            Debug.Log("no card picked");
        }
    }

    public void ResolveAction()
    {
        
    }


    void DummyAI()
    {
        //  Debug.Log("Enemy movement");
        enemyController.SelectCard();
    }
    public bool isSolving = false;
    IEnumerator SolveTurn()
    {
        if (!isSolving)
        {
            isSolving = true;
            player2.RevealSelected();
            yield return new WaitForSeconds(0.5f);
            //Debug.Log("Solving turn");
            bool ship1moved = false;
            bool ship2moved = false;
            // move
            foreach (Card c1 in player1.selected.cards)
            {
                ship1moved |= c1.DoMove();
            }
            foreach (Card c2 in player2.selected.cards)
            {
                ship2moved |= c2.DoMove();
            }
            yield return new WaitForSeconds(0.5f);
            bool ship1hit = false;
            bool ship2hit = false;
            // attack
            foreach (Card c1 in player1.selected.cards)
            {
                ship1hit |= c1.DoAttack(ship2moved);                
            }
            if (ship1hit)
            {
                map.tiles[player2.ship.position].emission.Emit(Random.Range(60, 100));
            }
            foreach (Card c2 in player2.selected.cards)
            {
                ship2hit |= c2.DoAttack(ship1moved);
            }
            if (ship2hit)
            {
                map.tiles[player1.ship.position].emission.Emit(Random.Range(60, 100));
            }
            yield return new WaitForSeconds(1f);

            if (player1.hp <= 0)
            {
                game.LoseFight();
            }
            else if (player2.hp <= 0)
            {
                game.WinFight();
            }
            if (player1.ship.position > player2.ship.position)
            {
                game.DrawFight();
            }
            // repair
            foreach (Card c1 in player1.selected.cards)
            {
                c1.DoRepair();
            }
            foreach (Card c2 in player2.selected.cards)
            {
                c2.DoRepair();
            }
            yield return new WaitForSeconds(0.5f);

            player1.DiscardSelected();
            player2.DiscardSelected();
            yield return new WaitForSeconds(0.5f);

            player1.Draw();
            player2.Draw();
            yield return new WaitForSeconds(0.5f);
            BeginTurn();

            isSolving = false;
        }
        
    }
}
