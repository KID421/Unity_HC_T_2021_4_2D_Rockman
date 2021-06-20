using UnityEngine;

public class Player : MonoBehaviour
{
    #region 欄位
    [Header("移動速度"), Range(0, 5000)]
    public float speed = 10.5f;
    [Header("跳躍高度"), Range(0, 3000)]
    public int jump = 100;
    [Header("血量"), Range(0, 200)]
    public float hp = 100;
    [Header("是否在地板上"), Tooltip("儲存角色是否在地板上")]
    public bool isGrounded;
    [Header("子彈"), Tooltip("角色要發射的子彈物件")]
    public GameObject bullet;
    [Header("子彈速度"), Range(0, 5000)]
    public int bulletSpeed = 800;
    [Header("開槍音效"), Tooltip("開槍的聲音")]
    public AudioClip bulletSound;
    [Header("判斷地板碰撞的位移與半徑")]
    public Vector3 groundOffset;
    public float groundRadius = 0.2f;
    [Header("子彈生成位置")]
    public Vector3 posBullet;

    private AudioSource aud;
    private Rigidbody2D rig;
    private Animator ani;
    private ParticleSystem ps;
    #endregion

    #region 事件
    private void Start()
    {
        // 利用程式取得元件
        // 傳回元件 取得元件<元件名稱>() - <泛型>
        // 取得跟此腳本同一層的元件
        rig = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();

        // 粒子系統 = 變形元件.搜尋子物件("子物件名稱")
        ps = transform.Find("集氣特效").GetComponent<ParticleSystem>();

        // 2D 物理.忽略圖層碰撞(圖層1，圖層2，是否要忽略)
        Physics2D.IgnoreLayerCollision(9, 10, true);
    }

    // 一秒約執行 60 次
    private void Update()
    {
        Move();
        Jump();
        Fire();
    }

    // 繪製圖示 - 輔助編輯時的圖形線條
    private void OnDrawGizmos()
    {
        // 1. 指定顏色
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        // 2. 繪製圖形
        // transform 可以抓到此腳本同一層的變形元件
        // 繪製球體(中心點，半徑)
        // 物件的右方 X 軸：transform.right
        // 物件的上方 Y 軸：transform.up
        // 物件的前方 Z 軸：transform.forward
        Gizmos.DrawSphere(transform.position + transform.right * groundOffset.x + transform.up * groundOffset.y, groundRadius);

        // 先指定顏色在畫圖型
        Gizmos.color = new Color(0.3f, 0.9f, 0.9f, 0.8f);
        Gizmos.DrawSphere(transform.position + transform.right * posBullet.x + transform.up * posBullet.y, 0.1f);
    }
    #endregion

    #region 方法
    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        // 1. 要抓到玩家按下左右鍵的資訊 Input
        float h = Input.GetAxis("Horizontal");

        // 2. 使用左右鍵的資訊控制角色移動
        // 剛體.加速度 = 二維向量(水平 * 速度 * 一幀的時間，指定回原本的 Y 軸加速度)
        // 一幀的時間 - 解決不同效能的裝置速度差問題
        rig.velocity = new Vector2(h * speed * Time.deltaTime, rig.velocity.y);

        // 如果 按下 D 面向右邊 0 0 0
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.eulerAngles = Vector3.zero;
        }
        // 否則 如果 按下 A 面向左邊 0 180 0
        else if (Input.GetKeyDown(KeyCode.A))
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        // 設定動畫
        // 水平值 不等於 零 布林值 打勾
        // 水平值 等於 零 布林值 取消
        // 不等於 !=
        ani.SetBool("走路開關", h != 0);
    }

    /// <summary>
    /// 跳躍
    /// </summary>
    private void Jump()
    {
        // 如果 玩家 按下 空白鍵 並且 在地板上 就 往上跳躍
        // 判斷式 C# 
        // 傳回值為布林值的方法可以當成布林值使用
        // ※ 判斷布林值是否等於 true 寫法
        // 1. isGrounded == true (原本寫法)
        // 2. isGrounded (簡寫)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // 剛體.添加推力(二維向量)
            rig.AddForce(new Vector2(0, jump));
        }

        // 碰到的物件 = 2D 物理.覆蓋圓形(中心點，半徑，圖層)
        // 圖層語法：1 << 圖層編號 (LayerMask int)
        Collider2D hit = Physics2D.OverlapCircle(transform.position + transform.right * groundOffset.x + transform.up * groundOffset.y, groundRadius, 1 << 8);

        // print("碰到的物件：" + hit.name);

        // 如果 碰到的物件 存在 並且 碰到的物件名稱 等於 地板 就代表在地板上
        // 並且 && (Shift + 7)
        // 等於 ==
        // 或者 || (Shift + \ 鎮)
        // 或者 名稱 等於 跳台

        if (hit && (hit.name == "地板" || hit.name == "跳台"))
        {
            isGrounded = true;
        }
        // 否則 不在地板上
        // 否則 else
        // 語法： else { 程式區塊 } - 僅能寫在 if 下方
        else
        {
            isGrounded = false;
        }
    }

    /// <summary>
    /// 紀錄按住左鍵的計時器
    /// </summary>
    private float timer;
    /// <summary>
    /// 攻擊力
    /// </summary>
    private float attack = 10;

    /// <summary>
    /// 開槍
    /// </summary>
    private void Fire()
    {
        // 如果 玩家按下左鍵 就開槍 - 動畫與音效 發射子彈
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ani.SetTrigger("攻擊觸發");
            ps.Play();                              // 播放集氣
        }
        // 否則如果
        // else if (布林值) { 程式區塊 }
        // 按住左鍵
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            // 累加 +=
            timer += Time.deltaTime;
            // print("按住左鍵的時間：" + timer);
        }
        // 放開左鍵
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            ps.Stop();                              // 停止集氣
            aud.PlayOneShot(bulletSound, 1f);

            // Object.Instantiate(bullet);  // 原始寫法

            // 暫存物件 = 生成(物件，座標，角度)
            // Quaternion 四位元 - 角度
            // Quaternion.identity 零角度
            GameObject temp = Instantiate(bullet, transform.position + transform.right * posBullet.x + transform.up * posBullet.y, Quaternion.identity);            // 簡寫
            // 暫存物件.取得元件<2D 剛體>().添加推力(角色前方 * 子彈速度)
            temp.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);
            // 暫存物件.添加元件<子彈>();
            temp.AddComponent<Bullet>();
            // 刪除(物件，延遲秒數)
            Destroy(temp, 2f);

            // 讓子彈的角度跟玩家目前的角度相同 - 子彈角度問題
            // 取得粒子的渲染元件
            ParticleSystemRenderer render = temp.GetComponent<ParticleSystemRenderer>();
            // 渲染的翻面 = 角色的角度 - ? : 三元運算子
            render.flip = new Vector3(transform.eulerAngles.y == 0 ? 0 : 1, 0, 0);

            // 計時器 = 數學.夾住(計時器，最小，最大)
            timer = Mathf.Clamp(timer, 0, 5);

            // 子彈的攻擊力 = 攻擊力 加上 四捨五入(計時器) * 2
            temp.GetComponent<Bullet>().attack = attack + Mathf.Round(timer) * 2;

            // 集氣：調整子彈尺寸
            // temp.transform.lossyScale = Vector3.one; // lossyScale 為唯讀 Read Only - 不能指定值 - 此行為錯誤示範 會出現紅色蚯蚓
            temp.transform.localScale = Vector3.one + Vector3.one * timer;

            // 計時器歸零
            timer = 0;
        }
    }

    /// <summary>
    /// 受傷
    /// </summary>
    /// <param name="damage">造成的傷害</param>
    private void Hit(float damage)
    {

    }

    /// <summary>
    /// 死亡
    /// </summary>
    /// <returns>是否死亡</returns>
    private bool Dead()
    {
        return false;
    }

    /// <summary>
    /// 吃道具
    /// </summary>
    /// <param name="prop">道具的名稱</param>
    private void EatProp(string prop)
    {

    }
    #endregion
}
