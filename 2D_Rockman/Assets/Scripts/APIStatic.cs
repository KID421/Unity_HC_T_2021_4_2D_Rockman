using UnityEngine;

public class APIStatic : MonoBehaviour
{
    // 認識靜態 API
    // 包含關鍵字 static 就是靜態

    private void Start()
    {
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
    }
}
