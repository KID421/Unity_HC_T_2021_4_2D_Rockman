using UnityEngine;

public class API : MonoBehaviour
{
    // 靜態 API
    // 非靜態 API

    // 1. 非靜態 API 需要物件
    // 2. 定義一個欄位 - 物件
    public Transform tra1;
    public Transform tra2;
    public SpriteRenderer spr;

    private void Start()
    {
        // 靜態
        // 類別名稱.靜態屬性
        float f = Random.value;

        // 非靜態
        // 取得屬性
        // 物件名稱.非靜態屬性
        print("取得物件的座標：" + tra1.position);

        // 存放屬性
        // 物件名稱.非靜態屬性 指定 值；
        tra2.localScale = new Vector3(3, 3, 3);

        spr.color = new Color(1, 0, 0);
        spr.flipX = true;
    }

    private void Update()
    {
        // 非靜態
        // 使用方法
        // 物件名稱.非靜態方法(對應參數)
        tra2.Translate(0.1f, 0, 0);
    }
}
