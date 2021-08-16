using UnityEngine;

namespace PongWithMe
{
    public class PlayersManager : MonoBehaviour
    {
        [SerializeField] private PaddleBehaviour[] _playerPaddles = null;

        private void Start()
        {
            for (int i = 0; i < _playerPaddles.Length; i++)
            {
                var input = new PongInput(i);
                var player = new PlayerPaddle(input, i, (Direction) i);
                _playerPaddles[i].Initialize(player);
            }
        }
    }
}

