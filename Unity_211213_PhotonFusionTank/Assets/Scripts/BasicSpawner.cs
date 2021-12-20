using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    #region Field
    [Header("創建與加入房間欄位")]
    public InputField ipfCreateRoom;
    public InputField ipfJoinRoom;
    [Header("玩家控制物件")]
    public NetworkPrefabRef[] goPlayer;
    [Header("連線畫面畫布")]
    public GameObject goCanvas;
    [Header("版本號")]
    public Text txtVersion;
    [Header("玩家生成位置")]
    public Transform[] traSpawnPoints;

    // 玩家輸入的房間名稱
    private string roomNameInput;
    // 連線執行器
    private NetworkRunner runner;
    private string stringVersion = "Coffee Copyright 2022. | Version ";
    // 玩家資料集合：參考資訊、連線物件
    private Dictionary<PlayerRef, NetworkObject> players = new Dictionary<PlayerRef, NetworkObject>();
    #endregion

    #region
    private void Awake()
    {
        txtVersion.text = stringVersion + Application.version;
    }
    #endregion

    #region Method
    public void BtnCreateRoom()
    {
        roomNameInput = ipfCreateRoom.text;
        StartGame(GameMode.Host);
    }

    public void BtnJoinRoom()
    {
        roomNameInput = ipfJoinRoom.text;
        StartGame(GameMode.Client);
    }

    /// <summary>
    /// 開始連線遊戲
    /// </summary>
    /// <param name="mode">連線模式：Host, Client</param>
    // async 非同步處理：執行系統時處理連線
    private async void StartGame(GameMode mode)
    {
        runner = gameObject.AddComponent<NetworkRunner>();
        runner.ProvideInput = true;

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomNameInput,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneObjectProvider = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        goCanvas.SetActive(false);
    }
    #endregion

    #region Fusion Callbacks
    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {

    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData inputData = new NetworkInputData();

        #region 自定義輸入按鍵與移動資訊
        if (Input.GetKey(KeyCode.W)) inputData.direction += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) inputData.direction += Vector3.back;
        if (Input.GetKey(KeyCode.A)) inputData.direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) inputData.direction += Vector3.right;

        inputData.inputFire = Input.GetKey(KeyCode.Mouse0);
        #endregion

        input.Set(inputData);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    /// <summary>
    /// 當玩家成功加入房間
    /// </summary>
    /// <param name="runner">連線執行器</param>
    /// <param name="player">玩家資訊</param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // 隨機生成點
        int randomSpawnPoint = UnityEngine.Random.Range(0, traSpawnPoints.Length);
        int randomGoPlayer = UnityEngine.Random.Range(0, goPlayer.Length);
        // 連線執行器 生成玩家物件與資訊
        NetworkObject playerNO = runner.Spawn(goPlayer[randomGoPlayer], traSpawnPoints[randomSpawnPoint].position, Quaternion.identity, player);
        players.Add(player, playerNO);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // 如果 "離開的玩家連線物件" "存在" 即刪除該物件
        if (players.TryGetValue(player, out NetworkObject playerNO))
        {
            runner.Despawn(playerNO);
            players.Remove(player);
        }
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }
    #endregion
}
