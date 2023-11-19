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

        List<Effect> effects = new List<Effect>();
        List<Effect> activeEffects = new List<Effect>();

        void Awake()
        {
            instance = this;
            Logger.LogInfo("Plugin loaded");


            effects.Add(new Effect(600, "Observed much?", CreepyCars));
            effects.Add(new Effect(0, "Start All Cars", StartAllCars));
            effects.Add(new Effect(600, "FOV based on speed", SpeedFOV));
            effects.Add(new Effect(0, "No brakes", DisableBrakes));
            effects.Add(new Effect(0, "Teleport cars", TeleportCars));
        }

        void FixedUpdate()
        {
            if (SceneManager.GetActiveScene().name == "Master")
            {
                RunActiveEffects();
            }

            // Log stuff
            Logger.LogInfo(Gameplay.i.PlayerWalking.transform.position);
        }

        private void RunActiveEffects()
        {
            foreach (Effect effect in activeEffects)
            {
                effect.method();
                effect.timeLeft -= 1;
                if (effect.timeLeft <= 0)
                {
                    activeEffects.Remove(effect);
                }
            }
        }

        private void DoTheStuff()
        {
            //Gameplay.i.ExitVehicle();
            //Logger.LogInfo($"Position:{Gameplay.i.PlayerWalking.transform.position}");
            //Gameplay.i.PlayerWalking.transform.Translate(0,1,0);


            // teleporting everything purchasable to dumpster with Gameplay.i.TeleportItemInDumpster
            /*
            foreach(GameObject test in allObjects){
                //Logger.LogInfo($"Object:{test}, {test.tag}, {test.ToString()}");
                if (test.GetComponent<ItemForSale_Display>()){
                    //Logger.LogInfo(test);
                    
                    //Gameplay.i.TeleportItemInDumpster(test);
                }
                
            }
            */
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
            foreach (Effect effect in effects)
            {
                if (GUILayout.Button(effect.name))
                {
                    activeEffects.Add((Effect)effect.Clone());
                }
            }
        }




        private void CreepyCars()
        {
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

            foreach (GameObject test in allObjects)
            {
                if (test.GetComponent<NWH.VehiclePhysics2.VehicleController>())
                {
                    NWH.VehiclePhysics2.VehicleController vehicleController = test.GetComponent<NWH.VehiclePhysics2.VehicleController>();
                    vehicleController.vehicleTransform.LookAt(Gameplay.i.PlayerWalking.transform.position);
                }
            }
        }

        private void TeleportCars()
        {
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

            foreach (GameObject test in allObjects)
            {
                if (test.GetComponent<NWH.VehiclePhysics2.VehicleController>())
                {
                    NWH.VehiclePhysics2.VehicleController vehicleController = test.GetComponent<NWH.VehiclePhysics2.VehicleController>();
                    vehicleController.vehicleTransform.position = Gameplay.i.PlayerWalking.transform.position;
                }
            }
        }

        private void StartAllCars()
        {
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

            foreach (GameObject test in allObjects)
            {
                if (test.GetComponent<NWH.VehiclePhysics2.VehicleController>())
                {
                    NWH.VehiclePhysics2.VehicleController vehicleController = test.GetComponent<NWH.VehiclePhysics2.VehicleController>();
                    vehicleController.StartEngine();
                }
            }
        }


        private void DisableBrakes()
        {
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

            foreach (GameObject test in allObjects)
            {
                if (test.GetComponent<NWH.VehiclePhysics2.VehicleController>())
                {
                    NWH.VehiclePhysics2.VehicleController vehicleController = test.GetComponent<NWH.VehiclePhysics2.VehicleController>();
                    vehicleController.brakes.Disable();
                }
            }
        }



        private Vector3 oldPosition;
        private void SpeedFOV()
        {
            float dist = 0;
            if (oldPosition != null)
            {
                dist = Vector3.Distance(oldPosition, Gameplay.i.PlayerWalking.transform.position);
            }
            oldPosition = Gameplay.i.PlayerWalking.transform.position;

            int fov = (int)(dist * 150);
            fov += 20;
            Gameplay.i.ChangeFieldOfView(fov);

        }
    }

    public class Effect
    {
        public int timeLeft;
        public string name;
        public Action method;

        public Effect(int _timeLeft, string _name, Action _method)
        {
            timeLeft = _timeLeft;
            name = _name;
            method = _method;
        }

        public object Clone()
        {
            return new Effect(this.timeLeft, this.name, this.method);
        }
    }
}
