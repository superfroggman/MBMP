using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace MBMP
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Main : BaseUnityPlugin
    {
        public enum State { Host, Client, NotConnected };

        public static Main instance;
        private RiptideServer _server;
        private State _state = State.NotConnected;
        private Task _serverTask;

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
            GUILayout.TextField($"Current state: {_state}");
            if (GUILayout.Button("Host game") && _state != State.Host)
            {
                Logger.LogInfo("Starting server");
                _state = State.Host;
                _server = new RiptideServer("localhost", 8080, (fromClientId, message) =>
                {
                    Logger.LogInfo($"Received message from client {fromClientId}: {message.Message}");
                });



            }

            if (_state == State.Host)
            {
                GUILayout.TextField($"Server is listening on {_server.ListeningAddress}");
                if (_server.IsListening)
                {
                    GUILayout.TextField("Server is listening");
                }
                else
                {
                    GUILayout.TextField("Server is not listening");
                }
            }
        }

        void OnDestroy()
        {
            if (_server != null)
            {
                _server.Stop();
                if (_serverTask != null && _serverTask.Status == TaskStatus.Running)
                {
                    _serverTask.Wait(); // Ensure the server task is properly stopped
                }
            }
        }
    }
}
