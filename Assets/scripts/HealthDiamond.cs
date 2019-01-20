using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDiamond : MonoBehaviour {

    public Sprite healthSprite;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    /// <summary>
    /// 设置生命宝石 sprite的可见性
    /// </summary>
    /// <param name="visibility">true 显示生命宝石， false 不显示生命宝石</param>
    public void setVisibility(bool visibility)
    {
        this.spriteRenderer.gameObject.SetActive(visibility);
    }

    /// <summary>
    /// 将生命宝石移动到指定位置
    /// </summary>
    public void moveTo(float x, float y)
    {
        Vector3 vector = new Vector3(x, y, 0);
        this.transform.position = vector;
    }

    /// <summary>
    /// 设置spriteRenderer
    /// </summary>
    public void enableSprite()
    {
        this.spriteRenderer.sprite = this.healthSprite;
    }

    /// <summary>
    /// 获得生命宝石的位置
    /// </summary>
    /// <returns>生命宝石的位置</returns>
    public Vector3 getPosition()
    {
        return this.transform.position;
    }

    /// <summary>
    /// 设置生命宝石的位置
    /// </summary>
    /// <param name="vector">生命宝石的位置</param>
    public void setPosition(Vector3 vector)
    {
        this.transform.position = vector;
    }
}
