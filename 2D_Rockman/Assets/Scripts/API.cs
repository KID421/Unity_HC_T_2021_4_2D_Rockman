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

    // 非靜態所需要的物件
    public Camera cam;
    public SpriteRenderer spr1;
    public Transform spr2;
    public Rigidbody2D rig;

    private void Start()
    {
        #region 認識 API
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
        #endregion

        // 取得非靜態屬性
        print("攝影機的深度：" + cam.depth);
        print("圖片1的顏色：" + spr1.color);
        // 存放非靜態屬性
        cam.backgroundColor = new Color(0.5f, 0.2f, 0.2f);
        spr1.flipY = true;
    }

    private void Update()
    {
        #region 認識 API
        // 非靜態
        // 使用方法
        // 物件名稱.非靜態方法(對應參數)
        tra2.Translate(0.1f, 0, 0);
        #endregion

        // 使用非靜態方法
        spr2.Rotate(0, 0, 1);
        rig.AddForce(new Vector2(0, 5));
    }
}
