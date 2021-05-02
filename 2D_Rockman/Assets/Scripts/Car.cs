using UnityEngine;

public class Car : MonoBehaviour
{
    #region 欄位
    // 單行註解 2021.4.25 KID
    /*
     * 多行註解
     * 多行註解
     */

    // 物件資料 - 欄位 Field：儲存物件資料
    // 欄位語法
    // 修飾詞 類型 名稱 指定 預設值 結尾

    // 四大類型
    // 整　數 int：任何沒有小數點的正負數值
    // 浮點數 float：任何有小數點的正負數值，有小數點結尾要加 f (大小寫皆可)
    // 字　串 string：任何文字，必須使用雙引號 ""
    // 布林值 bool：正反 true、false

    // 關鍵字 顏色：藍色
    // 自訂名稱 顏色：白色

    // 修飾詞
    // 私人：不顯示 private (預設值)
    // 公開：顯　示 public

    // 欄位屬性語法
    // [屬性名稱(屬性內容)]
    // 標題 Header(字串)
    // 提示 ToolTip(字串)
    // 範圍 Range(最小值，最大值) - 限定數值類型
    [Header("汽車的 CC 數")]
    [Tooltip("這是汽車的 CC 數。")]
    [Range(1000, 5000)]
    public int cc = 3000;
    // 逗號後面的空格可有可無
    [Header("汽車的重量"), Tooltip("這是汽車的重量。"), Range(0.5f, 10)]
    public float weight = 1.8f;
    [Header("汽車的品牌")]
    public string brand = "BMW";
    [Header("有沒有天窗")]
    public bool hasWindow = true;

    // Unity 常見類型
    // 顏色 Color
    public Color color;
    public Color red = Color.red;
    public Color y = Color.yellow;
    // 指定顏色的值為 0 - 1
    public Color myColor = new Color(0.3f, 0, 0.6f);            // Color(紅，綠，藍)
    public Color myColor2 = new Color(0, 0.5f, 0.5f, 0.5f);     // Color(紅，綠，藍，透明)

    // 座標二維-四維 Vector2、Vector3、Vector4
    public Vector2 v2;
    public Vector2 v2zero = Vector2.zero;
    public Vector2 v2one = Vector2.one;
    public Vector2 v2my = new Vector2(7, 9);

    public Vector3 v3 = new Vector3(1, 2, 3);
    public Vector4 v4 = new Vector4(1, 2, 3, 4);

    // 按鍵 KeyCode
    public KeyCode key1;                                // 不指定為 None (無)
    public KeyCode key2 = KeyCode.A;
    public KeyCode key3 = KeyCode.Mouse0;               // 左 0，右 1，滾輪 2
    public KeyCode key4 = KeyCode.Joystick1Button0;

    // 遊戲物件 與 元件
    // 遊戲物件 GameObject
    public GameObject obj1;
    public GameObject obj2;
    // 元件 Component - 屬性面板上可摺疊的
    // 名稱去掉空格
    public Transform tra;               // 可儲存任何包含 Transform 元件的物件
    public SpriteRenderer spr;          // 可儲存任何包含 SpriteRenderer 元件的物件
    #endregion

    #region 事件 Event
    public int number = 1;
    public bool test = false;
    public string prop = "紅色藥水";

    // 事件：在特定時間點會被執行的方法
    // Unity 提供的事件：開始、更新

    // 開始事件執行時間點與次數：播放遊戲後執行一次
    // 應用：數值初始化，例如：遊戲一開始多少金幣或生命值等等...
    private void Start()
    {
        // print(任何資料) - 輸出資料到 Console 儀表板上
        print("我是開始事件唷~");

        // ※ 欄位存取
        // 取得
        // 語法：欄位名稱
        // 字串串接：字串 + 其他欄位
        print("取得欄位：" + number);

        // 存放
        // 語法：欄位名稱 指定 值；
        // ※ 值必須與此欄位類型相同
        test = true;
        print("存放欄位後的結果：" + test);

        prop = "藍色藥水";
        print("存放後的道具名稱：" + prop);

        // ※ 呼叫方法
        // 方法名稱()；
        Test();
        Test();
        // 傳回方法：
        // 傳回類型 名稱 = 傳回方法()；
        int t = Ten();
        print("傳回方法的結果：" + t);

        Drive50();
        Drive100();
        Drive300();
        // 呼叫方法要有相同數量的參數
        // 有預設值得參數為【選填式參數】
        Drive(200, "咻咻咻");
        Drive(999, "轟轟轟", "爆炸特效");
        // 有多個選填式參數
        Drive(70, "閃電");                // 錯誤 - 把特效放在音效上
        Drive(70, effect: "閃電");        // 正確 - 指定特效參數

        float bmi = BMI(1.68f, 60);
        print("計算後的 BMI：" + bmi);
    }

    // 更新事件執行時間點與次數：開始事件後以每秒約六十次執行 60FPS
    // 應用：監聽玩家輸入與物件持續行為，例如：玩家有沒有按按鈕或讓物件持續移動
    private void Update()
    {
        print("我是更新事件!!!");
    }
    #endregion

    #region 方法 Unity Method、Function
    // 方法：保存較複雜或演算法的程式區塊
    // 語法：
    // 修飾詞 傳回類型 名稱(參數1，參數2，...，參數n) { 較複雜或演算法的程式區塊 }
    // void 無傳回：使用這個方法不會有傳回
    // 方法需要被【呼叫】才會執行
    /// <summary>
    /// 我是一個測試方法
    /// </summary>
    private void Test()
    {
        print("我是測試方法。");
    }

    // 如果不是無傳回 void
    // 在大括號內必須使用 傳回 return 值 (必須跟傳回類型相同)
    /// <summary>
    /// 傳回整數十的方法
    /// </summary>
    /// <returns>整數十</returns>
    private int Ten()
    {
        return 10;
    }

    // 舉例：
    // 三個方法 1. 以時數 50 開車 2. 時數 100 3. 時數 300
    // 加新功能 要有音效
    // 加特效
    private void Drive50()
    {
        print("開車時速：" + 50);
        print("開車音效");
    }
    private void Drive100()
    {
        print("開車時速：" + 100);
        print("開車音效");
    }
    private void Drive300()
    {
        print("開車時速：" + 300);
        print("開車音效");
    }

    // 用參數解決 Paramater
    // 參數語法：類型 參數名稱
    /// <summary>
    /// 開車
    /// </summary>
    /// <param name="speed">開車的時速</param>
    /// <param name="sound">開車的音效</param>
    /// <param name="effect">特效</param>
    // 有預設值得參數只能寫在最右邊
    private void Drive(int speed, string sound = "咻~", string effect = "灰塵效果")
    {
        print("開車時速：" + speed);
        print("開車音效：" + sound);
        print("特效：" + effect);
    }

    /// <summary>
    /// BMI 值計算方法
    /// </summary>
    /// <param name="height">請輸入身高 (單位為公尺)</param>
    /// <param name="weight">請輸入體重 (單位為公斤)</param>
    /// <returns></returns>
    private float BMI(float height, float weight)
    {
        return weight / (height * height);
    }
    #endregion
}
