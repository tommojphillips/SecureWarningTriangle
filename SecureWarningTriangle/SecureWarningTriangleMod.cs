using HutongGames.PlayMaker;
using MSCLoader;
using System;
using TommoJProductions.ModApi;
using TommoJProductions.ModApi.Attachable;
using UnityEngine;
using static TommoJProductions.ModApi.Attachable.Part;

namespace SecureWarningTriangle
{
    public class SecureWarningTriangleMod : Mod
    {
        public override string ID => "SecureWarningTriangle";

        public override string Version => VersionInfo.version;

        public override string Author => "tommojphillips";

        private const string FILE_NAME = "SecureWarningTriangle.txt";

        public override string Description => $"Latest release date: {VersionInfo.lastestRelease}, Secure the warning triangle to the trunk of the satsuma or the back panel!";

        private GameObject warningTriangleGo;
        private Part warningTrianglePart;
        private PartSaveInfo warningTriangleLoadedSaveInfo;
        private bool initialized = false;
        private GameObject satsuma;
        private GameObject backPanel;
        private FsmBool warningTriangleInstalled;

        public override void OnLoad()
        {
            backPanel = GameObject.Find("back panel(Clone)");
            updateBackPanelCollision();
            warningTriangleLoadedSaveInfo = loadData();
            warningTriangleGo = GameObject.Find("warning triangle(Clone)");
            PlayMakerFSM databaseData = GameObject.Find("Database/DatabaseOrders/WarningTriangle").GetPlayMaker("Data");
            warningTriangleInstalled = databaseData.FsmVariables.FindFsmBool("Installed");
            databaseData.GetState("Save game").prependNewAction(fixTransform);
            if (warningTriangleInstalled.Value)
                warningTriangleGo.GetPlayMakerState("Remove part").appendNewAction(initWarningTriangle);
            else
                initWarningTriangle();
            ModConsole.Print($"SecureWarningTriangle v{Version}: Loaded");
        }

        public override void OnSave()
        {
            try
            {
                if (warningTrianglePart != null)
                    SaveLoad.SerializeSaveFile(this, warningTrianglePart.getSaveInfo(), FILE_NAME);
            }
            catch (Exception ex)
            {
                ModConsole.Error("<b>[SecureWarningTriangle]</b> - an error occured while attempting to save part info.. see: " + ex.ToString());
            }
        }

        private void fixTransform() 
        {
            warningTriangleGo.transform.localScale = Vector3.one;
        }
        private void initWarningTriangle()
        {
            if (!initialized)
            {
                initialized = true;
                satsuma = GameObject.Find("SATSUMA(557kg, 248)");
                Trigger triggerTrunk = new Trigger("WarningTriangleTrunkTrigger", satsuma, new Vector3(0.4419999f, 0.132f, -1.582f), new Vector3(270f, 50.89845f, 0));
                Trigger triggerBackPanel = new Trigger("WarningTriangleBackPanelTrigger", backPanel, new Vector3(-0.4130001f, 0.09600005f, 0.162f), new Vector3(355, 0f, 0f));
                warningTrianglePart = warningTriangleGo.AddComponent<Part>();
                warningTrianglePart.defaultSaveInfo = new PartSaveInfo() { installed = false };
                PartSettings ps = new PartSettings() { setPositionRotationOnInitialisePart = false };
                warningTrianglePart.initPart(warningTriangleLoadedSaveInfo, ps, triggerTrunk, triggerBackPanel);
                GameObject.Find("KEKMET(350-400psi)/trigger_warning triangle").SetActive(false);
                backPanel.GetPlayMakerState("Remove part").appendNewAction(updateBackPanelCollision);
                ModConsole.Print("SecureWarningTriangle: Initialized Part");
            }
        }
        private void updateBackPanelCollision() 
        {
            Rigidbody r = backPanel.GetComponent<Rigidbody>();
            if (r)
                r.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;       
        }

        private PartSaveInfo loadData() 
        {
            try
            {
                return SaveLoad.DeserializeSaveFile<PartSaveInfo>(this, FILE_NAME);
            }
            catch (NullReferenceException)
            {
                // no save file exists.. // setting/loading default save data.

                return null;
            }

        }
    }
}
