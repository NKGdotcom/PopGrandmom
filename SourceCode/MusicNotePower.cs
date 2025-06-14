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
    /// 飛ばす方向を決める
    /// </summary>
    /// <param name="_dir"></param>
    public void SetDirection(Vector3 _dir)
    {
        shotDirection = _dir.normalized;
    }
    /// <summary>
    /// 音符を飛ばす
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
            SoundManager.Instance.PlaySE(SESource.hitWall);
            Destroy(this.gameObject);
        }
        if (_col.gameObject.CompareTag("GrandMom"))
        {
            GameResult.Instance.Result().Forget();
            _col.gameObject.GetComponent<ObstaclePlayer>().PlayAnimation();
        }
    }
}
