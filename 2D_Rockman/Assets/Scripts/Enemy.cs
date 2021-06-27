using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region 欄位
    [Header("移動速度"), Range(0, 500)]
    public float speed = 1f;
    [Header("攻擊力"), Range(0, 100)]
    public float attack = 10f;
    [Header("攻擊冷卻"), Range(0, 30)]
    public float cd = 3;
    [Header("血量"), Range(0, 5000)]
    public float hp = 200f;
    [Header("追蹤範圍"), Range(0, 50)]
    public float radiusTrack = 5;
    [Header("攻擊範圍"), Range(0, 30)]
    public float radiusAttack = 2;
    [Header("偵測地板的位移與半徑")]
    public Vector3 groundOffset;
    public float groundRadius = 0.1f;
    [Header("攻擊區域位移與尺寸")]
    public Vector3 attackOffset;
    public Vector3 attackSize;
    [Header("掉落道具")]
    public GameObject prop;
    [Header("掉落機率"), Range(0f, 1f)]
    public float propProbility = 0.5f;

    private Rigidbody2D rig;
    /// <summary>
    /// 原始速度
    /// </summary>
    private float speedOriginal;

    protected Transform player;
    protected Animator ani;
    /// <summary>
    /// 計時器：紀錄攻擊冷卻
    /// </summary>
    protected float timer;
    #endregion

    #region 事件
    protected virtual void Start()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();

        // 玩家 = 遊戲物件.尋找("物件名稱) - 搜尋場景內所有物件
        // transform.Find("子物件名稱") - 搜尋此物件的子物件
        player = GameObject.Find("玩家").transform;
        
        timer = cd;                 // 讓敵人一開始就進行攻擊
        speedOriginal = speed;      // 取得原始速度
    }

    private void OnDrawGizmos()
    {
        #region 繪製距離與檢查地板
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, radiusTrack);

        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, radiusAttack);

        Gizmos.color = new Color(0.6f, 0.9f, 1, 0.7f);
        Gizmos.DrawSphere(transform.position + transform.right * groundOffset.x + transform.up * groundOffset.y, groundRadius);
        #endregion

        #region 繪製攻擊區域
        Gizmos.color = new Color(0.3f, 0.3f, 1, 0.8f);
        Gizmos.DrawCube(transform.position + transform.right * attackOffset.x + transform.up * attackOffset.y, attackSize);
        #endregion
    }

    private void Update()
    {
        Move();
    }
    #endregion

    #region 方法
    /// <summary>
    /// 移動：偵測是否進入追蹤範圍
    /// </summary>
    private void Move()
    {
        // 如果 死亡 就 跳出
        if (ani.GetBool("死亡開關")) return;

        // 距離 = 三維向量.距離(A 點，B 點)
        float dis = Vector3.Distance(player.position, transform.position);

        // 如果 距離 小於等於 攻擊範圍 就攻擊
        if (dis <= radiusAttack)
        {
            Attack();
            LookAtPlayer();
        }
        // 否則 如果 玩家跟敵人 的 距離 小於等於 追蹤範圍 就移動
        else if (dis <= radiusTrack)
        {
            rig.velocity = transform.right * speed * Time.deltaTime;
            ani.SetBool("走路開關", speed != 0);                        // 速度不等於零時 走路 否則 等待
            LookAtPlayer();
            CheckGround();
        }
        else
        {
            ani.SetBool("走路開關", false);
        }
    }

    /// <summary>
    /// 攻擊
    /// </summary>
    private void Attack()
    {
        ani.SetBool("走路開關", false);

        // 如果 計時器 <= 攻擊冷卻 就累加
        if (timer <= cd)
        {
            timer += Time.deltaTime;
        }
        // 否則 攻擊 並將計時器歸零
        else AttackState();
    }

    /// <summary>
    /// 攻擊狀態 - 根據階段選擇要攻擊的方式
    /// </summary>
    protected virtual void AttackState()
    {
        timer = 0;
        ani.SetTrigger("攻擊觸發");
        // 碰撞物件 = 2D 物理.覆蓋盒形(中心點，尺寸，角度)
        Collider2D hit = Physics2D.OverlapBox(transform.position + transform.right * attackOffset.x + transform.up * attackOffset.y, attackSize, 0);
        // 如果 碰撞物件存在 並且 名稱是玩家 就對玩家 呼叫 受傷 方法
        if (hit && hit.name == "玩家") hit.GetComponent<Player>().Hit(attack);
    }

    /// <summary>
    /// 面向玩家
    /// </summary>
    private void LookAtPlayer()
    {
        // 如果 敵人 x 大於 玩家 x 就代表玩家在左邊 角度 180
        if (transform.position.x > player.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        // 否則 敵人 x 小於 玩家 x 就代表玩家在右邊 角度 0
        else
        {
            transform.eulerAngles = Vector3.zero;
        }
    }

    /// <summary>
    /// 檢查前方是否有地板
    /// </summary>
    private void CheckGround()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position + transform.right * groundOffset.x + transform.up * groundOffset.y, groundRadius, 1 << 8);

        // 判斷式 程式只有一句 (一個分號) 可以省略 大括號
        if (hit && (hit.name == "地板" || hit.name == "跳台")) speed = speedOriginal;
        else speed = 0;
    }

    /// <summary>
    /// 死亡
    /// </summary>
    protected virtual void Dead()
    {
        ani.SetBool("死亡開關", true);
        rig.Sleep();                                            // 剛體 睡著 - 避免飄移
        rig.constraints = RigidbodyConstraints2D.FreezeAll;     // 剛體 凍結全部
        GetComponent<CapsuleCollider2D>().enabled = false;      // 碰撞器 關閉
        Destroy(gameObject, 2);                                 // 刪除
        Prop();
    }

    /// <summary>
    /// 掉落道具
    /// </summary>
    private void Prop()
    {
        float r = Random.value;         // 取得隨機值 0 ~ 1

        if (r <= propProbility) Instantiate(prop, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// 受傷
    /// </summary>
    /// <param name="damage">接收到的傷害</param>
    public virtual void Hit(float damage)
    {
        hp -= damage;

        // 判斷式 只有一個分號 可以省略 大括號
        if (hp <= 0) Dead();
    }
    #endregion
}
