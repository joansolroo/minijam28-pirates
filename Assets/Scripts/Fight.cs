using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class Fight : MonoBehaviour
{

    [SerializeField] Player player1;
    [SerializeField] Player player2;
    [SerializeField] EnemyController enemyController;
    [SerializeField] Map map;
    [SerializeField] Game game;

    [SerializeField] Button confirmButton;
    [SerializeField] Text Resolution0;
    [SerializeField] Text Resolution1;
    [SerializeField] Text Resolution2;
    [SerializeField] Text Resolution3;
    [SerializeField] Text Resolution4;

    [SerializeField] PreviewAction preview1;
    [SerializeField] PreviewAction preview2;
    // Use this for initialization

    public static Fight current;
    public void Setup(List<CardRule> player1deck, EnemyData enemy)
    {
        fightOver = false;
        current = this;
        player1.deckbuild = player1deck;
        player1.RemakeDeck();
        player1.hp = player1.maxHp;
        player1.enemy = player2;
        player1.ship.MoveTo(0, false);

        enemyController.data = enemy;
        player2.deckbuild = enemy.deck;
        player2.faction = enemy.face;
        player2.ship.shipRenderer[0].sprite = enemy.ship;
        player2.RemakeDeck();
        player2.hp = enemy.startLife;
        player2.maxHp = enemy.maxLife;
        player2.enemy = player1;
        player2.ship.MoveTo(12, false);

        StartCoroutine(DoSetup());
    }

    private void Update()
    {
        if (confirmButton)
        {
            confirmButton.interactable = player1.selected.cards.Count > 0 && player2.selected.cards.Count > 0 && !isSolving;
            confirmButton.gameObject.SetActive(player1.selected.cards.Count > 0);
        }
    }
    public void BeginTurn()
    {
        // Debug.Log("Turn begins");
        DummyAI();
        Resolution0.gameObject.SetActive(false);
        Resolution1.gameObject.SetActive(false);
        Resolution2.gameObject.SetActive(false);
        Resolution3.gameObject.SetActive(false);
        Resolution4.gameObject.SetActive(false);
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


    bool fightOver = false;
    IEnumerator DoSetup()
    {
        isSolving = true;
        yield return new WaitForSeconds(0.5f);
        for (int c = 0; c < 3; ++c)
        {
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
            player1.Draw();
            yield return new WaitForSeconds(Random.Range(0, 0.05f));
            player2.Draw();
        }
        yield return new WaitForSeconds(1f);
        BeginTurn();
        isSolving = false;
    }
    IEnumerator SolveTurn()
    {
        if (!isSolving)
        {
            Resolution0.gameObject.SetActive(true);

            isSolving = true;
            player2.RevealSelected();
            yield return new WaitForSeconds(0.5f);

            bool ship1hit = false;
            bool ship2hit = false;
            // attack

            int pos1 = player1.ship.position;
            int pos2 = player2.ship.position;
            Resolution1.gameObject.SetActive(true);
            preview1.DoPreviewAction(1, pos1,pos2);
            preview2.DoPreviewAction(1, pos2, pos1);
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
            if (ship1moved || ship2moved)
            {
                yield return new WaitForSeconds(0.5f);
            }

            Resolution2.gameObject.SetActive(true);
            preview1.DoPreviewAction(0, pos1, pos2);
            preview2.DoPreviewAction(0, pos2, pos1);
            yield return new WaitForSeconds(0.5f);
            if (!ship2moved)
            {
                foreach (Card c1 in player1.selected.cards)
                {
                    ship1hit |= c1.DoAttackNotMoved(ship2moved, pos1, pos2);
                }
            }
            if (!ship1moved)
            {
                foreach (Card c2 in player2.selected.cards)
                {
                    ship2hit |= c2.DoAttackNotMoved(ship1moved, pos2, pos1);
                }

            }/*
            if (ship1hit)
            {
                ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                parameter.startColor = player2.color;
                map.tiles[pos2].emission.Emit(parameter, Random.Range(10, 20));
            }
            
            if (ship2hit)
            {
                ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                parameter.startColor = player1.color;
                map.tiles[pos1].emission.Emit(parameter, Random.Range(20, 50));
            }*/
            
            if (ship1hit || ship2hit)
            {
                yield return new WaitForSeconds(1);
            }


            Resolution3.gameObject.SetActive(true);
            preview1.DoPreviewAction(2, pos1, pos2);
            preview2.DoPreviewAction(2, pos2, pos1);
            yield return new WaitForSeconds(0.5f);
            bool dynamicHit = false;
            if (!ship1hit)
            {
                foreach (Card c1 in player1.selected.cards)
                {
                    ship1hit |= c1.DoAttackMoved(ship2moved);
                }
                dynamicHit |= ship1hit;
                /*
                if (ship1hit)
                {
                    ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                    parameter.startColor = player2.color;
                    map.tiles[player2.ship.position].emission.Emit(parameter, Random.Range(20, 50));
                    dynamicHit = true;
                }*/
            }
            if (!ship2hit)
            {
                foreach (Card c2 in player2.selected.cards)
                {
                    ship2hit |= c2.DoAttackMoved(ship1moved);
                }
                dynamicHit |= ship2hit;
                /*
                if (ship2hit)
                {
                    ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                    parameter.startColor = player1.color;
                    map.tiles[player1.ship.position].emission.Emit(parameter, Random.Range(20, 50));
                    dynamicHit = true;
                }*/
            }
            if (dynamicHit)
            {
                yield return new WaitForSeconds(1);
            }

            if (player1.hp <= 0)
            {
                player1.ship.gameObject.SetActive(false);

                yield return new WaitForSeconds(1f);
                if (!fightOver)
                {
                    game.LoseFight();
                    fightOver = true;
                }
            }
            else if (player2.hp <= 0)
            {
                player2.ship.gameObject.SetActive(false);
                yield return new WaitForSeconds(1f);
                if (!fightOver)
                {
                    game.WinFight();
                    fightOver = true;
                }
            }
            if (!fightOver)
            {
                Resolution4.gameObject.SetActive(true);
                preview1.DoPreviewAction(3, pos1, pos2);
                preview2.DoPreviewAction(3, pos2, pos1);
                yield return new WaitForSeconds(1f);

                // repair
                foreach (Card c1 in player1.selected.cards)
                {
                    c1.DoSpecial();
                }
                foreach (Card c2 in player2.selected.cards)
                {
                    c2.DoSpecial();
                }
                yield return new WaitForSeconds(0.5f);

                if (player1.ship.position > player2.ship.position)
                {
                    if (!fightOver)
                    {
                        game.DrawFight();
                        fightOver = true;
                    }
                }
                player1.DiscardSelected();
                player2.DiscardSelected();
                yield return new WaitForSeconds(0.5f);

                player1.Draw();
                player2.Draw();
                yield return new WaitForSeconds(0.5f);
                BeginTurn();
            }
            isSolving = false;
        }
    }

    public void OnBoard(Player source, Player target)
    {
        if (!fightOver)
        {
            if (source == player1)
            {
                game.WinFight();
            }
            else
            {
                game.LoseFight();
            }
            fightOver = true;
        }
    }
}
