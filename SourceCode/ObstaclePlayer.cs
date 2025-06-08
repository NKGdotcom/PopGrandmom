using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstaclePlayer : MoveLimited
{
    [Header("邪魔者の動き")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float fitDistance = 0.02f;
    [SerializeField] private Animator grandMomAnimator;
    private Vector3 moveX = new Vector3(1f, 0f, 0f);
    private Vector3 moveZ = new Vector3(0f, 0f, 1f);
    private Vector3 moveDir;
    private Vector3 targetPos;

    private Rigidbody enemyRb;

    private bool stopMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPos = transform.position;

        enemyRb = GetComponent<Rigidbody>();
        enemyRb.isKinematic = true;
        grandMomAnimator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        float _distance = (transform.position - targetPos).sqrMagnitude;
        if(_distance < fitDistance)
        {
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y, Mathf.RoundToInt(transform.position.z));

            TargetPosition();
        }
        if (!SpeakerState.Instance.IsSpeakerRotating) return;
        Move();

    }
    /// <summary>
    /// ターゲットの位置を決める
    /// </summary>
    private void TargetPosition()
    {
        List<Vector3> _possibleMoves = new List<Vector3>
    {
        moveX,
        -moveX,
        moveZ,
        -moveZ
    };
        // 有効な方向のみを残す
        _possibleMoves = _possibleMoves
            .Where(dir => IsWithinBounds(transform.position + dir))
            .ToList();

        if (_possibleMoves.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            // ランダムな方向を選ぶ
            moveDir = _possibleMoves[Random.Range(0, _possibleMoves.Count)];
            targetPos = transform.position + moveDir;
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }
    }
    public void PlayAnimation()
    {
        grandMomAnimator.enabled = true;
        SoundManager.Instance.PlaySE(SESource.hitGrandmom);
    }
}
