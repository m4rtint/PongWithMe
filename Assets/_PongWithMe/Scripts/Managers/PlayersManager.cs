using UnityEngine;

namespace PongWithMe
{
    public class PlayersManager : MonoBehaviour
    {
        [SerializeField] private PaddleBehaviour[] _playerPaddles = null;
        // THIS SHOULD BE INJECTED IN
        [SerializeField] private BallBehaviour _ballBehaviour = null;
        public void Initialize(BallBehaviour ball)
        {
            
        }
        
        private void Start()
        {
            var input = new PongInput(0);                
            var player = new PlayerPaddle(input, 0, Direction.Right);
            _playerPaddles[0].Initialize(player);
            
            var ai = new AIInput();
            var aiPaddle = new AIPaddle(ai, 1, Direction.Left, _ballBehaviour.gameObject);
            _playerPaddles[1].Initialize(aiPaddle);
        }
    }
}

