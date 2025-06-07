using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameResult : MonoBehaviour
{
    public static GameResult Instance { get; private set; }

    [SerializeField] private GameObject GameResultUIl;
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
    public async UniTaskVoid Result()
    {

    }
}
