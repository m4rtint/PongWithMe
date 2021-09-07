using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace PongWithMe
{
    public interface INetworkCore
    {
        void Connect();
        void SetUserName(string username);
    }
    
    public class NetworkLauncher : MonoBehaviourPunCallbacks, INetworkCore
    {
        private const byte MAX_PLAYERS_PER_ROOM = 4;
        private const string GAME_VERSION = "1.0.0";
        
        private bool _isConnecting = false;
        
        #region CallBack
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
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }
        #endregion

        #region Mono
        public void Initialize()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public void SetUserName(string username)
        {
            PhotonNetwork.NickName = username;
        }

        public void Connect()
        {
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