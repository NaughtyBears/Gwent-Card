using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Game : MonoBehaviour
{
    public static Game _instance; //单列
    public GameObject fogWeatherUp;
    public GameObject frostWeatherUp;
    public GameObject rainWeatherUp;
    public GameObject fogWeatherDown;
    public GameObject frostWeatherDown;
    public GameObject rainWeatherDown;

    private Card activeCard;
    private Card activeShowingCard;
    private static int activePlayerNumber;
    private static int state = (int)Status.FREE;
    private static int gameStatus = (int)GameStatus.TOUR1;

    private GameObject deckObject;
    private GameObject deskObject;
    private GameObject areasObject;

    private Deck activeDeck;
    private Desk desk;
    private Areas areas;

    private GameObject playerDownNameTextObject;
    private Text playerDownNameText;

    private GameObject playerUpNameTextObject;
    private Text playerUpNameText;

    private GameObject player1Object;
    private GameObject player2Object;
    private Player player1;
    private Player player2;


    private GameObject score1Object;
    private GameObject score2Object;
    private GameObject score3Object;
    private GameObject score4Object;
    private GameObject score5Object;
    private GameObject score6Object;
    private GameObject score7Object;
    private GameObject score8Object;
    private GameObject cardNumber1Object;
    private GameObject cardNumber2Object;
    private Text score1Text;
    private Text score2Text;
    private Text score3Text;
    private Text score4Text;
    private Text score5Text;
    private Text score6Text;
    private Text score7Text;
    private Text score8Text;
    private Text cardNumber1;
    private Text cardNumber2;

    private GameObject buttonObject;
    private Button button;

    public GameObject endPanelObject;
    public GameObject endTextObject;
    public Text endText;

    private GameObject giveUpButtonObject;

    void Awake()
    {
        _instance = this;
        player1Object = GameObject.Find("Player1");
        player2Object = GameObject.Find("Player2");
        player1 = player1Object.GetComponent<Player>();
        player2 = player2Object.GetComponent<Player>();

        deskObject = GameObject.Find("Desk");
        desk = deskObject.GetComponent<Desk>();

        areasObject = GameObject.Find("Areas");
        areas = areasObject.GetComponent<Areas>();

        playerDownNameTextObject = GameObject.Find("DownPlayerName");
        playerDownNameText = playerDownNameTextObject.GetComponent<Text>();

        playerUpNameTextObject = GameObject.Find("UpPlayerName");
        playerUpNameText = playerUpNameTextObject.GetComponent<Text>();


        fogWeatherUp = GameObject.Find("fogWeatherUp");
        fogWeatherUp.SetActive(false);

        frostWeatherUp = GameObject.Find("frostWeatherUp");
        frostWeatherUp.SetActive(false);

        rainWeatherUp = GameObject.Find("rainWeatherUp");
        rainWeatherUp.SetActive(false);

        fogWeatherDown = GameObject.Find("fogWeatherDown");
        fogWeatherDown.SetActive(false);

        frostWeatherDown = GameObject.Find("frostWeatherDown");
        frostWeatherDown.SetActive(false);

        rainWeatherDown = GameObject.Find("rainWeatherDown");
        rainWeatherDown.SetActive(false);

        score1Object = GameObject.Find("Score1");
        score2Object = GameObject.Find("Score2");
        score3Object = GameObject.Find("Score3");
        score4Object = GameObject.Find("Score4");
        score5Object = GameObject.Find("Score5");
        score6Object = GameObject.Find("Score6");
        score7Object = GameObject.Find("Score7");
        score8Object = GameObject.Find("Score8");
        cardNumber1Object = GameObject.Find("cardNumber1");
        cardNumber2Object = GameObject.Find("cardNumber2");
        score1Text = score1Object.GetComponent<Text>();
        score2Text = score2Object.GetComponent<Text>();
        score3Text = score3Object.GetComponent<Text>();
        score4Text = score4Object.GetComponent<Text>();
        score5Text = score5Object.GetComponent<Text>();
        score6Text = score6Object.GetComponent<Text>();
        score7Text = score7Object.GetComponent<Text>();
        score8Text = score8Object.GetComponent<Text>();
        cardNumber1 = cardNumber1Object.GetComponent<Text>();
        cardNumber2 = cardNumber2Object.GetComponent<Text>();

        buttonObject = GameObject.Find("Button");
        button = buttonObject.GetComponent<Button>();

        endPanelObject = GameObject.FindGameObjectWithTag("EndPanel");
        endPanelObject.transform.position = new Vector3(0, 0, -1.8f);
        endTextObject = GameObject.FindGameObjectWithTag("EndText");
        endText = endTextObject.GetComponent<Text>();

        giveUpButtonObject = GameObject.FindGameObjectWithTag("giveUpButton");
        giveUpButtonObject.SetActive(true);

        endPanelObject.SetActive(false);

        activePlayerNumber = (int)PlayerNumber.PLAYER1;
        gameStatus = (int)GameStatus.TOUR1;
    }

    void Start()
    {

        player1.getDeck().buildDeck(10);
        player2.getDeck().buildDeck(10);

        activePlayerNumber = (int)PlayerNumber.PLAYER1;

        initializePlayersDecks();
    }

    void initializePlayersDecks()
    {
        //移除当前回合战场上的牌到墓地List中
        player1.getDeck().sendCardsToDeathList();
        player2.getDeck().sendCardsToDeathList();

        //清除战场牌并移动到墓地框中
        player1.moveCardsFromDeskToDeathArea(activePlayerNumber);
        player2.moveCardsFromDeskToDeathArea(activePlayerNumber);

        //Debug.Log("Deleted " + deleteAllCardClones() + " cards");

        // player1.reloadDeck();
        //player2.reloadDeck();

        Debug.Log("P1 amount of cards: " + player1.getDeck().cardsInDeck.Count);
        Debug.Log("P2 amount of cards: " + player2.getDeck().cardsInDeck.Count);

        if(gameStatus == (int)GameStatus.TOUR1)
            player2.setDeckVisibility(false);

        activeDeck = player1.getDeck();

        if (player1.getDeck().cardsInDeck.Count > 0)
            activeCard = player1.getDeck().cardsInDeck[0];

        activeShowingCard = Instantiate(activeCard) as Card;
        activeShowingCard.transform.position = new Vector3(8.96f, 0, -0.1f);
        showActiveCard(false);

        if (player1.score > player2.score && activePlayerNumber == (int)PlayerNumber.PLAYER2 && player1.getDeck().cardsInDeck.Count != 0)
            switchPlayer();
        else if (player1.score < player2.score && player2.getDeck().cardsInDeck.Count != 0)
            switchPlayer();
        //防止出现手牌为空是进入次回合的BUG(BUG内容：手牌互换)
        else if (player1.score > player2.score && activePlayerNumber == (int)PlayerNumber.PLAYER1 && player1.getDeck().cardsInDeck.Count == 0)
            switchPlayer();
        else if (player1.score < player2.score  && player2.getDeck().cardsInDeck.Count == 0)
            switchPlayer();

        reorganizeGroup();
    }

    /// <summary>
    /// Game的状态枚举
    /// </summary>
    private enum Status
    {
        /// <summary>
        /// 出牌阶段
        /// </summary>
        FREE,
        /// <summary>
        /// 手牌被选中阶段
        /// </summary>
        ACTIVE_CARD,
        /// <summary>
        /// 当前玩家此轮结束
        /// </summary>
        BLOCKED 
    };

    /// <summary>
    /// Delete all clone cards
    /// </summary>
    /// <returns>Number of deleted cards</returns>
    /*public int deleteAllCardClones()
    {
        int cloneNumber = 0;
        GameObject[] cloneCards = GameObject.FindGameObjectsWithTag("CloneCard");
        cloneNumber = cloneCards.Length;

        foreach (GameObject go in cloneCards)
            GameObject.DestroyObject(go);

        return cloneNumber;
    }*/

    void Update()
    {
        // 更新 numberOfCards
        // ---------------------------------------------------------------------------------------------------------------
        cardNumber1.text = player1.getDeck().cardsInDeck.Count.ToString();
        cardNumber2.text = player2.getDeck().cardsInDeck.Count.ToString();

        // 选取卡牌
        // ---------------------------------------------------------------------------------------------------------------
        /*
            屏幕空间是用像素定义的。屏幕左下方为(0,0);右上角是(pixelWidth,pixelHeight)
            Input.mousePosition 返回值为：当前鼠标在像素坐标中的位置
            mouseRelativePosition：鼠标的2D位置转换为3D位置
            
        PS: 使用ScreenToWorldPoint(Vector3 position)时，如果将z坐标设置为零的话，那么转换的结果可能是出错，会变成摄像机的位置
         */
        Vector3 mouseRelativePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("mousePosition: " + Input.mousePosition + "mouseRelativePosition: " + mouseRelativePosition);

        mouseRelativePosition.z = -0.1f;
        //Debug.Log("new mouseRelativePosition: " + mouseRelativePosition);
        // Debug.Log("-------------------------------------------------------------------------");
        if (Input.GetMouseButtonDown(0) && state != ((int)Status.BLOCKED))
        {
            // 当点击了 area设置好的 collision后
            if (areas.getDeckColliderBounds().Contains(mouseRelativePosition) && activeDeck.cardsInDeck.Count > 0)
            {
                foreach (Card c in activeDeck.getCards())
                {
                    // 当点击了手牌中的卡片时
                    if (c.getBounds().Contains(mouseRelativePosition))
                    {
                        activeDeck.disactiveAllInDeck();
                        activeCard = c;
                        c.setActive(true);

                        showActiveCard(true);

                        activeCard.transform.position += new Vector3(0, 0.15f, 0);
                        state = (int)Status.ACTIVE_CARD;
                    }
                }
            }
            // 诱饵牌替换近程牌
            if (areas.getSwordColliderBounds().Contains(mouseRelativePosition) && state == (int)Status.ACTIVE_CARD && activeCard.getIsSpecial() == (int)TypeOfCard.MANEKIN)
            {
                foreach (Card c in activeDeck.getSwordCards())
                {
                    if (c.getBounds().Contains(mouseRelativePosition) && c.getIsSpecial() != (int)TypeOfCard.GOLD && c.getIsSpecial() != (int)TypeOfCard.GOLD_SPY && c.getIsSpecial() != (int)TypeOfCard.MANEKIN)
                    {
                        Debug.Log("Manekin target!");
                        activeCard.setActive(false);
                        activeDeck.moveCardToDeckFromSwords(c);
                        if (activeDeck.addCardToSwords(activeCard) == true)
                        {
                            activeDeck.disactiveAllInDeck();
                            state = (int)Status.FREE;

                            if (player1.isPlaying && player2.isPlaying)
                            {
                                Debug.Log("switchPlayer()");
                                switchPlayer();
                            }
                            else
                            {
                                reorganizeGroup();
                                state = (int)Status.FREE;
                                showActiveCard(false);
                            }
                        }
                        break;
                    }
                }
            }
            // 诱饵牌替换远程牌
            else if (areas.getBowColliderBounds().Contains(mouseRelativePosition) && state == (int)Status.ACTIVE_CARD && activeCard.getIsSpecial() == (int)TypeOfCard.MANEKIN)
            {
                foreach (Card c in activeDeck.getBowCards())
                {
                    if (c.getBounds().Contains(mouseRelativePosition) && c.getIsSpecial() != (int)TypeOfCard.GOLD && c.getIsSpecial() != (int)TypeOfCard.GOLD_SPY && c.getIsSpecial() != (int)TypeOfCard.MANEKIN)
                    {
                        activeCard.setActive(false);
                        activeDeck.moveCardToDeckFromBows(c);
                        if (activeDeck.addCardToBows(activeCard) == true)
                        {
                            activeDeck.disactiveAllInDeck();
                            state = (int)Status.FREE;

                            if (player1.isPlaying && player2.isPlaying)
                            {
                                Debug.Log("switchPlayer()");
                                switchPlayer();
                            }
                            else
                            {
                                reorganizeGroup();
                                state = (int)Status.FREE;
                                showActiveCard(false);
                            }
                        }
                        break;
                    }
                }
            }
            // 诱饵牌替换攻城牌
            else if (areas.getTrebuchetColliderBounds().Contains(mouseRelativePosition) && state == (int)Status.ACTIVE_CARD && activeCard.getIsSpecial() == (int)TypeOfCard.MANEKIN)
            {
                foreach (Card c in activeDeck.getTrebuchetCards())
                {
                    if (c.getBounds().Contains(mouseRelativePosition) && c.getIsSpecial() != (int)TypeOfCard.GOLD && c.getIsSpecial() != (int)TypeOfCard.GOLD_SPY && c.getIsSpecial() != (int)TypeOfCard.MANEKIN)
                    {
                        activeCard.setActive(false);
                        activeDeck.moveCardToDeckFromTrebuchets(c);
                        if (activeDeck.addCardToTrebuchets(activeCard) == true)
                        {
                            activeDeck.disactiveAllInDeck();
                            state = (int)Status.FREE;

                            if (player1.isPlaying && player2.isPlaying)
                            {
                                Debug.Log("switchPlayer()");
                                switchPlayer();
                            }
                            else
                            {
                                reorganizeGroup();
                                state = (int)Status.FREE;
                                showActiveCard(false);
                            }
                        }
                        break;
                    }
                }
            }
            // 点击卡牌并点击到近战组
            else if (areas.getSwordColliderBounds().Contains(mouseRelativePosition))
            {
                if (state == (int)Status.ACTIVE_CARD && activeCard.getGroup() == (int)CardGroup.SWORD && activeCard.getIsSpecial() != (int)TypeOfCard.SPY && activeCard.getIsSpecial() != (int)TypeOfCard.GOLD_SPY )
                {
                    showActiveCard(false);
                    if (activeDeck.addCardToSwords(activeCard) == true)
                    {
                        // TODO - system 牌组管理
                        activeCard.setActive(false);
                        activeDeck.disactiveAllInDeck();
                        state = (int)Status.FREE;

                        if (player1.isPlaying && player2.isPlaying)
                            switchPlayer();
                        else
                        {
                            reorganizeGroup();
                            state = (int)Status.FREE;
                            //showActiveCard(false);
                        }
                    }
                }
            }
            else if (areas.getBowColliderBounds().Contains(mouseRelativePosition))
            {
                if (state == (int)Status.ACTIVE_CARD && activeCard.getGroup() == (int)CardGroup.BOW)
                {
                    // TODO - system 牌组管理
                    activeCard.setActive(false);
                    if (activeDeck.addCardToBows(activeCard) == true)
                    {
                        activeDeck.disactiveAllInDeck();
                        state = (int)Status.FREE;
                        if (player1.isPlaying && player2.isPlaying)
                            switchPlayer();
                        else
                        {
                            reorganizeGroup();
                            state = (int)Status.FREE;
                            showActiveCard(false);
                        }
                    }
                }
            }
            else if (areas.getTrebuchetColliderBounds().Contains(mouseRelativePosition))
            {
                if (state == (int)Status.ACTIVE_CARD && activeCard.getGroup() == (int)CardGroup.TREBUCHET)
                {
                    // TODO - system 牌组管理
                    activeCard.setActive(false);
                    if (activeDeck.addCardToTrebuchets(activeCard) == true)
                    {
                        activeDeck.disactiveAllInDeck();
                        state = (int)Status.FREE;
                        if (player1.isPlaying && player2.isPlaying)
                            switchPlayer();
                        else
                        {
                            reorganizeGroup();
                            state = (int)Status.FREE;
                            showActiveCard(false);
                        }
                    }
                }
            }
            // 放置间谍牌
            else if (areas.getSword2ColliderBounds().Contains(mouseRelativePosition))
            {
                // 条件：间谍牌/黄金间谍牌被选中且被选中的间谍牌是近战类型，则满足当前if条件
                if (state == (int)Status.ACTIVE_CARD && (activeCard.getIsSpecial() == (int)TypeOfCard.SPY || activeCard.getIsSpecial() == (int)TypeOfCard.GOLD_SPY) && activeCard.getGroup() == (int)CardGroup.SWORD)
                {
                    // TODO - system 牌组管理

                    if (activePlayerNumber == (int)PlayerNumber.PLAYER1)
                    {
                        activeCard.setActive(false);
                        player2.getDeck().addSpy(activeCard);
                        player1.getDeck().cardsInDeck.Remove(activeCard);
                        player1.getDeck().addTwoRandomCards();
                        activeDeck.disactiveAllInDeck();
                        state = (int)Status.FREE;
                        if (player1.isPlaying && player2.isPlaying)
                            switchPlayer();
                        else
                        {
                            reorganizeGroup();
                            state = (int)Status.FREE;
                            showActiveCard(false);
                        }
                    }
                    if(activePlayerNumber == (int)PlayerNumber.PLAYER2)
                    {
                        activeCard.setActive(false);  
                        player1.getDeck().addSpy(activeCard);
                        player2.getDeck().cardsInDeck.Remove(activeCard);
                        player2.getDeck().addTwoRandomCards();
                        activeDeck.disactiveAllInDeck();
                        state = (int)Status.FREE;
                        if (player1.isPlaying && player2.isPlaying)
                            switchPlayer();
                        else
                        {
                            reorganizeGroup();
                            state = (int)Status.FREE;
                            showActiveCard(false);
                        }
                    }
                }
                else
                {
                    activeDeck.disactiveAllInDeck();
                    activeCard = null;
                    showActiveCard(false);
                    state = (int)Status.FREE;
                }
            }
            else if (areas.getSpecial1ColliderBounds().Contains(mouseRelativePosition))
            {
                if (state == (int)Status.ACTIVE_CARD && (activeCard.getIsSpecial() == (int)TypeOfCard.DESTROY || activeCard.getIsSpecial() == (int)TypeOfCard.WEATHER))
                {
                    activeCard.setActive(false);
                    activeCard.transform.position = areas.getSpecial1CenterVector();
                    activeDeck.addToSpecial(activeCard);
                    // 烧灼
                    if (activeCard.getIsSpecial() == 4)
                    {
                        if (activePlayerNumber == (int)PlayerNumber.PLAYER1)
                        {
                            player2.getDeck().removeMaxPowerCard();
                            player1.getDeck().sendCardToDeathList(activeCard, activePlayerNumber);
                        }

                        else
                        {
                            player1.getDeck().removeMaxPowerCard();
                            player2.getDeck().sendCardToDeathList(activeCard, activePlayerNumber);
                        }
                        activeDeck.disactiveAllInDeck();
                        state = (int)Status.FREE;
                        if (player1.isPlaying && player2.isPlaying)
                            switchPlayer();
                        else
                        {
                            reorganizeGroup();
                            state = (int)Status.FREE;
                            showActiveCard(false);
                        }
                    }
                    // 天气牌
                    else if (activeCard.getIsSpecial() == 5)
                    {
                        int weatherCardRange = activeCard.getPower() * -1;
                        player1.getDeck().applyWeatherEffect(weatherCardRange);
                        player2.getDeck().applyWeatherEffect(weatherCardRange);


                        if (weatherCardRange == 4)
                        {
                            player1.getDeck().deleteFromSpecial();
                            player2.getDeck().deleteFromSpecial();
                        }

                        activeDeck.disactiveAllInDeck();
                        state = (int)Status.FREE;
                        if (player1.isPlaying && player2.isPlaying)
                            switchPlayer();
                        else
                        {
                            reorganizeGroup();
                            state = (int)Status.FREE;
                            showActiveCard(false);
                        }
                    }
                }
            }
            // 结束当前玩家回合
            // TODO - 修改当前玩家的 isPlaying
            // ---------------------------------------------------------------------------------------------------------------
            if (player1.getDeck().cardsInDeck.Count == 0 && player1.isPlaying)
            {
                Debug.Log("Player1 has no cards");
                player1.isPlaying = false;
            }
            if (player2.getDeck().cardsInDeck.Count == 0 && player2.isPlaying)
            {
                Debug.Log("Player2 has no cards");
                player2.isPlaying = false;
            }
            if (player1.isPlaying == false && player2.isPlaying == false && gameStatus != (int)GameStatus.END)
            {
                player1.updateScore();
                player2.updateScore();
                Debug.Log("Both players have no cards");
                Debug.Log("P1: " + player1.score + ", P2: " + player2.score);
                // 回合结束 - 检查谁赢了，扣去失败者的一点儿health并开启次回合
                if (player1.score > player2.score)
                {
                    Debug.Log("player1.score > player2.score");
                    if (player2.health > 0)
                    {
                        Debug.Log("player2.health > 0");
                        // Player 1 赢得此回合
                        endText.text = "玩家1赢了!!!";
                        player2.health--;
                        player2.updateHealthDiamonds();
                    }
                    if (player2.health == 0)
                    {
                        // Player 1 赢得比赛最终胜利
                        player2.health = -1;
                    }
                }
                else if (player1.score < player2.score)
                {
                    Debug.Log("player1.score <= player2.score");
                    if (player1.health > 0)
                    {
                        Debug.Log("player1.health > 0");
                        // Player 2 赢得当前回合
                        endText.text = "玩家2赢了!!!";
                        player1.health--;
                        player1.updateHealthDiamonds();
                    }
                    if (player1.health == 0)
                    {
                        // Player 2 赢得比赛
                        player1.health = -1;
                    }
                }
                else //平局情况，双方都扣去一点 health， 设的health等于0谁失败
                {
                    if (player1.health > 0)
                        player1.health--;
                    if (player2.health > 0)
                        player2.health--;
                    endText.text = "平局!";
                    if (player1.health == 0)
                        player1.health = -1;
                    if (player2.health == 0)
                        player2.health = -1;

                    player1.updateHealthDiamonds();
                    player2.updateHealthDiamonds();
                }

                // 游戏结束 GameOver
                if (player1.health == -1 && player2.health == -1)
                {
                    Debug.Log("平局!");
                    gameStatus = (int)GameStatus.END;

                    //activePlayerNumber = (int)PlayerNumber.PLAYER1;
                }
                else if (player1.health == -1)
                {
                    Debug.Log("P2 WON!");
                    gameStatus = (int)GameStatus.END;
                    // activePlayerNumber = (int)PlayerNumber.PLAYER1;
                }
                else if (player2.health == -1)
                {
                    Debug.Log("P1 WON!");
                    gameStatus = (int)GameStatus.END;
                    //  activePlayerNumber = (int)PlayerNumber.PLAYER2;
                }

                if (gameStatus != (int)GameStatus.END)
                {
                    Debug.Log("回合结束");
                    endTour();
                }
                else
                {
                    gameOver();

                    //Debug.Log("Deleting!");
                    // deleteAllCardClones();
                }
            }
        }
    }

    private void gameOver()
    {
        endPanelObject.SetActive(true);
        giveUpButtonObject.SetActive(false);
        StartCoroutine(GameOverScreen(2f));
    }

    IEnumerator GameOverScreen(float duration)
    {
        yield return new WaitForSeconds(duration);

        // after
        endText.text = "游戏结束\n";
        if (player1.health == -1 && player2.health == -1)
            endText.text += "\n平局";
        else if (player2.health == -1)
            endText.text += "\n玩家1 赢了!!!";
        else if (player1.health == -1)
            endText.text += "\n玩家2 赢了!!!";
    }

    /// <summary>
    /// 解决每组中每张卡的新位置
    /// </summary>
    public void reorganizeGroup()
    {

        if (activeDeck.cardsInDeck.Count > 0)
        {
            Vector3 centerVector = areas.getDeckCenterVector() + new Vector3(0, -0.1456f, -0.1f);
           // Debug.Log("centerVector: " + centerVector);
            // 对于奇数数量的手牌
            if (activeDeck.cardsInDeck.Count % 2 == 1)
            {
               // Debug.Log("手牌为奇数" + activeDeck.cardsInDeck.Count);
               // Debug.Log("------------------------------------------------------------------");
                int j = 1;
                activeDeck.cardsInDeck[0].transform.position = centerVector;

                for (int i = 1; i < activeDeck.cardsInDeck.Count; i++)
                {
              
                    activeDeck.cardsInDeck[i].transform.position = new Vector3(centerVector.x + j * 1.05f, centerVector.y, centerVector.z);
                   // Debug.Log("位置: " + activeDeck.cardsInDeck[i].transform.position +"i: " +  i);
                   // Debug.Log("------------------------------------------------------------------");
                    j *= -1;
                    if (i % 2 == 0)
                        j++;
                }
            }
            else
            {
                //Debug.Log("手牌为偶数" + activeDeck.cardsInDeck.Count);
                //Debug.Log("------------------------------------------------------------------");

                int j = 1;
                activeDeck.cardsInDeck[0].transform.position = centerVector + new Vector3(0.525f, 0, 0);
                activeDeck.cardsInDeck[1].transform.position = centerVector + new Vector3(-0.525f, 0, 0);
                Debug.Log("activeDeck.cardsInDeck[0].transform.position: " + activeDeck.cardsInDeck[0].transform.position + "activeDeck.cardsInDeck[1].transform.position" + activeDeck.cardsInDeck[1].transform.position);

                for (int i = 2; i < activeDeck.cardsInDeck.Count; i++)
                {
                    activeDeck.cardsInDeck[i].transform.position = new Vector3(centerVector.x + j * 1.05f + Math.Sign(j) * 0.525f, centerVector.y, centerVector.z);
                   // Debug.Log("位置: " + activeDeck.cardsInDeck[i].transform.position + "i: " + i + "j: " + j);
                   // Debug.Log("------------------------------------------------------------------");
                    j *= -1;
                    if (i % 2 == 1)
                        j++;
                }
            }
//             else
//             {
//                 /*测试代码：通过卡牌之间的间隙，排除多余的两张卡*/
//                 Debug.Log("手牌为偶数" + activeDeck.cardsInDeck.Count);
//                 Debug.Log("------------------------------------------------------------------");
// 
//                 int j = 1;
// //                 activeDeck.cardsInDeck[0].transform.position = centerVector + new Vector3(0.525f, 0, 0);
// //                 activeDeck.cardsInDeck[1].transform.position = centerVector + new Vector3(-0.525f, 0, 0);
//                 activeDeck.cardsInDeck[0].transform.position = centerVector + new Vector3(0.618f, 0, 0);
//                 activeDeck.cardsInDeck[1].transform.position = centerVector + new Vector3(-0.618f, 0, 0);
//                 Debug.Log("activeDeck.cardsInDeck[0].transform.position: " + activeDeck.cardsInDeck[0].transform.position + "activeDeck.cardsInDeck[1].transform.position" + activeDeck.cardsInDeck[1].transform.position);
// 
//                 for (int i = 2; i < activeDeck.cardsInDeck.Count; i++)
//                 {
//                     activeDeck.cardsInDeck[i].transform.position = new Vector3(centerVector.x + j * 1.28f + Math.Sign(j) * 0.620f, centerVector.y, centerVector.z);
//                     Debug.Log("位置: " + activeDeck.cardsInDeck[i].transform.position + "i: " + i + "j: " + j);
//                     Debug.Log("------------------------------------------------------------------");
//                     j *= -1;
//                     if (i % 2 == 1)
//                         j++;
//                 }
//             }
        }
        if (activeDeck.cardsInSwords.Count > 0)
        {
            Vector3 centerVector = areas.getSwordsCenterVector() + new Vector3(0, -0.1456f, -0.1f);

            // 卡牌数量为奇数
            if (activeDeck.cardsInSwords.Count % 2 == 1)
            {
                int j = 1;
                activeDeck.cardsInSwords[0].transform.position = centerVector;

                for (int i = 1; i < activeDeck.cardsInSwords.Count; i++)
                {
                    activeDeck.cardsInSwords[i].transform.position = new Vector3(centerVector.x + j *1.05f, centerVector.y, centerVector.z);

                    j *= -1;
                    if (i % 2 == 0)
                        j++;
                }
            }
            else
            {
                int j = 1;
                activeDeck.cardsInSwords[0].transform.position = centerVector + new Vector3(0.525f, 0, 0);
                activeDeck.cardsInSwords[1].transform.position = centerVector + new Vector3(-0.525f, 0, 0);

                for (int i = 2; i < activeDeck.cardsInSwords.Count; i++)
                {
                    activeDeck.cardsInSwords[i].transform.position = new Vector3(centerVector.x + j * 1.05f + Math.Sign(j) * 0.525f, centerVector.y, centerVector.z);

                    j *= -1;
                    if (i % 2 == 1)
                        j++;
                }
            }
        }
        if (activeDeck.cardsInBows.Count > 0)
        {
            Vector3 centerVector = areas.getBowsCenterVector() + new Vector3(0, -0.1456f, -0.1f);

            // 卡牌数量为奇数
            if (activeDeck.cardsInBows.Count % 2 == 1)
            {
                int j = 1;
                activeDeck.cardsInBows[0].transform.position = centerVector;

                for (int i = 1; i < activeDeck.cardsInBows.Count; i++)
                {
                    activeDeck.cardsInBows[i].transform.position = new Vector3(centerVector.x + j * 1.05f, centerVector.y, centerVector.z);

                    j *= -1;
                    if (i % 2 == 0)
                        j++;
                }
            }
            else
            {
                int j = 1;
                activeDeck.cardsInBows[0].transform.position = centerVector + new Vector3(0.525f, 0, 0);
                activeDeck.cardsInBows[1].transform.position = centerVector + new Vector3(-0.525f, 0, 0);

                for (int i = 2; i < activeDeck.cardsInBows.Count; i++)
                {
                    activeDeck.cardsInBows[i].transform.position = new Vector3(centerVector.x + j * 1.05f + Math.Sign(j) * 0.525f, centerVector.y, centerVector.z);

                    j *= -1;
                    if (i % 2 == 1)
                        j++;
                }
            }
        }
        if (activeDeck.cardsInTrebuchets.Count > 0)
        {
            Vector3 centerVector = areas.getTrebuchetsCenterVector() + new Vector3(0, -0.1456f, -0.1f);

            // 卡牌数量为奇数
            if (activeDeck.cardsInTrebuchets.Count % 2 == 1)
            {
                int j = 1;
                activeDeck.cardsInTrebuchets[0].transform.position = centerVector;

                for (int i = 1; i < activeDeck.cardsInTrebuchets.Count; i++)
                {
                    activeDeck.cardsInTrebuchets[i].transform.position = new Vector3(centerVector.x + j * 1.05f, centerVector.y, centerVector.z);

                    j *= -1;
                    if (i % 2 == 0)
                        j++;
                }
            }
            else
            {
                int j = 1;
                activeDeck.cardsInTrebuchets[0].transform.position = centerVector + new Vector3(0.525f, 0, 0);
                activeDeck.cardsInTrebuchets[1].transform.position = centerVector + new Vector3(-0.525f, 0, 0);

                for (int i = 2; i < activeDeck.cardsInTrebuchets.Count; i++)
                {
                    activeDeck.cardsInTrebuchets[i].transform.position = new Vector3(centerVector.x + j * 1.05f + Math.Sign(j) * 0.525f, centerVector.y, centerVector.z);

                    j *= -1;
                    if (i % 2 == 1)
                        j++;
                }
            }
        }
        // For spy cards - PLAYER 1
        if (activePlayerNumber == (int)PlayerNumber.PLAYER1 && player2.getDeck().cardsInSwords.Count > 0)
        {
            Vector3 centerVector = areas.getSword2CenterVector() + new Vector3(0, -0.1456f, -0.1f);

            // 卡牌数量为奇数
            if (player2.getDeck().cardsInSwords.Count % 2 == 1)
            {
                int j = 1;
                player2.getDeck().cardsInSwords[0].transform.position = centerVector;

                for (int i = 1; i < player2.getDeck().cardsInSwords.Count; i++)
                {
                    player2.getDeck().cardsInSwords[i].transform.position = new Vector3(centerVector.x + j * 1.05f, centerVector.y, centerVector.z);

                    j *= -1;
                    if (i % 2 == 0)
                        j++;
                }
            }
            else
            {
                int j = 1;
                player2.getDeck().cardsInSwords[0].transform.position = centerVector + new Vector3(0.525f, 0, 0);
                player2.getDeck().cardsInSwords[1].transform.position = centerVector + new Vector3(-0.525f, 0, 0);

                for (int i = 2; i < player2.getDeck().cardsInSwords.Count; i++)
                {
                    player2.getDeck().cardsInSwords[i].transform.position = new Vector3(centerVector.x + j * 1.05f + Math.Sign(j) * 0.525f, centerVector.y, centerVector.z);

                    j *= -1;
                    if (i % 2 == 1)
                        j++;
                }
            }
        }
        // For spy cards - PLAYER 2
        if (activePlayerNumber == (int)PlayerNumber.PLAYER2 && player1.getDeck().cardsInSwords.Count > 0 )
        {
            Vector3 centerVector = areas.getSword2CenterVector() + new Vector3(0, -0.1456f, -0.1f);

            // 卡牌数量为奇数
            if (player1.getDeck().cardsInSwords.Count % 2 == 1)
            {
                int j = 1;
                player1.getDeck().cardsInSwords[0].transform.position = centerVector;

                for (int i = 1; i < player1.getDeck().cardsInSwords.Count; i++)
                {
                    player1.getDeck().cardsInSwords[i].transform.position = new Vector3(centerVector.x + j * 1.05f, centerVector.y, centerVector.z);

                    j *= -1;
                    if (i % 2 == 0)
                        j++;
                }
            }
            else
            {
                int j = 1;
                player1.getDeck().cardsInSwords[0].transform.position = centerVector + new Vector3(0.525f, 0, 0);
                player1.getDeck().cardsInSwords[1].transform.position = centerVector + new Vector3(-0.525f, 0, 0);

                for (int i = 2; i < player1.getDeck().cardsInSwords.Count; i++)
                {
                    player1.getDeck().cardsInSwords[i].transform.position = new Vector3(centerVector.x + j * 1.05f + Math.Sign(j) * 0.525f, centerVector.y, centerVector.z);

                    j *= -1;
                    if (i % 2 == 1)
                        j++;
                }
            }
        }
    }

    /// <summary>
    /// Defined type of card groups
    /// </summary>
    private enum CardGroup { DECK, SWORD, BOW, TREBUCHET };

    /// <summary>
    /// Defined name of players
    /// </summary>
    private enum PlayerNumber { PLAYER1 = 1, PLAYER2 };

    /// <summary>
    /// Defined game status
    /// </summary>
    private enum GameStatus { END, TOUR1, TOUR2, TOUR3 };

    /// <summary>
    /// Defined type of card
    /// </summary>
    private enum TypeOfCard { NORMAL, GOLD, SPY, MANEKIN, DESTROY, WEATHER, GOLD_SPY };

    /// <summary>
    /// Switch player - update active deck
    /// </summary>
    private void switchPlayer()
    {
        reorganizeGroup();
        state = (int)Status.BLOCKED;
        showActiveCard(false);

        StartCoroutine(Wait(0.75f));
    }

    /// <summary>
    /// End tour - 显示谁获胜了，然后开启次回合
    /// </summary>
    private void endTour()
    {
        // 结束回合之前
        endPanelObject.SetActive(true);
        giveUpButtonObject.SetActive(false);

        player1.isPlaying = true;
        player2.isPlaying = true;

        //待优化
        if ((player1.health == 1 && player2.health == 2) || (player1.health == 2 && player2.health == 1))
            gameStatus = (int)GameStatus.TOUR2;
        else if ((player1.health == 1 && player2.health == 1) && (gameStatus == (int)GameStatus.TOUR2))
            gameStatus = (int)GameStatus.TOUR3;
        else if((player1.health == 1 && player2.health == 1) && (gameStatus != (int)GameStatus.TOUR2))
        {
            gameStatus = (int)GameStatus.TOUR2;
        }

        Debug.Log("当前回合: " + gameStatus);
        initializePlayersDecks();
        // TODO - PlayerX 获胜 - started
        Debug.Log("WaitEndTour() has started");
        StartCoroutine(WaitEndTour(3f));
    }

    IEnumerator WaitEndTour(float duration)
    {
        yield return new WaitForSeconds(duration);

        Debug.Log("WaitEndTour() has ended");
        // 结束回合之后，及duration结束之后
        endPanelObject.SetActive(false);
        giveUpButtonObject.SetActive(true);
    }

    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);

        //giveUpButtonObject.SetActive(false);
        //playerDownNameTextObject.SetActive(false);
        // playerUpNameTextObject.SetActive(false);

        button.transform.position = new Vector3(0, 0, -1f);

        if (activePlayerNumber == (int)PlayerNumber.PLAYER1)
        {
            this.activeDeck = player2.getDeck();
            player1.setDeckVisibility(false);
            player2.setDeckVisibility(true);
            activePlayerNumber = (int)PlayerNumber.PLAYER2;
        }
        else if (activePlayerNumber == (int)PlayerNumber.PLAYER2)
        {
            this.activeDeck = player1.getDeck();
            player1.setDeckVisibility(true);
            player2.setDeckVisibility(false);
            activePlayerNumber = (int)PlayerNumber.PLAYER1;
        }

        button.GetComponentInChildren<Text>().text = "玩家 " + activePlayerNumber + ",\n触摸继续";

        Vector3 upPlayerNamePosition = playerUpNameTextObject.transform.position;
        playerUpNameTextObject.transform.position = playerDownNameTextObject.transform.position;
        playerDownNameTextObject.transform.position = upPlayerNamePosition;

        // 互换双方生命宝石的位置
        Vector3 playerOneHealthOneVector = player1.getHealthDiamond(1).getPosition();
        Vector3 playerOneHealthSecondVector = player1.getHealthDiamond(2).getPosition();
        Vector3 playerTwoHealthOneVector = player2.getHealthDiamond(1).getPosition();
        Vector3 playerTwoHealthSecondVector = player2.getHealthDiamond(2).getPosition();
        player1.getHealthDiamond(1).setPosition(playerTwoHealthOneVector);
        player1.getHealthDiamond(2).setPosition(playerTwoHealthSecondVector);
        player2.getHealthDiamond(1).setPosition(playerOneHealthOneVector);
        player2.getHealthDiamond(2).setPosition(playerOneHealthSecondVector);

        //cardNumber  的位置替换
        Vector3 playerOneNumberOfCardsPosition = cardNumber1.transform.position;
        cardNumber1.transform.position = cardNumber2.transform.position;
        cardNumber2.transform.position = playerOneNumberOfCardsPosition;

        // 上下翻转卡牌
        player1.getDeck().flipGroupCards();
        player2.getDeck().flipGroupCards();
        //player1.getDeck().changeDeathPos(player2.getDeck().cardsInDeaths); //函数测试中

        if(activePlayerNumber == (int)PlayerNumber.PLAYER1)
        {
            Deck.changeDeathPos(player1.getDeck().cardsInDeaths, player2.getDeck().cardsInDeaths);
        }
        else
        {
            Deck.changeDeathPos(player2.getDeck().cardsInDeaths, player1.getDeck().cardsInDeaths);
        }

        // 比分位置交换
        Vector3 tempVector = score1Text.transform.position;
        score1Text.transform.position = score2Text.transform.position;
        score2Text.transform.position = tempVector;

        tempVector = score3Text.transform.position;
        score3Text.transform.position = score6Text.transform.position;
        score6Text.transform.position = tempVector;

        tempVector = score4Text.transform.position;
        score4Text.transform.position = score7Text.transform.position;
        score7Text.transform.position = tempVector;

        tempVector = score5Text.transform.position;
        score5Text.transform.position = score8Text.transform.position;
        score8Text.transform.position = tempVector;

        reorganizeGroup();

        state = (int)Status.FREE;
    }

    private void showActiveCard(bool ifShow)
    {

        if (ifShow)
            activeShowingCard.setBigFront(activeCard.getIndex() + 1); // +1 因为0是null
        else
            activeShowingCard.setBigFront(0);
    }

    public void giveUp()
    {
        Debug.Log("Button position: " + button.transform.position);
        if (button.transform.position.y > 5f)
        {
            Debug.Log("Give up!");
            switchPlayer();

            if (activePlayerNumber == (int)PlayerNumber.PLAYER1)
                player1.isPlaying = false;
            else if (activePlayerNumber == (int)PlayerNumber.PLAYER2)
                player2.isPlaying = false;
        }
        else
            Debug.Log("Blocked!");
    }
}