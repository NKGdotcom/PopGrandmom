using UnityEngine;

public class MoveLimited : MonoBehaviour
{
    [Header("移動制限")]
    [SerializeField] protected int minX;
    [SerializeField] protected int maxX;
    [SerializeField] protected int minZ;
    [SerializeField] protected int maxZ;

    /// <summary>
    /// 指定した座標が範囲内かどうかを判定
    /// </summary>
    protected bool IsWithinBounds(Vector3 position)
    {
        return position.x >= minX && position.x <= maxX &&
               position.z >= minZ && position.z <= maxZ;
    }
}
