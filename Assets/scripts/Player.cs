using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName;   
    public int score;
    public int health;
    public bool isPlaying;
    public int player;

    private GameObject lifeObject;
    private HealthDiamond life;
    private HealthDiamond firstHealthDiamond;
    private HealthDiamond secondHealthDiamond;

    private GameObject deckObject;
    private Deck deck;

    void Awake()
    {
        lifeObject = GameObject.Find("HealthDiamond");
        life = lifeObject.GetComponent<HealthDiamond>();

        // 创造两个显示生命宝石实例
        firstHealthDiamond = Instantiate(life) as HealthDiamond;
        secondHealthDiamond = Instantiate(life) as HealthDiamond;

        // 设置firstHealthDiamond与secondHealthDiamond
        Debug.Log("Player creating - P" + player);
        if (player == (int)WhichPlayer.PLAYER2)
        {
            firstHealthDiamond.moveTo(-8.05f, 2.26f);
            secondHealthDiamond.moveTo(-7.31f, 2.26f);
        }
        else
        {
            firstHealthDiamond.moveTo(-8.05f, -1.93f);
            secondHealthDiamond.moveTo(-7.31f, -1.93f);
        }

        //显示生命宝石实例
        firstHealthDiamond.enableSprite();
        secondHealthDiamond.enableSprite();

        string objectGameName = this.gameObject.name + "Deck";
        deckObject = GameObject.Find(objectGameName);
        deck = deckObject.GetComponent<Deck>();
        health = 2;
        score = 0;
        isPlaying = true;
        if (this.gameObject.name.Equals("Player1"))
            player = (int)WhichPlayer.PLAYER1;
        else
            player = (int)WhichPlayer.PLAYER2;
    }

    void Update()
    {
        //每一帧更新一次玩家对战分数
        this.score = deck.getPowerSum(0);
    }

    /// <summary>
    /// 更新玩家生命宝石
    /// </summary>
    public void updateHealthDiamonds()
    {
        if (health == 1)
            //secondHealthDiamond.setVisibility(false);
            firstHealthDiamond.setVisibility(false);
        else if (health == 0 || health == -1)
            // firstHealthDiamond.setVisibility(false);
            secondHealthDiamond.setVisibility(false);
    }

    /// <summary>
    /// 获取生命宝石实例对象
    /// </summary>
    /// <param name="whichHealth">数字表示我们想要获得的哪个生命宝石</param>
    /// <returns>生命宝石实例对象</returns>
    public HealthDiamond getHealthDiamond(int whichHealth)
    {
        if (whichHealth == 1)
            return firstHealthDiamond;
        else
            return secondHealthDiamond;
    }

    /// <summary>
    /// 将卡片从每组的战场移动到墓地
    /// </summary>
    /// <param name="activePlayerNumber">当前是哪个玩家 is playing</param>
    public void moveCardsFromDeskToDeathArea(int activePlayerNumber)
    {
        //Vector3 player1DeathAreaVector = new Vector3(8.41f, -4.7f, -0.1f);
        Vector3 player1DeathAreaVector = new Vector3(8.51f, -4.57f, -0.1f);
        Vector3 player2DeathAreaVector = new Vector3(8.51f, 4.57f, -0.1f);

        float count = 0.0f;
        foreach (Card card in deck.getDeathCards())
        {
            if (player == (int)WhichPlayer.PLAYER2)
            {
                card.transform.position = player1DeathAreaVector;
                //上方，绝对值越来越大
                if (activePlayerNumber == (int)WhichPlayer.PLAYER1)
                 {
                     float x = card.transform.position.x + count / 25.0f;
                    float y = card.transform.position.y - count / 25.0f;
                     float z = card.transform.position.z - count / 50.0f;

                    count++;
                     card.transform.position = new Vector3(x, y * -1f , z);
                 }
                //下方，绝对值越来越小
                else
                {
                    float x = card.transform.position.x + count / 25.0f;
                    float y = card.transform.position.y +count / 25.0f;
                    float z = card.transform.position.z - count / 50.0f;
                    count++;
                    card.transform.position = new Vector3(x, y , z);
                }
            }
            else
            {
                card.transform.position = player2DeathAreaVector;
                //下方，绝对值越来越小
                if (activePlayerNumber == (int)WhichPlayer.PLAYER1)
                {
                    float x = card.transform.position.x + count / 25.0f;
                    float y = card.transform.position.y - count / 25.0f;
                    float z = card.transform.position.z - count / 50.0f;
                    count++;

                    card.transform.position = new Vector3(x, y * -1f, z);
                }
                //上方，绝对值越来越大
                else
                {
                    float x = card.transform.position.x + count / 25.0f;
                    float y = card.transform.position.y + count / 25.0f;
                    float z = card.transform.position.z - count / 50.0f;
                    count++;
                    card.transform.position = new Vector3(x, y, z);
                }
            }
        }
    }

    /// <summary>
    /// Reloading player's deck
    /// </summary>
    public void reloadDeck()
    {
        string objectGameName = this.gameObject.name + "Deck";
        deckObject = GameObject.Find(objectGameName);
        deck = deckObject.GetComponent<Deck>();
    }

    /// <summary>
    /// Get player's name
    /// </summary>
    /// <returns>plyer's name</returns>
    public string getPlayerName()
    {
        return this.playerName;
    }

    /// <summary>
    /// Set player's name
    /// </summary>
    /// <param name="name">player's new name</param>
    public void setPlayerName(string playerName)
    {
        this.playerName = playerName;
    }

    /// <summary>
    /// Get player's score
    /// </summary>
    /// <returns>player's score</returns>
    public int getScore()
    {
        return this.score;
    }

    /// <summary>
    /// Set plyer's score
    /// </summary>
    /// <param name="score">player's new score</param>
    public void setScore(int score)
    {
        this.score = score;
    }

    /// <summary>
    /// Get player's health
    /// </summary>
    /// <returns></returns>
    public int getHealth()
    {
        return this.health;
    }

    /// <summary>
    /// Set player's health
    /// </summary>
    /// <param name="health">player's new health</param>
    public void setHealth(int health)
    {
        this.health = health;
    }

    /// <summary>
    /// Get player's deck
    /// </summary>
    /// <returns>player's deck</returns>
    public Deck getDeck()
    {
        return this.deck;
    }

    /// <summary>
    /// Set visibility of cards in deck
    /// </summary>
    /// <param name="visibility">Player双方的手牌上下移动，以至于显示一方手牌，隐藏另一方手牌</param>
    public void setDeckVisibility(bool visibility)
    {
        Debug.Log("setDeckVisibility()");
        float value = -10;

        if (visibility == true)
            value = -5.35f;
                
        foreach (Card card in deck.getCards())
        {
            card.transform.position = new Vector3(card.transform.position.x, value, card.transform.position.z);
        }
    }

    /// <summary>
    /// Updating player's score
    /// </summary>
    public void updateScore()
    {
        this.score = deck.getPowerSum(0);
    }

    /// <summary>
    /// Indicate player
    /// </summary>
    private enum WhichPlayer { PLAYER1 = 1, PLAYER2 };
}
