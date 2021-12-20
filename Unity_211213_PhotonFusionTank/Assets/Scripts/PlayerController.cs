using Fusion;
using UnityEngine;

/// <summary>
/// 玩家坦克控制器
/// 一般移動、旋轉砲塔與發射子彈
/// </summary>
public class PlayerController : NetworkBehaviour
{
    #region Field
    [Header("移動速度"), Range(0, 100)]
    public float speed = 7.5f;
    [Header("子彈發射頻率"), Range(0, 1.5f)]
    public float interalFire = 0.35f;
    [Header("子彈物件")]
    public GameObject bullet;

    private NetworkCharacterController ncc;
    #endregion

    #region Event
    private void Awake()
    {
        ncc = GetComponent<NetworkCharacterController>();
    }
    #endregion

    #region Method
    public override void FixedUpdateNetwork()
    {
        Move();
    }

    private void Move()
    {
        if(GetInput(out NetworkInputData dataInput))
        {
            ncc.Move(speed * dataInput.direction * Runner.DeltaTime);
        }
    }
    #endregion
}
