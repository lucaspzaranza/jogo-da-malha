using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Invoke(nameof(SettleNumOfTriesCallback), 0.5f);
    }

    private void OnDisable()
    {
        GameMesh.OnNumOfTriesDecremented -= MeshUIManager.instance.HandleNumOfTriesDecremented;
        GameMesh.OnNumOfTriesDecremented -= EndMatchVerification;
    }

    private void SettleNumOfTriesCallback()
    {
        GameMesh.OnNumOfTriesDecremented += MeshUIManager.instance.HandleNumOfTriesDecremented;
        GameMesh.OnNumOfTriesDecremented += EndMatchVerification;
    }

    private void EndMatchVerification(int delta)
    {
        if (GetActiveGameMesh().RemainingTries == 0)
            print("Game over");
    }

    public void MoveOnMesh(int directionInt)
    {
        currentDirection = (Direction)directionInt;
        if (GameMeshes[CurrentMeshIndex].CanStep(currentDirection))
            GameMeshes[CurrentMeshIndex].MoveStep(currentDirection);
        else
            Debug.LogWarning("Can't move there!");
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
}
