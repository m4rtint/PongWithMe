using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PongWithMe
{
    [RequireComponent(typeof(Button))]
    public class StartGameButtonBehaviour : MonoBehaviour
    {
        private Button _startGameButton = null;
        
        public void Initialize(UnityAction startGameAction)
        {
            _startGameButton.onClick.RemoveAllListeners();
            _startGameButton.onClick.AddListener(startGameAction);
            ShowButton();
        }

        private void ShowButton()
        {
            transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
        }

        public void HideButton()
        {
            transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack);
        }

        private void Awake()
        {
            transform.localScale = Vector3.zero;
            _startGameButton = GetComponent<Button>();
        }
    }
}
