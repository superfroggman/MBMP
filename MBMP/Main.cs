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

        private Rect _window = new Rect(200, 200, 500, 500);

        private bool testbool = false;

        void Awake()
        {
            instance = this;
            Logger.LogInfo("Plugin loaded");
        }

        void FixedUpdate()
        {
            if (SceneManager.GetActiveScene().name == "Master")
            {
                if(!testbool){
                    testbool = true;
                    //GameObject brother = GameObject.Find("MrBonjour");
                    //Instantiate(brother, Gameplay.i.Player.transform.position, Gameplay.i.Player.transform.rotation);
                    spawnPlayer();
                }
            }
        }

        private void spawnPlayer(){
            GameObject caplsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            
            caplsule.transform.position = Gameplay.i.Player.transform.position;
            caplsule.transform.localScale = new Vector3(1, 1.5f, 1);
            caplsule.name = "testplayer";
            caplsule.layer = 10;

            GameObject usernameText = new GameObject("Text");

            usernameText.AddComponent<TextMesh>();
            usernameText.GetComponent<TextMesh>().text = "testplayer";
            usernameText.GetComponent<TextMesh>().alignment = TextAlignment.Center;
            usernameText.GetComponent<TextMesh>().fontSize = 32;
            usernameText.GetComponent<TextMesh>().characterSize = 0.1f;
            usernameText.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
            usernameText.transform.parent = caplsule.transform;
            usernameText.transform.localPosition = new Vector3(0, 1.5f, 0);
            usernameText.transform.localRotation = Quaternion.Euler(0, 180, 0);

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
            // Player position and rotation
            GUILayout.TextField("P1" + Gameplay.i.Player.transform.position.ToString() + Gameplay.i.Player.transform.rotation.ToString());


            // All vehicles position and rotation
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach(GameObject test in allObjects){
                if (test.GetComponent<NWH.VehiclePhysics2.VehicleController>()){
                    NWH.VehiclePhysics2.VehicleController vehicleController = test.GetComponent<NWH.VehiclePhysics2.VehicleController>();
                    // vehicleController.brakes.Disable();

                    GUILayout.TextField(test.name + vehicleController.vehicleTransform.position.ToString() + vehicleController.vehicleTransform.rotation.ToString());
                }
            }
        }
    }
}
