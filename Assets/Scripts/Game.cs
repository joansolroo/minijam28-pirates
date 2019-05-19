using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] Fight fight;
    [SerializeField] UIIntroduction introductionScreen;
    [SerializeField] UIIntroduction wonScreen;
    [SerializeField] UIIntroduction looseScreen;

    Fight currentFight;

    [SerializeField] List<CardRule> playerDeck;
    [SerializeField] List<CardRule> enemyDeck;

    [SerializeField] Card option1;
    [SerializeField] Card option2;

    public void Start()
    {
        fight.gameObject.SetActive(false);
        introductionScreen.gameObject.SetActive(true);
        looseScreen.gameObject.SetActive(false);
        wonScreen.gameObject.SetActive(false);

        introductionScreen.Play();
    }

    void Update()
    {
        if (introductionScreen.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            LaunchFight(introductionScreen);
        }
    }
    public void LaunchFight(UIIntroduction previous)
    {
        previous.gameObject.SetActive(false);
        if (currentFight != null) Destroy(currentFight);
        currentFight = GameObject.Instantiate<Fight>(fight);
        currentFight.gameObject.SetActive(true);
        currentFight.Setup(playerDeck, enemyDeck);
      
    }
    public void WinFight()
    {
        currentFight.gameObject.SetActive(false);
        option1.rule = enemyDeck[Random.Range(0,enemyDeck.Count)];
        option1.GetComponent<CardRenderer>().UpdateLayout();
        option2.rule = enemyDeck[Random.Range(0, enemyDeck.Count)];
        option2.GetComponent<CardRenderer>().UpdateLayout();
        wonScreen.gameObject.SetActive(true);
        wonScreen.Play();
    }
    public void LoseFight()
    {
        currentFight.gameObject.SetActive(false);
        looseScreen.gameObject.SetActive(true);
        looseScreen.Play();
    }


    public void Equipe1()
    {
        Debug.Log("equipe card 1");
        playerDeck.Add(option1.rule);
        LaunchFight(wonScreen);
    }
    public void Equipe2()
    {
        Debug.Log("equipe card 2");
        playerDeck.Add(option2.rule);
        LaunchFight(wonScreen);
    }
    public void Loose1()
    {
        Debug.Log("loose card 1");
        LaunchFight(looseScreen);
    }
    public void Loose2()
    {
        Debug.Log("loose card 2");
        LaunchFight(looseScreen);
    }
}
