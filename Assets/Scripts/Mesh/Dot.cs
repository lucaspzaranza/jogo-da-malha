using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [Header("Connected directions by a line")]
    public bool Up;
    public bool Down;
    public bool Left;
    public bool Right;

    public GameObject dotImg;

    private void OnEnable()
    {
        Invoke(nameof(SetOnGameMeshResetHandler), 0.5f);
    }

    private void OnDisable()
    {
        GameMesh.OnGameMeshReset -= HandleOnGameMeshReset;
    }

    private void SetOnGameMeshResetHandler()
    {
        GameMesh.OnGameMeshReset += HandleOnGameMeshReset;
    }

    private void HandleOnGameMeshReset()
    {
        Up = false;
        Down = false;
        Left = false;
        Right = false;

        if (transform.childCount > 0)
            transform.GetChild(0).gameObject.SetActive(false);

        // Se > 1, tem uma linha como child do ponto
        if (transform.childCount > 1)
        {
            var line = transform.GetChild(1).gameObject;
            Destroy(line);
        }
    }
}
