using Cysharp.Threading.Tasks;
using UnityEngine;


public class SpeakerPlayerMove : MoveLimited
{
    [Header("プレイヤーの動きについて")]
    [SerializeField] private float rotSpeed = 8f;
    [SerializeField] private int moveFiringNum = 3;
    private Vector3 moveToXPositive = new Vector3(1, 0, 0);
    private Vector3 moveToXNegative = new Vector3(-1, 0, 0);
    private Vector3 moveToZPositive = new Vector3(0, 0, 1);
    private Vector3 moveToZNegative = new Vector3(0, 0, -1);

    private Vector3 rotPoint = Vector3.zero;
    private Vector3 rotAxis = Vector3.zero;

    [SerializeField] private int grandMomInstantiateNum = 12;
    private int moveNum;
    private float rotAngle;
    private float rightAngle = 90f;

    private float halfCubeSize;
    private bool isRotate = false;

    [Header("スピーカーの音を出す")]
    [SerializeField] private GameObject musicInstantFrontPos;
    [SerializeField] private GameObject musicInstantRightPos;
    [SerializeField] private GameObject musicInstantLeftPos;
    [SerializeField] private GameObject musicNote;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        halfCubeSize = transform.localScale.x/2f;
        rotAngle = rotSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotate) return;

        KeyInput();

        if (rotPoint == Vector3.zero) return;

        MoveSpeaker().Forget();
    }
    /// <summary>
    /// キー入力
    /// </summary>
    private void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector3 nextPos = transform.position + moveToXPositive;
            if (!IsWithinBounds(nextPos)) return;
            rotPoint = transform.position + new Vector3(halfCubeSize,-halfCubeSize, 0f);
            rotAxis = moveToZNegative;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector3 nextPos = transform.position + moveToXNegative;
            if (!IsWithinBounds(nextPos)) return;
            rotPoint = transform.position + new Vector3(-halfCubeSize, -halfCubeSize, 0f);
            rotAxis = moveToZPositive;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector3 nextPos = transform.position + moveToZPositive;
            if (!IsWithinBounds(nextPos)) return;
            rotPoint = transform.position + new Vector3(0f, -halfCubeSize, halfCubeSize);
            rotAxis = moveToXPositive;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector3 nextPos = transform.position + moveToZNegative;
            if (!IsWithinBounds(nextPos)) return;
            rotPoint = transform.position + new Vector3(0f, -halfCubeSize, -halfCubeSize);
            rotAxis = moveToXNegative;
        }
    }
    /// <summary>
    /// スピーカーの移動
    /// </summary>
    /// <returns></returns>
    private async UniTaskVoid MoveSpeaker()
    {
        isRotate = true;
        SpeakerState.Instance.IsRotating(isRotate);
        float _angle = 0f;
        moveNum++;
        while(_angle < rightAngle)
        {
            _angle += rotAngle;
            if(_angle > rightAngle)
            {
                rotAngle -= _angle - rightAngle;
            }
            transform.RotateAround(rotPoint, rotAxis, rotAngle);
            rotAngle = rotSpeed;
            await UniTask.Yield();
        }
        isRotate = false;
        SpeakerState.Instance.IsRotating(isRotate);
        rotPoint = Vector3.zero;
        rotAxis = Vector3.zero;
        if (moveNum % moveFiringNum == 0)
        {
            FiringMusicNote();
        }
        if(moveNum % grandMomInstantiateNum == 0)
        {
            Debug.Log("おばあちゃん生成");
            InstantiateGrandMom.Instance.InstantiateGrandMother();
        }
    }
    /// <summary>
    /// 音符を生成する準備
    /// </summary>
    private void FiringMusicNote()
    {
        
        GameObject _musicNote = musicNote;
        if (musicInstantFrontPos !=null && musicInstantLeftPos != null || musicInstantRightPos != null)
        {
            InstantiateMusicNote(_musicNote, musicInstantFrontPos.transform);
            InstantiateMusicNote(_musicNote, musicInstantLeftPos.transform);
            InstantiateMusicNote(_musicNote, musicInstantRightPos.transform);
        } 
    }
    /// <summary>
    /// 音符を生成
    /// </summary>
    /// <param name="_musicNote"></param>
    /// <param name="_position"></param>
    private void InstantiateMusicNote(GameObject _musicNote, Transform _position)
    {
        GameObject _note = Instantiate(_musicNote,_position.position,Quaternion.identity);
        MusicNotePower _notePower = _note.GetComponent<MusicNotePower>();
        _notePower.SetDirection(_position.right);
    }
}
