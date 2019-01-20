using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel : MonoBehaviour
{
    public Sprite[] smallFronts;
    public Sprite[] bigFronts;
    public string[] names;
    public int[] powers;
    public int[] groups; //天气牌+烧灼 4group     近战 1group     远程 2group     攻城 3group    诱饵 0group
    public int[] isSpecial;

    /* 
     * > isSpecial 值所代表的卡牌种类:
     * [1] - gold card               / 金卡
     * [2] - spy card                 / 间谍牌
     * [3] - manekin card        / 稻草人
     * [4] - destroy card         / 灼烧
     * [5] - weather card        /天气卡
     * [6] - gold spy               / 金卡(间谍)
     *
     * > Power table of values:
     * [0+] - normal                    /普通卡
     * [-1] - 
     * weather        / 降低近战的天气牌
     * [-2] - bow weather          / 降低远程的天气牌
     * [-3] - trebuchet weather / 降低攻城器械的天气牌
     * [-4] - clean weather        / 晴天
     */

    public Sprite getSmallFront(int index)
    {
        return smallFronts[index];
    }

    public Sprite getBigFront(int index)
    {
        return bigFronts[index];
    }

    public string getName(int index)
    {
        return names[index];
    }

    public int getPower(int index)
    {
        return powers[index];
    }

    public int getGroup(int index)
    {
        return groups[index];
    }

    public int getIsSpecial(int index)
    {
        Debug.Log("isSpecial[index]: " + isSpecial[index]);
        return isSpecial[index];
    }
}
