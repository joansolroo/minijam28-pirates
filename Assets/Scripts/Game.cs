using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] Fight fight;
    [SerializeField] UIIntroduction introductionScreen;
    [SerializeField] UIIntroduction wonScreen;
    [SerializeField] UIIntroduction looseScreen;
    [SerializeField] UIIntroduction introduceEnnemy;

    Fight currentFight;

    [SerializeField] List<CardRule> playerDeck;
    [SerializeField] List<CardRule> enemyDeck;

    [SerializeField] Card option1;
    [SerializeField] Card option2;

    [SerializeField] EnemyData[] ennemies;
    private int currentEnnemy;

    public void Start()
    {
        fight.gameObject.SetActive(false);
        introductionScreen.gameObject.SetActive(true);
        looseScreen.gameObject.SetActive(false);
        wonScreen.gameObject.SetActive(false);

        introductionScreen.Play();
        currentEnnemy = 0;
    }

    void Update()
    {
        if (introductionScreen.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            LaunchEnnemyIntruduction(introductionScreen);
        }
    }
    public void LaunchFight(UIIntroduction previous)
    {
        previous.gameObject.SetActive(false);
        if (currentFight != null) Destroy(currentFight);
        currentFight = GameObject.Instantiate<Fight>(fight);
        currentFight.gameObject.SetActive(true);
        currentFight.Setup(playerDeck, ennemies[currentEnnemy]);
      
    }
    public void WinFight()
    {
        currentFight.gameObject.SetActive(false);
        option1.rule = ennemies[currentEnnemy].option1;
        option1.GetComponent<CardRenderer>().UpdateLayout();
        option2.rule = ennemies[currentEnnemy].option2;
        option2.GetComponent<CardRenderer>().UpdateLayout();
        wonScreen.gameObject.SetActive(true);
        wonScreen.Play();
        currentEnnemy++;
    }

    public void DrawFight()
    {
        Debug.LogWarning("this is the win screen, but it was a draw");
        WinFight();
    }
   
    public void LoseFight()
    {
        currentFight.gameObject.SetActive(false);
        looseScreen.gameObject.SetActive(true);
        looseScreen.Play();
        currentEnnemy++;
    }


    public void Equipe1()
    {
        Debug.Log("equipe card 1");
        playerDeck.Add(option1.rule);
        LaunchEnnemyIntruduction(wonScreen);
    }
    public void Equipe2()
    {
        Debug.Log("equipe card 2");
        playerDeck.Add(option2.rule);
        LaunchEnnemyIntruduction(wonScreen);
    }
    public void Loose1()
    {
        Debug.Log("loose card 1");
        LaunchEnnemyIntruduction(looseScreen);
    }
    public void Loose2()
    {
        Debug.Log("loose card 2");
        LaunchEnnemyIntruduction(looseScreen);
    }
    public void LaunchEnnemyIntruduction(UIIntroduction previous)
    {
        previous.gameObject.SetActive(false);
        introduceEnnemy.gameObject.SetActive(true);
        introduceEnnemy.gameObject.GetComponent<IntroduceEnnemy>().Setup(ennemies[currentEnnemy]);
    }
}
