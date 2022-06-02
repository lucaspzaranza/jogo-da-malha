using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Direction
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
}

public class MeshManager : MonoBehaviour
{
    public static MeshManager instance;

    [SerializeField] private bool _limitedTries;
    public bool LimitedTries => _limitedTries;

    [SerializeField] private List<GameMesh> _gameMeshes;
    public IReadOnlyList<GameMesh> GameMeshes => _gameMeshes;

    [SerializeField] private int _currentMeshIndex;
    public int CurrentMeshIndex => _currentMeshIndex;

    [SerializeField] private int _maxTries;
    public int MaxTries => _maxTries;

    [SerializeField] private GameObject _horizontal;
    public GameObject Horizontal => _horizontal;
    
    [SerializeField] private GameObject _vertical;
    public GameObject Vertical => _vertical;

    [SerializeField] private Sprite _slot;
    public Sprite Slot => _slot;

    [SerializeField] private Sprite _emptySlot;
    public Sprite EmptySlot => _emptySlot;

    [SerializeField] private List<AudioClip> _directionAudios;
    public IReadOnlyList<AudioClip> DirectionAudios => _directionAudios;

    private Direction currentDirection;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        Invoke(nameof(SettleNumOfTriesCallback), 0.1f);
    }

    private void OnDisable()
    {
        GameMesh.OnNumOfTriesDecremented -= MeshUIManager.instance.HandleNumOfTriesDecremented;
        GameMesh.OnNumOfTriesDecremented -= GameOverVerification;
    }

    private void SettleNumOfTriesCallback()
    {
        GameMesh.OnNumOfTriesDecremented += MeshUIManager.instance.HandleNumOfTriesDecremented;
        GameMesh.OnNumOfTriesDecremented += GameOverVerification;
    }

    public void IncrementMeshIndex()
    {
        _currentMeshIndex++;
    }

    private void GameOverVerification(int delta)
    {
        var gameMesh = GetActiveGameMesh();

        if (!gameMesh.ReachedGoal && gameMesh.RemainingTries == 0)
        {
            MeshUIManager.instance.SetDPadButtonsInteractable(false);
            StartCoroutine(MeshUIManager.instance.SetGameOverScreenActivation(true, 2f));
        }
    }

    public void MoveOnMesh(int directionInt)
    {
        if(!LimitedTries || (LimitedTries && GetActiveGameMesh().RemainingTries > 0))
        {
                        currentDirection = (Direction)directionInt;
            if (GameMeshes[CurrentMeshIndex].CanStep(currentDirection))
            {
                GameMeshes[CurrentMeshIndex].MoveStep(currentDirection);
                AudioManager.instance.PlayAudio(DirectionAudios[(int)currentDirection]);
            }
            else
                Debug.LogWarning("Can't move there!");
        }
    }

    public GameMesh GetActiveGameMesh()
    {
        return GameMeshes[CurrentMeshIndex];
    }

    public void ResetMatch()
    {
        GetActiveGameMesh().ResetGameMesh();
        MeshUIManager.instance.ResetSlots();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
