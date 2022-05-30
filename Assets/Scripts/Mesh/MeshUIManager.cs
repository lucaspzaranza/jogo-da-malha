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
        MeshManager.instance.ResetMatch();
    }
}
