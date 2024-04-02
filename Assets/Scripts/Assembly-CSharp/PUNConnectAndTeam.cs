using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TNetSdk;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Assembly_CSharp
{
    public class PUNConnectAndTeam: Photon.PunBehaviour
    {
        public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
        string _gameVersion = "1.0.0";
        public byte MaxPlayersPerRoom = 4;
        public GameObject playerPrefab;

        private void Start()
        {
            
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if (Crazy_PlayerControl_Net.player == null)
                {
                    Debug.Log("We are Instantiating LocalPlayer from " + Application.loadedLevelName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 0.1f, 6f), Quaternion.identity, 0);
                }
                else
                {
                    Debug.Log("Ignoring scene load for " + Application.loadedLevelName);
                }
            }

            Connect();
        }

        void Awake()
        {
            PhotonNetwork.logLevel = Loglevel;
            //PhotonNetwork.autoJoinLobby = false;
            PhotonNetwork.automaticallySyncScene = true;
            //PhotonNetwork.playerName = "WALLE";
        }

        public void Connect()
        {
            PhotonNetwork.ConnectUsingSettings(_gameVersion);
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }

        public override void OnJoinedLobby()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            Debug.Log("No random room available, so we create one.");
            PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = MaxPlayersPerRoom }, TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.isMasterClient)
            {
                Crazy_GlobalData.next_scene = "CrazyScene00";
                PhotonNetwork.LoadLevel("CrazyScene00");
            }
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            //if (PhotonNetwork.isMasterClient)
            //{
            //    Crazy_GlobalData.next_scene = "CrazyScene00";
            //    PhotonNetwork.LoadLevel("CrazyScene00");
            //}
        }

        public override void OnDisconnectedFromPhoton()
        {
            Debug.LogWarning("OnDisconnectedFromPhoton()");
        }

    }
}
