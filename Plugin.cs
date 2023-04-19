using BepInEx;
using Riptide;
using Riptide.Utils;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyFirstPlugin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private string ip = "127.0.0.1";
        private string port = "7777";
        private string username = Environment.MachineName;
        private Rect _window = new Rect(200, 200, 240, 240);

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            
        }
        private void Update(){

        }

        private void OnGUI()
        {
            if(SceneManager.GetActiveScene().name == "Master")
            {
                _window = GUILayout.Window(46489, _window, menu, "Multiplayer");   
            }
        }

        private void menu(int id)
        {
            ip = GUILayout.TextField(ip);
            port = GUILayout.TextField(port);
            username = GUILayout.TextField(username);


            if (GUILayout.Button("Connect"))
            {
                /*
                if (ClientNetworkManager.Singleton.Connect())
                {
                    Debug.Log("[CLIENT] Connected (custom log)");
                }
                else
                {
                    Debug.Log("[CLIENT] Error while Connecting (you're already connected or actual error)");
                }
                */
            }

            if (GUILayout.Button("Create Server")) {
                //ServerNetworkManager.Singleton.StartServer();
            }

        }


    }
}
