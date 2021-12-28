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

        public override string Version => "v0.1.1";

        public override string Author => "tommojphillips";

        private const string FILE_NAME = "SecureWarningTriangle.txt";

        private GameObject warningTriangleGo;
        private Part warningTrianglePart;
        private PartSaveInfo warningTriangleLoadedSaveInfo;
        private bool warningTriangleInitialized = false;

        public override void OnLoad()
        {
            warningTriangleLoadedSaveInfo = loadData();
            warningTriangleGo = GameObject.Find("warning triangle(Clone)");
            initWarningTriangle();
            ModConsole.Print("SecureWarningTriangle: Loaded");
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
        private void initWarningTriangle()
        {
            GameObject satsuma = GameObject.Find("SATSUMA(557kg, 248)");
            GameObject backPanel = GameObject.Find("back panel(Clone)");
            Trigger triggerTrunk = new Trigger("WarningTriangleTrunkTrigger", satsuma, new Vector3(0.4419999f, 0.132f, -1.582f), new Vector3(270f, 50.89845f, 0));
            Trigger triggerBackPanel = new Trigger("WarningTriangleBackPanelTrigger", backPanel, new Vector3(-0.4130001f, 0.09600005f, 0.162f), new Vector3(355, 0f, 0f));
            warningTrianglePart = warningTriangleGo.AddComponent<Part>();
            warningTrianglePart.defaultSaveInfo = new PartSaveInfo() { installed = false };
            AssemblyTypeJointSettings atjs = new AssemblyTypeJointSettings(satsuma.GetComponent<Rigidbody>(), backPanel.GetComponent<Rigidbody>());
            PartSettings ps = new PartSettings() { assembleType = AssembleType.joint, assemblyTypeJointSettings = atjs, setPositionRotationOnInitialisePart = false, installedPartToLayer = LayerMasksEnum.DontCollide };
            warningTrianglePart.initPart(warningTriangleLoadedSaveInfo, ps, triggerTrunk, triggerBackPanel);
            ModConsole.Print("SecureWarningTriangle: Initialized");
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
