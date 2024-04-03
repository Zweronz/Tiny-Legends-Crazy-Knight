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

        private void Start()
        {
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
            //if (PhotonNetwork.isMasterClient)
            //{
            //    Crazy_GlobalData.next_scene = "CrazyScene00";
            //    PhotonNetwork.LoadLevel("CrazyScene00");
            //}
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            if (PhotonNetwork.playerList.Count() == 2 && PhotonNetwork.isMasterClient)
            {
                Crazy_GlobalData.next_scene = "CrazyScene00";
                PhotonNetwork.LoadLevel("CrazyScene00");
            }
        }

        public override void OnDisconnectedFromPhoton()
        {
            Debug.LogWarning("OnDisconnectedFromPhoton()");
        }

    }
}
