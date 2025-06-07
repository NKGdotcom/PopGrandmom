using UnityEngine;

public class MusicNotePower : MonoBehaviour
{
    [SerializeField] private float noteSpeed = 30f;
    [SerializeField] private float destroyTime = 2f;
    private Vector3 shotDirection;
    private Rigidbody noteRb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        noteRb = GetComponent<Rigidbody>();
        ShotNote();
    }
    /// <summary>
    /// ”ò‚Î‚·•ûŒü‚ðŒˆ‚ß‚é
    /// </summary>
    /// <param name="_dir"></param>
    public void SetDirection(Vector3 _dir)
    {
        shotDirection = _dir.normalized;
    }
    /// <summary>
    /// ‰¹•„‚ð”ò‚Î‚·
    /// </summary>
    private void ShotNote()
    {
        noteRb.AddForce(shotDirection * noteSpeed, ForceMode.Impulse);

        Destroy(this.gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider _col)
    {
        if (_col.gameObject.CompareTag("Wall"))
        {
            Enthusiasm.Instance.UpEnthsiasm();
            Destroy(this.gameObject);
        }
        if (_col.gameObject.CompareTag("GrandMom"))
        {
            GameResult.Instance.Result().Forget();
        }
    }
}
