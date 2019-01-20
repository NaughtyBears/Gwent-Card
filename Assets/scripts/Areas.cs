using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Areas : MonoBehaviour {
    BoxCollider [] colliders;
    BoxCollider deckCollider;
    BoxCollider swordCollider;
    BoxCollider bowCollider;
    BoxCollider trebuchetCollider;
    BoxCollider special1Collider;
    BoxCollider special2Collider;
    BoxCollider sword2Collider;
    Bounds a, b, c, d, e, f, g;
    void Awake()
    {
        colliders = GetComponents<BoxCollider>();
        deckCollider = colliders[(int)CardGroup.DECK];
        swordCollider = colliders[(int)CardGroup.SWORD];
        bowCollider = colliders[(int)CardGroup.BOW];
        trebuchetCollider = colliders[(int)CardGroup.TREBUCHET];
        special1Collider = colliders[(int)CardGroup.SPECIAL1];
        special2Collider = colliders[(int)CardGroup.SPECIAL2];
        sword2Collider = colliders[(int)CardGroup.SWORD2];
    }
    void Start()
    {
        a = deckCollider.bounds;
        b = swordCollider.bounds;
        c = bowCollider.bounds;
        d = trebuchetCollider.bounds;
        e = special1Collider.bounds;
        f = special2Collider.bounds;
        g = sword2Collider.bounds;
    }

    /// <summary>
    /// 获取玩家手牌的 collision bounds
    /// </summary>
    /// <returns>Deck's collision bounds</returns>
    public Bounds getDeckColliderBounds()
    {
        return deckCollider.bounds;
    }

    /// <summary>
    /// 获取玩家近战组的 collision bounds
    /// </summary>
    /// <returns>Sword group collision bounds</returns>
    public Bounds getSwordColliderBounds()
    {
        return swordCollider.bounds;
    }

    /// <summary>
    /// 获取玩家远程组的 collision bounds
    /// </summary>
    /// <returns>Bow group collision bounds</returns>
    public Bounds getBowColliderBounds()
    {
        return bowCollider.bounds;
    }

    /// <summary>
    /// 获取 攻城组的 collision bounds
    /// </summary>
    /// <returns>Trebuchet group bounds</returns>
    public Bounds getTrebuchetColliderBounds()
    {
        return trebuchetCollider.bounds;
    }

    /// <summary>
    /// 获取特殊组1的 collision bounds
    /// </summary>
    /// <returns>Special 1 group bounds</returns>
    public Bounds getSpecial1ColliderBounds()
    {
        return special1Collider.bounds;
    }

    /// <summary>
    ///  获取特殊组2的 collision bounds
    /// </summary>
    /// <returns>Special 2 group bounds</returns>
    public Bounds getSpecial2ColliderBounds()
    {
        return special2Collider.bounds;
    }

    /// <summary>
    ///  获取 player up 近战组的 collision bounds
    /// </summary>
    /// <returns>Sword in player 2 group bounds</returns>
    public Bounds getSword2ColliderBounds()
    {
        return sword2Collider.bounds;
    }

    /// <summary>
    /// 获取 deck collider center vector
    /// </summary>
    /// <returns>deck collider center vector</returns>
    public Vector3 getDeckCenterVector()
    {
        return deckCollider.center;
    }

    /// <summary>
    /// 获取 sword group collider center vector
    /// </summary>
    /// <returns>sworr group collider center vector</returns>
    public Vector3 getSwordsCenterVector()
    {
        return swordCollider.center;
    }

    /// <summary>
    /// 获取 bow group collider center vector
    /// </summary>
    /// <returns>bow group collider center vector</returns>
    public Vector3 getBowsCenterVector()
    {
        return bowCollider.center;
    }

    /// <summary>
    /// 获取 trebuchet group collider center vector
    /// </summary>
    /// <returns>trebuchet group collider center vector</returns>
    public Vector3 getTrebuchetsCenterVector()
    {
        return trebuchetCollider.center;
    }

    /// <summary>
    /// 获取 special 1 box collider center vector
    /// </summary>
    /// <returns>special 1 box collider center vector</returns>
    public Vector3 getSpecial1CenterVector()
    {
        return special1Collider.center;
    }

    /// <summary>
    /// 获取 special 2 box collider center vector
    /// </summary>
    /// <returns>special 2 box collider center vector</returns>
    public Vector3 getSpecial2CenterVector()
    {
        return special2Collider.center;
    }

    /// <summary>
    /// 获取 sword 2 box collider center vector
    /// </summary>
    /// <returns> sword 2 box collider center vector</returns>
    public Vector3 getSword2CenterVector()
    {
        return sword2Collider.center;
    }

    /// <summary>
    /// 定义 card group的枚举成员
    /// </summary>
    private enum CardGroup { DECK, SWORD, BOW, TREBUCHET, SPECIAL1, SPECIAL2, SWORD2};
}