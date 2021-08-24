using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PongWithMe
{
    public class WorldManager : MonoBehaviour
    {
        private const int AMOUNT_OF_PLAYERS = 4;
        
        [SerializeField] private BricksBehaviour _bricksBehaviour = null;
        [SerializeField] private BallBehaviour _ballBehaviour = null;
        [SerializeField] private PlayersManager _playersManager = null;
        [SerializeField] private GoalsManager _goalsManager = null;
        
        [Title("User Interface")]
        [SerializeField] private LivesViewBehaviour _livesView = null;
        
        private Board _board = null;
        private PlayerLives _playerLives = null;
        private IStateManager _stateManager = null;

        private void Awake()
        {
            _stateManager = StateManager.Instance;
        }

        private void Start()
        {
            new ColorPalette();
            _stateManager.OnStateChanged += HandleStateChanges;
            
            //Board
            var boardGenerator = new BoardGenerator(AMOUNT_OF_PLAYERS);
            _playerLives = new PlayerLives(boardGenerator.Bricks);
            _board = new Board(boardGenerator.Bricks);
            _bricksBehaviour.Initialize(_board);
            
            // Ball
            _ballBehaviour.Initialize();
            
            // Players
            _playersManager.Initialize(_ballBehaviour, _playerLives);

            // Goals
            _goalsManager.Initialize(_playerLives);
            SetupGoals();
            
            // Interface
            _livesView.Initialize(_playerLives, _playersManager.Players);
        }

        private void OnDestroy()
        {
            _stateManager.OnStateChanged -= HandleStateChanges;
        }

        private void SetupGoals()
        {
            foreach (var player in _playersManager.Players)
            {
                _goalsManager.Set(player);
            }
        }

        [Button]
        public void ChangeState(bool play)
        {
            HandleStateChanges(play ? State.Play : State.Animating);
        }
        
        private void HandleStateChanges(State state)
        {
            switch (state)
            {
                case State.Play:
                    DOTween.To(()=> Time.timeScale, x=> Time.timeScale = x, 1, 2.0f).SetEase(Ease.InQuad).SetUpdate(true);
                    break;
                case State.Animating:
                    DOTween.To(()=> Time.timeScale, x=> Time.timeScale = x, 0, 2.0f).SetEase(Ease.OutExpo).SetUpdate(true);
                    break;
                case State.GameOver:
                    break;
                default:
                    PanicHelper.Panic(new Exception("Should not have hit this game state"));
                    break;
            }   
        }
    } 
}

