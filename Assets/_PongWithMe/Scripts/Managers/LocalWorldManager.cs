using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PongWithMe
{
    public class LocalWorldManager : MonoBehaviour
    {
        private const int AMOUNT_OF_PLAYERS = 4;
        private const int AMOUNT_OF_WINS = 3;

        [Title("Components")] 
        [SerializeField] private BricksBehaviour _bricksBehaviour = null;
        [SerializeField] private BallBehaviour _ballBehaviour = null;
        [SerializeField] private PlayersJoinManager _playersJoinManager = null;
        [SerializeField] private PlayersManager _playersManager = null;
        [SerializeField] private GoalsManager _goalsManager = null;
        [SerializeField] private MutatorBehaviour _mutatorBehaviour = null;

        [Title("Mutators")] 
        [SerializeField] private SplattersBehaviour _splattersBehaviour = null;
        [SerializeField] private PortalsBehaviour _portalsBehaviour = null;

        [Title("User Interface")] 
        [SerializeField] private LivesViewBehaviour _livesView = null;
        [SerializeField] private GameOverViewBehaviour _gameOverView = null;
        [SerializeField] private ScorePanelViewBehaviour _scorePanelView = null;
        [SerializeField] private MutatorAnnouncementViewBehaviour _mutatorAnnouncementView = null;
        [SerializeField] private StartGameButtonBehaviour _startGameButton = null;
        
        private IStateManager _stateManager = null;

        private PlayerLives _playerLives = null;
        private Board _board = null;

        private void Awake()
        {
            DOTween.SetTweensCapacity(200, 125);
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            new ColorPalette();
            _stateManager = StateManager.Instance;
            _stateManager.OnStateChanged += HandleStateChanges;
            
            _stateManager.SetState(State.PlayerJoining);
        }

        private void Start()
        {
            // Board
            var bricks = BoardFactory.Build(AMOUNT_OF_PLAYERS);
            _board = new Board(bricks);
            _bricksBehaviour.Initialize(_board);

            // Players
            _playerLives = new PlayerLives(_board, AMOUNT_OF_PLAYERS);
            _goalsManager.Initialize(_playerLives);
            _playersManager.Initialize(_playerLives);
            _playersJoinManager.Initialize(_playersManager, _goalsManager, _ballBehaviour, _scorePanelView);

            // Ball
            _ballBehaviour.Initialize();
            
            // Mutator
            _splattersBehaviour.Initialize();
            _portalsBehaviour.Initialize();
            
            // Interface
            _gameOverView.Initialize(_playersManager);
            _scorePanelView.Initialize(_playersManager, AMOUNT_OF_WINS);
            _startGameButton.Initialize(CompletePlayerSetup);
        }

        private void CompletePlayerSetup()
        {
            _playersJoinManager.CompletePlayerJoiningSession();
            
            _livesView.Initialize(_playerLives, _playersManager.Players);
            var mutatorManager = new MutatorManager(
                _playersManager.Players, 
                _playersManager, 
                _goalsManager,
                _playerLives,
                _ballBehaviour,
                _board, 
                _splattersBehaviour.Splatters,
                _portalsBehaviour.Portals);
            _mutatorBehaviour.Initialize(mutatorManager);
            _mutatorAnnouncementView.Initialize(mutatorManager);

            _startGameButton.HideButton();
            _playerLives.ForceUpdatePlayerScores();
            _stateManager.SetState(State.PreGame);
        }

        private void CleanUp()
        {
            _board.CleanUp();
            _playerLives.CleanUp();
            _bricksBehaviour.CleanUp();
            _ballBehaviour.CleanUp();
            _playersManager.CleanUp();
            
            _splattersBehaviour.CleanUp();
            _portalsBehaviour.CleanUp();
            _mutatorBehaviour.CleanUp();
            _mutatorAnnouncementView.CleanUp();
        }

        private void Reset()
        {
            var bricks = BoardFactory.Build(AMOUNT_OF_PLAYERS);
            _board.Reset(bricks);
            _playerLives.Reset();
            _bricksBehaviour.Reset();
            _ballBehaviour.Reset();
            _playersManager.Reset();
            
            _goalsManager.Reset(_playersManager.Players);

            _splattersBehaviour.Reset();
            _portalsBehaviour.Reset();
            _mutatorBehaviour.Reset();
        }

        private void HandleStateChanges(State state)
        {
            switch (state)
            {
                case State.PlayerJoining:
                    break;
                case State.PreGame:
                    TimeScaleController.PlayTimeScale();
                    CleanUp();
                    Reset();
                    break;
                case State.StartGame:
                    _stateManager.SetState(State.Play);
                    break;
                case State.Play:
                    TimeScaleController.PlayTimeScale();
                    break;
                case State.Animating:
                    break;
                case State.EndRound:
                    TimeScaleController.EndGameTimeScale();
                    break;
                case State.ShowScore:
                case State.GameOver:
                    break;
                default:
                    PanicHelper.Panic(new Exception("Should not have hit this game state"));
                    break;
            }   
        }
    }
}