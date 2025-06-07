using UnityEngine;

public class MoveLimited : MonoBehaviour
{
    [Header("ˆÚ“®§ŒÀ")]
    [SerializeField] protected int minX;
    [SerializeField] protected int maxX;
    [SerializeField] protected int minZ;
    [SerializeField] protected int maxZ;

    /// <summary>
    /// w’è‚µ‚½À•W‚ª”ÍˆÍ“à‚©‚Ç‚¤‚©‚ğ”»’è
    /// </summary>
    protected bool IsWithinBounds(Vector3 position)
    {
        return position.x >= minX && position.x <= maxX &&
               position.z >= minZ && position.z <= maxZ;
    }
}
