using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    #region Field
    [Header("移動速度"), Range(0, 100)]
    public float speed = 5;
    [Header("存在時間"), Range(0, 10)]
    public float existtime = 5;
    #endregion

    #region Property
    [Networked]
    private TickTimer exist { get; set; }
    #endregion

    #region Method
    public void Init()
    {
        exist = TickTimer.CreateFromSeconds(Runner, existtime);
    }
    #endregion
}
