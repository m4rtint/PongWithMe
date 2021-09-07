using System;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PongWithMe
{
    public class NetworkWorldManager : MonoBehaviour
    {
        private const int AMOUNT_OF_PLAYERS = 4;
        private const int AMOUNT_OF_WINS = 3;
        
        [Title("Components")] 
        [SerializeField] private BricksBehaviour _bricksBehaviour = null;
        [SerializeField] private BallBehaviour _ballBehaviour = null;
        [SerializeField] private NetworkPlayersManager _playersManager = null;
        [SerializeField] private NetworkPlayerJoinManager _playerJoinManager = null;
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
            CustomType.Register();
        }

        private void Start()
        {
            // Board
            _board = new Board();
            _bricksBehaviour.Initialize(_board);
            
            _playerLives = new PlayerLives(_board, AMOUNT_OF_PLAYERS);
            _goalsManager.Initialize(_playerLives);
            _playersManager.Initialize(_playerLives);
            _playerJoinManager.Initialize(_playersManager);
            
            // Ball
            _ballBehaviour.Initialize();
            
            // Mutator
            _splattersBehaviour.Initialize();
            _portalsBehaviour.Initialize();
            
            // Interface
            _gameOverView.Initialize(_playersManager);
            _scorePanelView.Initialize(_playersManager, AMOUNT_OF_WINS);
        }

        private void CleanUp()
        {
            
        }

        private void Reset()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var bricks = BoardFactory.Build(AMOUNT_OF_PLAYERS);
                _board.Reset(bricks);
                _bricksBehaviour.Reset();
                List<BrickDTO> list = new List<BrickDTO>();
                
                foreach (var brick in bricks)
                {
                    var brickDTO = new BrickDTO();
                    brickDTO.PlayerOwned = brick.PlayerOwned;
                    brickDTO.PositionX = brick.Position.x;
                    brickDTO.PositionY = brick.Position.y;
                    list.Add(brickDTO);
                }
                PhotonView.Get(this).RPC("BuildBricks", RpcTarget.Others, list.ToArray());
            }
        }

        [PunRPC]
        private void BuildBricks(BrickDTO[] stream)
        {
           Debug.Log(stream);
           List<Brick> list = new List<Brick>();
           foreach (var brickDTO in stream)
           {
               var brick = new Brick();
               brick.Position = new Vector3(brickDTO.PositionX, brickDTO.PositionY);
               brick.PlayerOwned = brickDTO.PlayerOwned;
               brick.BrickColor = ColorPalette.PlayerColor(brickDTO.PlayerOwned);
               list.Add(brick);
           }

           var bricks = list.ToArray();
           _board.Reset(bricks);
           _bricksBehaviour.Reset();
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