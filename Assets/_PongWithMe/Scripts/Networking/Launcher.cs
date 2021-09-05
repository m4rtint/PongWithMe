using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace PongWithMe
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        private const byte MAX_PLAYERS_PER_ROOM = 4;
        private const string GAME_VERSION = "1.0.0";

        [SerializeField] private Button _playButton = null;
        [SerializeField] private UsernameViewBehaviour _userNameView = null;

        private bool _isConnecting = false;
        
        public override void OnConnectedToMaster()
        {
            if (_isConnecting)
            {            
                Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
                PhotonNetwork.JoinRandomRoom();
                _isConnecting = false;
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MAX_PLAYERS_PER_ROOM } );
        }
        
        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");


                // #Critical
                // Load the Room Level.
                PhotonNetwork.LoadLevel("Main");
            }
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            _userNameView.SetLabelAsUsername();
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        #region Mono
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            _userNameView.SetLabelAsUsername();
            _playButton.onClick.AddListener(ConnectToPhoton);
        }

        private void ConnectToPhoton()
        {
            PhotonNetwork.NickName = _userNameView.UserName;
            _userNameView.SetLabelAsLoading();
            
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                _isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = GAME_VERSION;
            }
        }
        #endregion
    }
}