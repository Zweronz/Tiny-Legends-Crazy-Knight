using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Assembly_CSharp
{
    public class PUNPlayerNetwork : Photon.PunBehaviour
    {
        public GameObject playerPrefab;

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadedSceneMode)
        {
        }

        private void Start()
        {

        }
    }
}
