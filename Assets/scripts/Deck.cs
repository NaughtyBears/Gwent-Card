using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private GameObject cardGameObject;
    private Card baseCard;

    // TODO - 调用Awake或者start进行初始化
    public List<Card> cardsInDeck = new List<Card>(); // 手牌
    public List<Card> cardsInSwords = new List<Card>(); // 近战卡组
    public List<Card> cardsInBows = new List<Card>(); // 远程卡组
    public List<Card> cardsInTrebuchets = new List<Card>(); //攻城卡组
    public List<Card> cardsInDeaths = new List<Card>(); // 墓地卡组
    public List<Card> cardsInSpecial = new List<Card>(); // 特殊卡组

    // TODO - 添加卡牌时，卡牌与卡牌之间的间距
    public float startX = -6.53f;
    public float startY = -6.27f;
    public float startZ = -0.1f;
    public float stepX = 1.05f;

    private static int FRONTS_NUMBER = 35;
    // TODO - 每个范围组中的最大卡数
    private static int MAX_NUMBER_OF_CARDS_IN_GROUP = 10;
    private static int SWORD_GROUP_AMOUNT = 7;
    private static int SWORD_GOLD_GROUP_AMOUNT = 5;
    private static int BOW_GROUP_AMOUNT = 5;
    private static int BOW_GOLD_GROUP_AMOUNT = 1;
    private static int TREBUCHET_GROUP_AMOUNT = 3;
    private static int TREBUCHET_GOLD_GROUP_AMOUNT = 0;



    void Awake()
    {
        cardGameObject = GameObject.Find("Card");
        baseCard = cardGameObject.GetComponent<Card>();



    }

    /// <summary>
    /// 创建手牌
    /// 添加手牌
    /// </summary>
    /// <param name="numberOfCards">增加到玩家手牌的数量</param>
    public void buildDeck(int numberOfCards)
    {
        List<int> uniqueValues = new List<int>();

        for (int cardIndex = 0; cardIndex < numberOfCards; cardIndex++)
        {
            // 独立的卡牌堆
            int cardId;
            do
            {
                cardId = Random.Range(0, FRONTS_NUMBER);
            } while (uniqueValues.Contains(cardId));
            uniqueValues.Add(cardId);

            Card clone = Instantiate(baseCard) as Card;
            clone.tag = "CloneCard";
            clone.setFront(cardId);
            clone.setPower(baseCard.getCardModel().getPower(cardId));
            clone.setIndex(cardId);
            clone.setIsSpecial(clone.getCardModel().getIsSpecial(cardId));
            cardsInDeck.Add(clone);
        }
    }

    /// <summary>
    /// 随机增加俩卡牌到玩家手牌中
    /// </summary>
    /// <param name="whichPlayer"></param>
    public void addTwoRandomCards()
    {
        // TODO -卡牌不是唯一的!!!!!!
        for (int i = 0; i < 2; i++)
        {
            int cardId = Random.Range(0, FRONTS_NUMBER);
            Card clone = Instantiate(baseCard) as Card;
            clone.tag = "CloneCard";
            clone.setFront(cardId);
            clone.setPower(baseCard.getCardModel().getPower(cardId));
            clone.setIndex(cardId);
            clone.setIsSpecial(clone.getCardModel().getIsSpecial(cardId));
            cardsInDeck.Add(clone);
        }
    }


    public IEnumerable<Card> getCards()
    {
        foreach (Card c in cardsInDeck)
        {
            yield return c;
        }
    }

    public IEnumerable<Card> getSwordCards()
    {
        foreach (Card c in cardsInSwords)
        {
            yield return c;
        }
    }

    public IEnumerable<Card> getBowCards()
    {
        foreach (Card c in cardsInBows)
        {
            yield return c;
        }
    }

    public IEnumerable<Card> getTrebuchetCards()
    {
        foreach (Card c in cardsInTrebuchets)
        {
            yield return c;
        }
    }

    public IEnumerable<Card> getDeathCards()
    {
        foreach (Card c in cardsInDeaths)
        {
            yield return c;
        }
    }

    public IEnumerable<Card> getSpecialCards()
    {
        foreach (Card c in cardsInSpecial)
        {
            yield return c;
        }
    }

    /// <summary>
    /// 将卡牌添加到场上的进程组中
    /// </summary>
    /// <param name="card">添加到近程组的目标 card</param>
    /// <returns>成功返回true</returns>
    public bool addCardToSwords(Card card)
    {
        Debug.Log("Sword" + cardsInSwords.Count);
        if (cardsInSwords.Count < MAX_NUMBER_OF_CARDS_IN_GROUP && cardsInDeck.Contains(card))
        {
            Vector3 newVector = new Vector3(-2.53f + cardsInSwords.Count * 1.05f, -0.19f, -0.1f);
            card.transform.position = newVector;

            cardsInSwords.Add(card);
            // TODO - 移除手牌中的目标 card
            cardsInDeck.Remove(card);
            return true;
        }
        else
        {
            disactiveAllInDeck();

        }

        return false;
    }

    /// <summary>
    /// 从近战组中回到手牌
    /// </summary>
    /// <param name="card">返回手牌的目标 card</param>
    /// <returns>成功返回true</returns>
    public bool moveCardToDeckFromSwords(Card card)
    {
        if (card.weatherEffect == true)
            card.weatherEffect = false;
        cardsInDeck.Add(card);
        return cardsInSwords.Remove(card);
    }

    /// <summary>
    /// 从远程组中回到手牌
    /// </summary>
    /// <param name="card">返回手牌的目标 card</param>
    /// <returns>成功返回true</returns>
    public bool moveCardToDeckFromBows(Card card)
    {
        if (card.weatherEffect == true)
            card.weatherEffect = false;
        cardsInDeck.Add(card);
        return cardsInBows.Remove(card);
    }

    /// <summary>
    /// 从攻城组中回到手牌
    /// </summary>
    /// <param name="card">返回手牌的目标 card</param>
    /// <returns>成功返回true</returns>
    public bool moveCardToDeckFromTrebuchets(Card card)
    {
        if (card.weatherEffect == true)
            card.weatherEffect = false;
        cardsInDeck.Add(card);
        return cardsInTrebuchets.Remove(card);
    }

    /// <summary>
    /// 将间谍牌添加与对面近战组中
    /// </summary>
    /// <param name="card">spy card</param>
    public void addSpy(Card card)
    {
        Vector3 newVector = new Vector3(-2.53f + cardsInSwords.Count * 1.05f, 1.66495f, -0.1f);
        card.transform.position = newVector;

        cardsInSwords.Add(card);
    }

    /// <summary>
    /// 销毁手牌中当前选中卡牌并添加当前天气牌到特殊框中
    /// </summary>
    /// <param name="card">卡牌添加</param>
    public void addToSpecial(Card card)
    {
        cardsInSpecial.Add(card);
        cardsInDeck.Remove(card);
    }

    /// <summary>
    /// 从特殊框中移除天气到墓地
    /// </summary>
    public void deleteFromSpecial()
    {
        foreach (Card c in getSpecialCards())
        {
            if (c.isSpecial == 5)
            {
                if (c.power == -4)
                    sendCardToDeathList(c, 1);
                else
                    sendCardToDeathList(c);
            }

        }
        cardsInSpecial.Clear();
    }

    /// <summary>
    /// 添加卡牌到远程组
    /// </summary>
    /// <param name="card">添加到远程组的目标 card</param>
    /// <returns>成功返回true</returns>
    public bool addCardToBows(Card card)
    {
        if (cardsInBows.Count < MAX_NUMBER_OF_CARDS_IN_GROUP && cardsInDeck.Contains(card))
        {
            Vector3 newVector = new Vector3(-2.53f + cardsInBows.Count * 1.05f, -1.91f, -0.1f);
            card.transform.position = newVector;
            cardsInBows.Add(card);
            // TODO - should I remove deck from deck
            cardsInDeck.Remove(card);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 添加卡牌到攻城组
    /// </summary>
    /// <param name="card">添加到攻城组的目标 card</param>
    /// <returns>成功返回true</returns>
    public bool addCardToTrebuchets(Card card)
    {
        if (cardsInTrebuchets.Count < MAX_NUMBER_OF_CARDS_IN_GROUP && cardsInDeck.Contains(card))
        {
            Vector3 newVector = new Vector3(-2.53f + cardsInTrebuchets.Count * 1.05f, -3.66f, -0.1f);
            card.transform.position = newVector;

            cardsInTrebuchets.Add(card);
            // TODO - 移除以及使用的手牌
            cardsInDeck.Remove(card);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 将玩家场上的卡牌送往墓地
    /// </summary>
    /// <returns>成功返回true</returns>
    public bool sendCardsToDeathList()
    {
        bool ifSucceeded = false;

        for (int i = cardsInSwords.Count - 1; i >= 0; i--)
        {
            cardsInDeaths.Add(cardsInSwords[i]);
            ifSucceeded = cardsInSwords.Remove(cardsInSwords[i]);
        }
        for (int i = cardsInBows.Count - 1; i >= 0; i--)
        {
            cardsInDeaths.Add(cardsInBows[i]);
            ifSucceeded = cardsInBows.Remove(cardsInBows[i]);
        }
        for (int i = cardsInTrebuchets.Count - 1; i >= 0; i--)
        {
            cardsInDeaths.Add(cardsInTrebuchets[i]);
            ifSucceeded = cardsInTrebuchets.Remove(cardsInTrebuchets[i]);
        }

        return ifSucceeded;
    }


    /// <summary>
    /// 将 List cards 送往墓地
    /// </summary>
    /// <param name="card">想要送往墓地的 cards</param>
    /// <returns>成功返回true</returns>
    public bool sendCardToDeathList(List<Card> cards)
    {
        bool ifSucceeded = false;

        foreach (Card card in cards)
        {
            cardsInDeaths.Add(card);
            if (card.getGroup() == (int)CardGroup.SWORD)
                ifSucceeded = cardsInSwords.Remove(card);
            if (card.getGroup() == (int)CardGroup.BOW)
                ifSucceeded = cardsInBows.Remove(card);
            if (card.getGroup() == (int)CardGroup.TREBUCHET)
                ifSucceeded = cardsInTrebuchets.Remove(card);

            Vector3 player1DeathAreaVector;
            //Vector3 player1DeathAreaVector = new Vector3(8.51f, -4.6f, -0.1f);
            //Vector3 player1DeathAreaVector = new Vector3(8.41f, -4.7f, -0.1f);
            //Vector3 player1DeathAreaVector = new Vector3(8.51f, -4.6f, -0.1f);
            if (cardsInDeaths.Count == 1)
            {
                player1DeathAreaVector = new Vector3(8.47f, -4.56f, -0.08f);
                card.transform.position = player1DeathAreaVector;

                float x = card.transform.position.x;
                float y = card.transform.position.y;
                float z = card.transform.position.z;


                //card.transform.position = new Vector3(x + (float)cardsInDeaths.Count / 25.0f, (y + (float)cardsInDeaths.Count / 25.0f) * -1f, z - (float)cardsInDeaths.Count / 50.0f);
                card.transform.position = new Vector3(x + 0.04f, (y + 0.04f) * -1f, z - 0.02f);
            }

            else
            {
                player1DeathAreaVector = cardsInDeaths[cardsInDeaths.Count - 2].transform.position;
                card.transform.position = player1DeathAreaVector;

                float x = card.transform.position.x;
                float y = card.transform.position.y;
                float z = card.transform.position.z;


                //card.transform.position = new Vector3(x + (float)cardsInDeaths.Count / 25.0f, (y + (float)cardsInDeaths.Count / 25.0f) * -1f, z - (float)cardsInDeaths.Count / 50.0f);
                card.transform.position = new Vector3(x + 0.04f, y + 0.04f, z - 0.02f);
            }



        }

        return ifSucceeded;
    }

    /// <summary>
    /// 将卡牌送往墓地
    /// </summary>
    /// <param name="card">想要送往墓地的 card</param>
    /// <returns>true if succeeded</returns>
    public bool sendCardToDeathList(Card card)
    {
        bool ifSucceeded = false;

        cardsInDeaths.Add(card);
        if (card.getGroup() == (int)CardGroup.SWORD)
            ifSucceeded = cardsInSwords.Remove(card);
        else if (card.getGroup() == (int)CardGroup.BOW)
            ifSucceeded = cardsInBows.Remove(card);
        else if (card.getGroup() == (int)CardGroup.TREBUCHET)
            ifSucceeded = cardsInTrebuchets.Remove(card);
        else if(card.isSpecial != 5)
            ifSucceeded = cardsInSpecial.Remove(card);

        Vector3 player1DeathAreaVector;
        //Vector3 player1DeathAreaVector = new Vector3(8.51f, -4.6f, -0.1f);
        //Vector3 player1DeathAreaVector = new Vector3(8.41f, -4.7f, -0.1f);
        //Vector3 player1DeathAreaVector = new Vector3(8.51f, -4.6f, -0.1f);
        if (cardsInDeaths.Count == 1)
        {
            player1DeathAreaVector = new Vector3(8.47f, -4.56f, -0.08f);
            card.transform.position = player1DeathAreaVector;

            float x = card.transform.position.x;
            float y = card.transform.position.y;
            float z = card.transform.position.z;


            //card.transform.position = new Vector3(x + (float)cardsInDeaths.Count / 25.0f, (y + (float)cardsInDeaths.Count / 25.0f) * -1f, z - (float)cardsInDeaths.Count / 50.0f);
            card.transform.position = new Vector3(x + 0.04f, (y + 0.04f) * -1f, z - 0.02f);
        }

        else
        {
            player1DeathAreaVector = cardsInDeaths[cardsInDeaths.Count - 2].transform.position;
            card.transform.position = player1DeathAreaVector;

            float x = card.transform.position.x;
            float y = card.transform.position.y;
            float z = card.transform.position.z;


            //card.transform.position = new Vector3(x + (float)cardsInDeaths.Count / 25.0f, (y + (float)cardsInDeaths.Count / 25.0f) * -1f, z - (float)cardsInDeaths.Count / 50.0f);
            card.transform.position = new Vector3(x + 0.04f, y + 0.04f, z - 0.02f);
        }


        return ifSucceeded;
    }
    /// <summary>
    /// 将卡牌送往墓地
    /// </summary>
    /// <param name="card">将要去墓地的卡牌</param>
    /// <param name="activePlayerNumber">哪一位玩家</param>
    public bool sendCardToDeathList(Card card, int activePlayerNumber)
    {
        bool ifSucceeded = false;

        cardsInDeaths.Add(card);
        if (card.getGroup() == (int)CardGroup.SWORD)
            ifSucceeded = cardsInSwords.Remove(card);
        else if (card.getGroup() == (int)CardGroup.BOW)
            ifSucceeded = cardsInBows.Remove(card);
        else if (card.getGroup() == (int)CardGroup.TREBUCHET)
            ifSucceeded = cardsInTrebuchets.Remove(card);
        else if (card.isSpecial != 5)
            ifSucceeded = cardsInSpecial.Remove(card);

        Vector3 player1DeathAreaVector;
        //Vector3 player1DeathAreaVector = new Vector3(8.51f, -4.6f, -0.1f);
        //Vector3 player1DeathAreaVector = new Vector3(8.41f, -4.7f, -0.1f);
        //Vector3 player1DeathAreaVector = new Vector3(8.51f, -4.6f, -0.1f);
        if (cardsInDeaths.Count == 1)
            player1DeathAreaVector = new Vector3(8.47f, -4.56f, -0.08f);
        else
            player1DeathAreaVector = cardsInDeaths[cardsInDeaths.Count - 2].transform.position;


        card.transform.position = player1DeathAreaVector;

        float x = card.transform.position.x;
        float y = card.transform.position.y;
        float z = card.transform.position.z;


        //card.transform.position = new Vector3(x + (float)cardsInDeaths.Count / 25.0f, (y + (float)cardsInDeaths.Count / 25.0f) * -1f, z - (float)cardsInDeaths.Count / 50.0f);
        card.transform.position = new Vector3(x + 0.04f, y + 0.04f, z - 0.02f);
        return ifSucceeded;
    }

    /// <summary>
    /// 将卡牌撤回至手牌区域
    /// </summary>
    public void disactiveAllInDeck()
    {
        if (cardsInDeck.Count > 0)
        {
            foreach (Card c in getCards())
            {
                if (c.isActive())
                {
                    c.setActive(false);
                    c.transform.position += new Vector3(0, -0.15f, 0);
                }
            }
        }
    }

    /// <summary>
    /// 移除点数最大的牌，包含多张情况
    /// </summary>
    public void removeMaxPowerCard()
    {
        int maxPower = 1;
        List<Card> maxCard = new List<Card>();
        foreach (Card card in cardsInSwords)
        {
            // 如果卡牌不是金卡且未受天气效果影响 || 卡牌不是金卡且受天气效果
            if ((card.weatherEffect == false && card.getPower() >= maxPower && card.getIsSpecial() != 1) || (card.weatherEffect == true && 1 >= maxPower && card.getIsSpecial() != 1))
            {
                if (card.weatherEffect == false && maxPower < card.getPower())
                {
                    maxCard.Clear();
                    maxPower = card.getPower();
                }
                maxCard.Add(card);
            }
        }
        foreach (Card card in cardsInBows)
        {
            // 同上
            if ((card.weatherEffect == false && card.getPower() >= maxPower && card.getIsSpecial() != 1) || (card.weatherEffect == true && 1 >= maxPower && card.getIsSpecial() != 1))
            {
                //                 maxPower = card.getPower();
                //                 maxCard = card;
                if (card.weatherEffect == false && maxPower < card.getPower())
                {
                    maxCard.Clear();
                    maxPower = card.getPower();
                }
                maxCard.Add(card);
            }
        }
        foreach (Card card in cardsInTrebuchets)
        {
            // 同上
            if ((card.weatherEffect == false && card.getPower() >= maxPower && card.getIsSpecial() != 1) || (card.weatherEffect == true && 1 >= maxPower && card.getIsSpecial() != 1))
            {
                //                 maxPower = card.getPower();
                //                 maxCard = card;
                if (card.weatherEffect == false && maxPower < card.getPower())
                {
                    maxCard.Clear();
                    maxPower = card.getPower();
                }
                maxCard.Add(card);
            }
        }
        //防止空指针异常
        /* if (maxCard != null)
             sendCardToDeathList(maxCard);*/
        if (maxCard.Count != 0)
        {
            Debug.Log("第一张卡: " + maxCard[0].getPower());
            //             foreach (Card card in maxCard)
            //                 sendCardToDeathList(card);
            sendCardToDeathList(maxCard);
        }
    }

    /// <summary>
    /// 获取当前3个组的总点数(Get sum of the card's powers from group or all cards)
    /// </summary>
    /// <param name="group">各牌型对应的组 (0 - Decoy(诱饵), 1 - sword(近程), 2 - bow(远程), 3 - trebuchet(攻城))</param>
    /// <returns>当前玩家的总点数</returns>
    public int getPowerSum(int group)
    {
        int result = 0;

        if (group == 0 || group == 3)
        {
            foreach (Card card in getTrebuchetCards())
            {
                if (card.weatherEffect == false)
                    result += card.getPower();
                else
                    result++;
            }
        }
        if (group == 0 || group == 2)
        {
            foreach (Card card in getBowCards())
            {
                if (card.weatherEffect == false)
                    result += card.getPower();
                else
                    result++;
            }
        }
        if (group == 0 || group == 1)
        {
            foreach (Card card in getSwordCards())
            {
                if (card.weatherEffect == false)
                    result += card.getPower();
                else
                    result++;
            }
        }

        return result;
    }

    /// <summary>
    /// 标记卡牌已经被天气牌影响
    /// </summary>
    /// <param name="cardGroup">天气牌能够影响的组</param>
    public void applyWeatherEffect(int cardGroup)
    {
        //天气牌只能影响普通卡
        switch (cardGroup)
        {
            case 1:
                Game._instance.frostWeatherUp.SetActive(true);
                Game._instance.frostWeatherDown.SetActive(true);
                foreach (Card card in getSwordCards())
                {
                    if (card.getIsSpecial() == 0 || card.getIsSpecial() == 2)
                        card.weatherEffect = true;
                }
                break;
            case 2:
                Game._instance.fogWeatherUp.SetActive(true);
                Game._instance.fogWeatherDown.SetActive(true);
                foreach (Card card in getBowCards())
                {
                    //fogWeather.SetActive(true);
                    if (card.getIsSpecial() == 0)
                        card.weatherEffect = true;
                }
                break;
            case 3:
                Game._instance.rainWeatherUp.SetActive(true);
                Game._instance.rainWeatherDown.SetActive(true);
                foreach (Card card in getTrebuchetCards())
                {
                    if (card.getIsSpecial() == 0)
                        card.weatherEffect = true;
                }
                break;
            case 4: //晴天
                Game._instance.frostWeatherUp.SetActive(false);
                Game._instance.frostWeatherDown.SetActive(false);
                Game._instance.fogWeatherUp.SetActive(false);
                Game._instance.fogWeatherDown.SetActive(false);
                Game._instance.rainWeatherUp.SetActive(false);
                Game._instance.rainWeatherDown.SetActive(false);
                foreach (Card card in getSwordCards())
                {
                    if (card.getIsSpecial() == 0 || card.getIsSpecial() == 2)
                        card.weatherEffect = false;
                }
                foreach (Card card in getBowCards())
                {
                    if (card.getIsSpecial() == 0)
                        card.weatherEffect = false;
                }
                foreach (Card card in getTrebuchetCards())
                {
                    if (card.getIsSpecial() == 0)
                        card.weatherEffect = false;
                }
                break;
        }
    }

    /// <summary>
    /// 翻转玩家的卡牌，及switchPlayer函数之后
    /// </summary>
    public void flipGroupCards()
    {
        foreach (Card card in getSwordCards())
        {
            //card.flip(true, true);
            card.mirrorTransform();
        }
        foreach (Card card in getBowCards())
        {
            //card.flip(true, true);
            card.mirrorTransform();
        }
        foreach (Card card in getTrebuchetCards())
        {
            //card.flip(true, true);
            card.mirrorTransform();
        }
      /*  foreach (Card card in getDeathCards())
        {
            //card.flip(false, true);

            float x = card.transform.position.x;
            float y = card.transform.position.y;
            float z = card.transform.position.z;

            card.transform.position = new Vector3(x, y * -1f, z);
        }*/
        foreach (Card card in getSpecialCards())
        {
            float x = card.transform.position.x;
            float y = card.transform.position.y;
            float z = card.transform.position.z;

            card.transform.position = new Vector3(x, y * -1f, z);
        }
    }

    public void changeDeathPos(List<Card> cardsInDeath)
    {
        int count = (cardsInDeath.Count < this.cardsInDeaths.Count) ? this.cardsInDeaths.Count : cardsInDeath.Count;
        for (int i = 0; i <= count - 1; i++)
        {
            if (i <= cardsInDeath.Count  - 1 && i <= this.cardsInDeaths.Count - 1)
            {
                Vector3 tempVector = this.cardsInDeaths[i].transform.position;
                this.cardsInDeaths[i].transform.position = cardsInDeath[i].transform.position;
                cardsInDeath[i].transform.position = tempVector;
            }
            
            //player1墓地牌更多且player2以及调换完成
            if(count == this.cardsInDeaths.Count && i > cardsInDeath.Count - 1)
            {
                Vector3 tempVector = this.cardsInDeaths[i - 1].transform.position;
                tempVector.x += 0.04f;
                tempVector.y += 0.04f;
                tempVector.z -= 0.02f;
                this.cardsInDeaths[i].transform.position = tempVector;
            }
            else if(count == cardsInDeath.Count && i > this.cardsInDeaths.Count - 1)
            {
                Vector3 tempVector = cardsInDeath[i - 1].transform.position;
                tempVector.x += 0.04f;
                tempVector.y += 0.04f;
                tempVector.z -= 0.02f;
                cardsInDeath[i].transform.position = tempVector;
            }

        }
    }

    public void changeDeathPoss(List<Card> cardsInDeath)
    {
        var count = 0;
        foreach(Card c in cardsInDeath)
        {
            /*Vector3 tempVector = score1Text.transform.position;
            score1Text.transform.position = score2Text.transform.position;
            score2Text.transform.position = tempVector;*/

            if(count <= this.cardsInDeaths.Count - 1)
            {
                Vector3 tempVector = this.cardsInDeaths[count].transform.position;
                this.cardsInDeaths[count].transform.position = c.transform.position;
                c.transform.position = tempVector;
                count++;
            }
            //player2的牌比player1的墓地牌更多
            else
            {
                float x = this.cardsInDeaths[count - 1].transform.position.x;
                float y = this.cardsInDeaths[count - 1].transform.position.y + 0.04f;
                float z = this.cardsInDeaths[count - 1].transform.position.z;

                c.transform.position = new Vector3(x, y, z);

            }
        }
        if(count <= this.cardsInDeaths.Count - 1)
        {
            float x = this.cardsInDeaths[count - 1].transform.position.x;
            float y = this.cardsInDeaths[count - 1].transform.position.y + 0.04f;
            float z = this.cardsInDeaths[count - 1].transform.position.z;

            this.cardsInDeaths[count].transform.position = new Vector3(x, y, z);
        }
    }

    /// <summary>
    ///定义卡片组的类别
    /// </summary>
    private enum CardGroup { DECK, SWORD, BOW, TREBUCHET };
}
