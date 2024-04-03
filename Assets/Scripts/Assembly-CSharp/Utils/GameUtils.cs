using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Assembly_CSharp.Utils
{
    public static class GameUtils
    {
        [PunRPC]
        public static T InstantiateAsGameObject<T>(UnityEngine.Object obj) where T : class
        {
            if (IsMultiplayer())
            {
                Debug.Log("Instantiate using PhotonView");
                return PhotonView.Instantiate(obj) as T;
            }

            return UnityEngine.Object.Instantiate(obj) as T;
        }

        [PunRPC]
        public static T InstantiateAsGameObject<T>(UnityEngine.Object obj, Vector3 vector3, Quaternion quaternion) where T : class
        {
            if (IsMultiplayer())
                return PhotonView.Instantiate(obj, vector3, quaternion) as T;

            return UnityEngine.Object.Instantiate(obj, vector3, quaternion) as T;
        }

        public static bool IsMultiplayer()
        {
            return !PhotonNetwork.offlineMode && PhotonNetwork.connected;
        }
    }
}
