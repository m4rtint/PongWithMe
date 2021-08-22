using TMPro;
using UnityEngine;

namespace PongWithMe
{
    public class LivesViewBehaviour : MonoBehaviour
    {
        [SerializeField] private TMP_Text _topLivesPosition = null;
        [SerializeField] private TMP_Text _rightLivesPosition = null;
        [SerializeField] private TMP_Text _downLivesPosition = null;
        [SerializeField] private TMP_Text _leftLivesPosition = null;
        
        
    }

    public class LivesViewModel
    {
        private PlayerLives _playersLives = null;

        public LivesViewModel(PlayerLives lives)
        {
            _playersLives = lives;
            _playersLives.OnBrickBreak += HandleOnBrickBreak;
        }

        private void HandleOnBrickBreak(int playerOwner, int numberOfLives)
        {
            
        }
    }
}

