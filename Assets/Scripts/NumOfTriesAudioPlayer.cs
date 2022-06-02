using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumOfTriesAudioPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _numOfTriesAudios;
    public IReadOnlyList<AudioClip> NumOfTriesAudios => _numOfTriesAudios;

    private void OnEnable()
    {
        int index = MeshManager.instance.MaxTries - (MeshManager.instance.GetActiveGameMesh().RemainingTries + 1); // + 1 to 0 based index
        AudioManager.instance.PlayAudio(NumOfTriesAudios[index]);
    }
}
