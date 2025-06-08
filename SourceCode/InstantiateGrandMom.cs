using System.Runtime.CompilerServices;
using UnityEngine;

public class InstantiateGrandMom : MoveLimited
{
    public static InstantiateGrandMom Instance { get; private set; }

    [SerializeField] private GameObject grandMomPrefab;
    [SerializeField] private float yPosition = 1;
    private int maxGrandMomNum = 10;
    private int nowGrandMomNum = 1;

    private int maxMovePositionDecideTime = 50;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InstantiateGrandMother()
    {
        if (maxGrandMomNum <= nowGrandMomNum) return;
        for (int i = 0; i < maxMovePositionDecideTime; i++)
        {
            Vector3 _spawnPos = new Vector3(Random.Range(minX, maxX + 1), yPosition, Random.Range(minZ, maxZ + 1));
            if (IsWithinBounds(_spawnPos) && IsPositionFree(_spawnPos))
            {
                Instantiate(grandMomPrefab, _spawnPos, Quaternion.identity);
                nowGrandMomNum++;
                break;
            }
        }
    }

    /// <summary>
    /// 他のオブジェクトと重なっていないかチェック
    /// </summary>
    private bool IsPositionFree(Vector3 _pos)
    {
        float checkRadius = 0.4f;
        Collider[] hit = Physics.OverlapSphere(_pos, checkRadius);
        return hit.Length == 0;
    }


}

