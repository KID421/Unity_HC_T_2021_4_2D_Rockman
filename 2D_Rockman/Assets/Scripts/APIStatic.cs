using UnityEngine;

public class APIStatic : MonoBehaviour
{
    // 認識靜態 API
    // 包含關鍵字 static 就是靜態

    private void Start()
    {
        #region 認識靜態屬性與方法
        // 屬性 欄位 要知道如何存取
        // 練習取得靜態屬性 Static Properties
        // 語法
        // 類別名稱.靜態屬性名稱
        float r = Random.value;             // 隨機.值
        print("隨機值：" + r);

        // 練習存放靜態屬性 Static Properties
        // 有顯示 Read Only 的屬性不能存放
        // 語法
        // 類別名稱.靜態屬性名稱 指定 值；
        Cursor.visible = false;             // 指標.可見度

        // 練習使用靜態方法 Static Methods
        // 語法
        // 類別名稱.靜態方法(對應的參數)
        int attack = Random.Range(100, 300);
        print("隨機攻擊力：" + attack);

        float n = Mathf.Abs(-99.9f);
        print("絕對值結果：" + n);
        #endregion

        #region 練習
        print("攝影機數量：" + Camera.allCamerasCount);
        print("重力：" + Physics2D.gravity);
        #endregion
    }

    private void Update()
    {
        #region 練習
        print("玩家是否按下任意鍵：" + Input.anyKeyDown);
        print("遊戲時間：" + Time.time);
        #endregion
    }
}
