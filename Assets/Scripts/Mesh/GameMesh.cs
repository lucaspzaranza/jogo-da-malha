using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMesh : MonoBehaviour
{
    [SerializeField] int _minSteps;
    public int MinSteps => _minSteps;

    [SerializeField] private int _rows;
    [SerializeField] private int _columns;
    
    [SerializeField] private Vector2Int currentDotPos;
    [SerializeField] private Vector2Int goalDotPos;

    public float horLinePos;
    public float verLinePos;

    private Dot[,] dotsMatrix;
    private Vector2Int initDotPos;

    [SerializeField] private List<Dot> _dots;
    public IReadOnlyList<Dot> Dots => _dots;

    private int _remainingTries;
    public int RemainingTries
    {
        get => _remainingTries;
        private set
        {
            if(value > -1 && value < 11)
            {
                int delta = _remainingTries - value;
                _remainingTries = Mathf.Clamp(value, 0, MeshManager.instance.MaxTries);
                OnNumOfTriesDecremented(delta);
            }
        }
    }

    public delegate void NumOfTriesDecremented(int delta);
    public static event NumOfTriesDecremented OnNumOfTriesDecremented;

    public delegate void GameMeshReset();
    public static event GameMeshReset OnGameMeshReset;

    void Start()
    {
        _remainingTries = MeshManager.instance.MaxTries;
        initDotPos = currentDotPos;
        ArrayToMatrix();
    }

    private void ArrayToMatrix()
    {
        dotsMatrix = new Dot[_rows, _columns];
        int row = 0;

        for (int i = 0; i < _dots.Count; i++)
        {
            int index = i % _columns;
            if (i > 0 && index == 0)
                row++;

            dotsMatrix[row, index] = _dots[i];
        }
    }

    public void ResetGameMesh()
    {
        _remainingTries = MeshManager.instance.MaxTries;
        currentDotPos = initDotPos;
        OnGameMeshReset?.Invoke();
    }

    public bool HasLine(Direction direction)
    {
        int x = currentDotPos.x;
        int y = currentDotPos.y;
        
        switch (direction)
        {
            case Direction.Up:
                return dotsMatrix[x, y].Up && dotsMatrix[x - 1, y].Down;
            case Direction.Down:
                return dotsMatrix[x, y].Down && dotsMatrix[x + 1, y].Up;
            case Direction.Left:
                return dotsMatrix[x, y].Left && dotsMatrix[x, y - 1].Right;
            case Direction.Right:
                return dotsMatrix[x, y].Right && dotsMatrix[x, y + 1].Left;
            default:
                return true;
        }
    }

    public bool CanStep(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:

                return currentDotPos.x - 1 >= 0 && !HasLine(direction);

            case Direction.Down:
                return currentDotPos.x + 1 < _rows && !HasLine(direction);

            case Direction.Left:
                return currentDotPos.y - 1 >= 0 && !HasLine(direction);

            case Direction.Right:
                return currentDotPos.y + 1 < _columns && !HasLine(direction);

            default:
                return false;
        }
    }

    public void MoveStep(Direction direction)
    {
        int x = currentDotPos.x;
        int y = currentDotPos.y;
        int dirInt = (int)direction;
        GameObject line = null;

        dotsMatrix[x, y].dotImg.SetActive(true);
        if (dirInt < 2) // Up or down
        {
            if(dirInt == 0) // Up
            {
                //line = Instantiate(MeshManager.instance.LineUp);
                line = Instantiate(MeshManager.instance.Vertical,
                    new Vector2(dotsMatrix[x, y].transform.position.x, verLinePos),
                    MeshManager.instance.Vertical.transform.rotation);
                dotsMatrix[x, y].Up = true;
                dotsMatrix[x - 1, y].Down = true;
                dotsMatrix[x - 1, y].dotImg.SetActive(true);
                currentDotPos.x--;
            }
            else // Down
            {
                //line = Instantiate(MeshManager.instance.LineDown);
                line = Instantiate(MeshManager.instance.Vertical, 
                    new Vector2(dotsMatrix[x, y].transform.position.x, -verLinePos), 
                    MeshManager.instance.Vertical.transform.rotation);
                dotsMatrix[x, y].Down = true;
                dotsMatrix[x + 1, y].Up = true;
                dotsMatrix[x + 1, y].dotImg.SetActive(true);
                currentDotPos.x++;
            }
        }
        else // Left or right
        {
            if (dirInt == 2) // Left
            {
                //line = Instantiate(MeshManager.instance.LineLeft);
                line = Instantiate(MeshManager.instance.Horizontal,
                    new Vector2(-horLinePos, dotsMatrix[x, y].transform.position.y),
                    MeshManager.instance.Horizontal.transform.rotation);
                dotsMatrix[x, y].Left = true;
                dotsMatrix[x, y - 1].Right = true;
                dotsMatrix[x, y - 1].dotImg.SetActive(true);
                currentDotPos.y--;
            }
            else // Right
            {
                //line = Instantiate(MeshManager.instance.LineRight);
                line = Instantiate(MeshManager.instance.Horizontal,
                    new Vector2(horLinePos, dotsMatrix[x, y].transform.position.y),
                    MeshManager.instance.Horizontal.transform.rotation);
                dotsMatrix[x, y].Right = true;
                dotsMatrix[x, y + 1].Left = true;
                dotsMatrix[x, y + 1].dotImg.SetActive(true);
                currentDotPos.y++;
            }
        }

        if(line != null)
        {
            line.transform.SetParent(dotsMatrix[x, y].transform, false);
            Vector2 pos = line.transform.localPosition;

            if(dirInt < 2)
                line.transform.localPosition = new Vector3(0f, pos.y, 0f);
            else
                line.transform.localPosition = new Vector3(pos.x, 0f, 0f);

            RemainingTries--;
            GoalPointReachedVerification();
        }
    }

    private void GoalPointReachedVerification()
    {
        if (currentDotPos == goalDotPos)
            print("CHEGOU, CARAIO!");
    }
}