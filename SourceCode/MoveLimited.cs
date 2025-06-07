using UnityEngine;

public class MoveLimited : MonoBehaviour
{
    [Header("�ړ�����")]
    [SerializeField] protected int minX;
    [SerializeField] protected int maxX;
    [SerializeField] protected int minZ;
    [SerializeField] protected int maxZ;

    /// <summary>
    /// �w�肵�����W���͈͓����ǂ����𔻒�
    /// </summary>
    protected bool IsWithinBounds(Vector3 position)
    {
        return position.x >= minX && position.x <= maxX &&
               position.z >= minZ && position.z <= maxZ;
    }
}
