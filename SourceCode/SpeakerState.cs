using UnityEngine;

public class SpeakerState : MonoBehaviour
{
    public static SpeakerState Instance { get; private set; }

    public bool IsSpeakerRotating {  get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("instance = null");
            Instance = this;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    public void IsRotating(bool _isRotating)
    {
        IsSpeakerRotating = _isRotating;
    }
}
