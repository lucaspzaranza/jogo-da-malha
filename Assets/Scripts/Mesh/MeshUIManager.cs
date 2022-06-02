using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshUIManager : MonoBehaviour
{
    public static MeshUIManager instance;

    [Header("Sprites to the tries slots.")]
    [SerializeField] private Sprite _filledSlot;
    public Sprite FilledSlot => _filledSlot;

    [SerializeField] private Sprite _emptySlot;
    public Sprite EmptySlot => _emptySlot;

    [SerializeField] private GameObject _gameOverScreen;
    public GameObject GameOverScreen => _gameOverScreen;

    [SerializeField] private GameObject _successScreen;
    public GameObject SuccessScreen => _successScreen;

    [SerializeField] private GameObject _endGameScreen;
    public GameObject EndGameScreen => _endGameScreen;

    [SerializeField] private List<Image> _meshesSlots;
    public IReadOnlyList<Image> MeshesSlots => _meshesSlots;

    private int SlotIndex => (MeshManager.instance.MaxTries - MeshManager.instance.GetActiveGameMesh().RemainingTries) - 1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        Invoke(nameof(EventHandlerSetup), 0.1f);
    }
    private void OnDisable()
    {
        EventHandlerDispose();
    }

    private void EventHandlerSetup()
    {
        GameMesh.OnEndGame += HandleOnEndGame;
    }

    private void EventHandlerDispose()
    {
        GameMesh.OnEndGame -= HandleOnEndGame;
    }

    private void HandleOnEndGame(int remainingTries)
    {
        SetDPadButtonsInteractable(false);
        StartCoroutine(SetSuccesscreenActivation(true, 2f));
    }

    public void SetDPadButtonsInteractable(bool val)
    {
        var dPadBtns = MeshManager.instance.GetActiveGameMesh().DPadButtons;
        foreach (var btn in dPadBtns)
        {
            btn.interactable = val;
        }
    }

    public void HandleNumOfTriesDecremented(int delta)
    {
        if (delta == 1)
            EmptySingleSlot(SlotIndex);
        else if (delta > 1)
            EmptyRangeSlots(delta);
    }

    private void EmptySingleSlot(int index)
    {
        MeshesSlots[index].sprite = EmptySlot;
    }

    private void EmptyRangeSlots(int length)
    {
        for (int i = 0; i < length; i++)
        {
            MeshesSlots[i].sprite = EmptySlot;
        }
    }

    public void ResetSlots()
    {
        for (int i = 0; i < MeshesSlots.Count; i++)
        {
            MeshesSlots[i].sprite = FilledSlot;
        }
    }

    public void CallResetMatch()
    {
        SetDPadButtonsInteractable(true);
        MeshManager.instance.ResetMatch();
    }

    public IEnumerator SetGameOverScreenActivation(bool val, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        GameOverScreen.SetActive(val);
    }

    public IEnumerator SetSuccesscreenActivation(bool val, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        SuccessScreen.SetActive(val);
    }

    public void RetryMatch()
    {
        StartCoroutine(SetGameOverScreenActivation(false, 0f));
        CallResetMatch();
    }

    private void SetActiveMeshScreenActivation(bool val)
    {
        MeshManager.instance.GetActiveGameMesh().transform.parent.gameObject.SetActive(val);
    }

    private void CallEndGameScreenActivation()
    {
        EndGameScreen.SetActive(true);
    }

    public void GetNextMesh()
    {
        SetActiveMeshScreenActivation(false);
        StartCoroutine(SetSuccesscreenActivation(false, 0f));
        CallResetMatch();
        MeshManager.instance.IncrementMeshIndex();
        if (MeshManager.instance.CurrentMeshIndex < MeshManager.instance.GameMeshes.Count)
            SetActiveMeshScreenActivation(true);
        else
            CallEndGameScreenActivation();
    }
}
