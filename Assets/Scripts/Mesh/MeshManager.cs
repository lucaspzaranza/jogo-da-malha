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

    public List<GameMesh> gameMeshes;
    public int currentMesh;

    [SerializeField] private GameObject _horizontal;
    public GameObject Horizontal => _horizontal;
    
    [SerializeField] private GameObject _vertical;
    public GameObject Vertical => _vertical;

    Direction currentDirection;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void MoveOnMesh(int directionInt)
    {
        currentDirection = (Direction)directionInt;
        if (gameMeshes[currentMesh].CanStep(currentDirection))
        {
            print("MOVE!");
            gameMeshes[currentMesh].MoveStep(currentDirection);
        }
        else
            print("ERROR!");
    }
}
