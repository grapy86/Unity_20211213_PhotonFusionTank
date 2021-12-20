using Fusion;
using UnityEngine;

/// <summary>
/// 連線輸入資料
/// 保存(所有個別)連線玩家輸入資訊
/// </summary>
public struct NetworkInputData : INetworkInput
{
    // 移動方向
    public Vector3 direction;
    // 是否開火
    public bool inputFire;
}
