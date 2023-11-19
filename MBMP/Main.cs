using BepInEx;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace MBMP
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Main : BaseUnityPlugin
    {
        public static Main instance;

        private Rect _window = new Rect(200, 200, 240, 240);


        void Awake()
        {
            instance = this;
            Logger.LogInfo("Plugin loaded");
        }

        void FixedUpdate()
        {
            if (SceneManager.GetActiveScene().name == "Master")
            {
                // Log stuff
                Logger.LogInfo(Gameplay.i.PlayerWalking.transform.position);

            }
        }

        private void OnGUI()
        {
            if (SceneManager.GetActiveScene().name == "Master")
            {
                _window = GUILayout.Window(46489, _window, menu, "Multiplayer menu");
            }
        }

        private void menu(int id)
        {
            //foreach (Effect effect in effects)
            //{
            //    if (GUILayout.Button(effect.name))
            //    {
            //        activeEffects.Add((Effect)effect.Clone());
            //    }
            //}
            GUILayout.TextField(Gameplay.i.PlayerWalking.transform.position.ToString());
        }
    }
}
