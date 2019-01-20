using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private string cardName;
    public int power;
    public int index;
    public int group; 
    private bool active = false;
    public int isSpecial;
    public bool weatherEffect = false;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D cardColider;

    private GameObject cardModelGameObject;
    private CardModel cardModel;
    
    void Awake()
    {
        cardModelGameObject = GameObject.Find("CardModel");
        cardModel = cardModelGameObject.GetComponent<CardModel>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardColider = GetComponent<BoxCollider2D>();
        cardColider.size = new Vector2(1f, 1.45f);
    }

    /// <summary>
    /// Name of card
    /// </summary>
    /// <returns>name of card</returns>
    public string getCardName()
    {
        return this.cardName;
    }

    /// <summary>
    /// 设置card name
    /// </summary>
    /// <param name="cardName">card's name</param>
    public void setCardName(string cardName)
    {
        this.cardName = cardName;
    }

    /// <summary>
    /// Power of card
    /// </summary>
    /// <returns>power of card</returns>
    public int getPower()
    {
        return this.power;
    }

    /// <summary>
    /// Set power of card
    /// </summary>
    /// <param name="power">New card's power</param>
    public void setPower(int power)
    {
        this.power = power;
    }

    /// <summary>
    /// Set index of card
    /// </summary>
    /// <param name="index">new card's index</param>
    public void setIndex(int index)
    {
        this.index = index;
        this.group = cardModel.groups[index];
    }

    /// <summary>
    /// Index of card
    /// </summary>
    /// <returns>Card's index</returns>
    public int getIndex()
    {
        return this.index;
    }

    /// <summary>
    /// Set new state of card
    /// </summary>
    /// <param name="state">New card's state (true or false)</param>
    public void setActive(bool state)
    {
        this.active = state;
    }

    /// <summary>
    /// Check if card is active
    /// </summary>
    /// <returns>True if card is active, false otherwise</returns>
    public bool isActive()
    {
        return this.active;
    }

    /// <summary>
    /// 获取 card's collision bounds
    /// </summary>
    /// <returns>Card's collision bounds</returns>
    public Bounds getBounds()
    {
        return this.cardColider.bounds;
    }

    /// <summary>
    /// 获取 card's name and it's power in string
    /// </summary>
    ///
    public string toString()
    {
        return this.cardName + " card with power " + this.power;
    }

    /// <summary>
    /// 设置手牌中card的图片
    /// </summary>
    /// <param name="index">cardModel.smallFronts中的索引</param>
    public void setFront(int index)
    {
        spriteRenderer.sprite = cardModel.getSmallFront(index);
    }

    /// <summary>
    /// 设置选择卡片的正面大图像
    /// </summary>
    /// <param name="index">cardModel.bigFronts中的索引，及大图像的索引位置</param>
    public void setBigFront(int index)
    {
        if (index == 0)
            spriteRenderer.sprite = null;
        else
            spriteRenderer.sprite = cardModel.getBigFront(index - 1);
    }

    /// <summary>
    /// Set card's group
    /// </summary>
    /// <param name="group">1 - 近战组, 2 - 远程组, 3 - 攻城组, 4 - 天气+灼烧</param>
    public void setGroup(int group)
    {
        this.group = group;
    }

    /// <summary>
    /// Get card's group
    /// </summary>
    /// <returns>1 - sword(近战组), 2 - bow(远程组), 3 - trebuchet(攻城组)</returns>
    public int getGroup()
    {
        return this.group;
    }

    /// <summary>
    /// 获取cardModel对象
    /// </summary>
    /// <returns>返回cardModel对象</returns>
    public CardModel getCardModel()
    {
        return this.cardModel;
    }

    /// <summary>
    /// 获取卡的特殊效果
    /// </summary>
    /// <returns>特殊卡 ([0] - normal(普通牌), [1] - gold(金卡), [2] - spy, [3] - manekin(诱饵), [4] - destroy, [5] - weather)</returns>
    public int getIsSpecial()
    {
        if(this.isSpecial == 4 )
        {
            Debug.Log("① this.isSpecial" + this.isSpecial);
        }

        return this.isSpecial;
    }

    /// <summary>
    /// 设置isSpecial的值
    /// </summary>
    /// <param name="isSpecial">如果卡是特殊类型，则为true</param>
    public void setIsSpecial(int isSpecial)
    {
        this.isSpecial = isSpecial;
    }

    /// <summary>
    /// Flip card
    /// </summary>
    /// <param name="x">true if you want to flip in x axis</param>
    /// <param name="y">true if you want to flip in y axis</param>
    public void flip(bool x, bool y)
    {
        if (x == true)
        {
            if (spriteRenderer.flipX == true)
                spriteRenderer.flipX = false;
            else
                spriteRenderer.flipX = true;
        }
        if (y == true)
        {
            if (spriteRenderer.flipY == true)
                spriteRenderer.flipY = false;
            else
                spriteRenderer.flipY = true;
        }
    }

    /// <summary>
    /// 镜像转换在（0,0,0）点左右(Mirror transformation around (0,0,0) point of Desk)
    /// </summary>
    public void mirrorTransform()
    {     
        transform.position = new Vector3(transform.position.x * -1 + 4.39f, transform.position.y * -1 + 1.435f, transform.position.z);
        //transform.position = new Vector3(0, 0, 0);
    }
}