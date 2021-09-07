using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace PongWithMe
{
    public class PlayerScoreViewBehaviour : MonoBehaviour
    {
        private const float SHOW_SQUARE_ANIMATION = 0.5F;
        
        [Title("Prefab")]
        [SerializeField] private GameObject _winSquare = null;

        [Title("Components")] 
        [SerializeField] private Image _ownedByImage = null;
        [SerializeField] private HorizontalLayoutGroup _baseHorizontalLayoutGroup = null;
        [SerializeField] private HorizontalLayoutGroup _winsHorizontalLayoutGroup = null;

        private List<Image> _winSquares = new List<Image>();
        private int _currentWins = 0;

        public int CurrentWins => _currentWins;
        
        public void Initialize(Color color, int lives)
        {
            _ownedByImage.color = color;
            SetupSquares(lives);
        }

        public Tween WinNextSquare()
        {
            var square = _winSquares[_currentWins];
            square.color = _ownedByImage.color;
            var tween = square.transform
                .DOScale(Vector3.one, SHOW_SQUARE_ANIMATION)
                .SetEase(Ease.OutBack);
            UpdateCurrentWins();
            return tween;
        }

        private void UpdateCurrentWins()
        {
            _currentWins++;
        }

        private void SetupSquares(int lives)
        {
            for (int i = 0; i < lives; i++)
            {
                AddWinSquareToBaseLayoutGroup();
                AddBaseSquareToBaseLayoutGroup();
            }
        }

        private void AddWinSquareToBaseLayoutGroup()
        {
            var square = Instantiate(_winSquare, _winsHorizontalLayoutGroup.transform);
            square.transform.localScale = Vector3.zero;
            _winSquares.Add(square.GetComponent<Image>());
        }
        
        private void AddBaseSquareToBaseLayoutGroup()
        {
            Instantiate(_winSquare, _baseHorizontalLayoutGroup.transform);
        }
    }
}

