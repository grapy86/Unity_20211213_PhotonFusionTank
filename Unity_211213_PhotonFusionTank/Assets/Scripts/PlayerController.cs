using Fusion;
using UnityEngine;

/// <summary>
/// ���a�Z�J���
/// �@�벾�ʡB���௥��P�o�g�l�u
/// </summary>
public class PlayerController : NetworkBehaviour
{
    #region Field
    [Header("���ʳt��"), Range(0, 100)]
    public float speed = 7.5f;
    [Header("�l�u�o�g�W�v"), Range(0, 1.5f)]
    public float interalFire = 0.35f;
    [Header("�l�u����")]
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
