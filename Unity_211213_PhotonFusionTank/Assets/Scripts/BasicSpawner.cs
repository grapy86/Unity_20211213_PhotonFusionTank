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
    [Header("�ЫػP�[�J�ж����")]
    public InputField ipfCreateRoom;
    public InputField ipfJoinRoom;
    [Header("���a�����")]
    public NetworkPrefabRef[] goPlayer;
    [Header("�s�u�e���e��")]
    public GameObject goCanvas;
    [Header("������")]
    public Text txtVersion;
    [Header("���a�ͦ���m")]
    public Transform[] traSpawnPoints;

    // ���a��J���ж��W��
    private string roomNameInput;
    // �s�u���澹
    private NetworkRunner runner;
    private string stringVersion = "Coffee Copyright 2022. | Version ";
    // ���a��ƶ��X�G�ѦҸ�T�B�s�u����
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
    /// �}�l�s�u�C��
    /// </summary>
    /// <param name="mode">�s�u�Ҧ��GHost, Client</param>
    // async �D�P�B�B�z�G����t�ήɳB�z�s�u
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

        #region �۩w�q��J����P���ʸ�T
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
    /// ���a���\�[�J�ж�
    /// </summary>
    /// <param name="runner">�s�u���澹</param>
    /// <param name="player">���a��T</param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // �H���ͦ��I
        int randomSpawnPoint = UnityEngine.Random.Range(0, traSpawnPoints.Length);
        int randomGoPlayer = UnityEngine.Random.Range(0, goPlayer.Length);
        // �s�u���澹 �ͦ����a����P��T
        NetworkObject playerNO = runner.Spawn(goPlayer[randomGoPlayer], traSpawnPoints[randomSpawnPoint].position, Quaternion.identity, player);
        players.Add(player, playerNO);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // �p�G "���}�����a�s�u����" "�s�b" �Y�R���Ӫ���
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
