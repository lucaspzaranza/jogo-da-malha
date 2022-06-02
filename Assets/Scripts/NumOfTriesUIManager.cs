using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NumOfTriesUIManager : MonoBehaviour
{
    [System.Serializable]
    public struct NumberSprite
    {
        public int Number;
        public Sprite SpriteImage;
    }

    [SerializeField] private Image _spriteImg;
    [SerializeField] private List<NumberSprite> _numbersSprite;

    private void OnEnable()
    {
        Invoke(nameof(EventHandlerSetup), 0.1f);
    }

    private void OnDisable()
    {
        GameMesh.OnEndGame -= HandleOnEndGame;
    }

    private void EventHandlerSetup()
    {
        GameMesh.OnEndGame += HandleOnEndGame;
    }

    private void HandleOnEndGame(int remainingTries)
    {
        _spriteImg.sprite = _numbersSprite.SingleOrDefault(num => num.Number == remainingTries).SpriteImage;
    }
}
