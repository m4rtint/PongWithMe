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
                var direction = i == 0 ? Direction.Right : Direction.Up;
                var player = new PlayerPaddle(input, i, direction);
                _playerPaddles[i].Initialize(player);
            }
        }
    }
}

