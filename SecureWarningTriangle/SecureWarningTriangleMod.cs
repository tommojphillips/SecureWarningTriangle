using MSCLoader;
using System;
using System.Collections.Generic;
using TommoJProductions.ModApi.Attachable;
using UnityEngine;
using static TommoJProductions.ModApi.Attachable.Part;

namespace SecureWarningTriangle
{
    public class SecureWarningTriangleMod : Mod
    {
        public override string ID => "SecureWarningTriangle";

        public override string Version => "v0.1";

        public override string Author => "tommojphillips";

        private const string FILE_NAME = "SecureWarningTriangle.txt";

        private Part warningTrianglePart;
        private PartSaveInfo warningTriangleLoadedSaveInfo;

        public override void OnLoad()
        {
            warningTriangleLoadedSaveInfo = loadData();
            GameObject satsuma = GameObject.Find("SATSUMA(557kg, 248)");
            GameObject warningTri = GameObject.Find("warning triangle(Clone)");
            Trigger trigger = new Trigger("WarningTriangleTrigger", satsuma, new Vector3(0, -0.053f, -1.45f));
            warningTrianglePart = warningTri.AddComponent<Part>();
            AssemblyTypeJointSettings atjs = new AssemblyTypeJointSettings(satsuma.GetComponent<Rigidbody>()) { breakForce = 250 };
            PartSettings ps = new PartSettings() { assembleType = AssembleType.joint, assemblyTypeJointSettings = atjs, setPositionRotationOnInitialisePart = false };
            warningTrianglePart.initPart(warningTriangleLoadedSaveInfo, ps, trigger);
            ModConsole.Print("SecureWarningTriangle: Loaded");
        }

        public override void OnSave()
        {
            try
            {
                SaveLoad.SerializeSaveFile(this, warningTrianglePart.getSaveInfo(), FILE_NAME);
            }
            catch (Exception ex)
            {
                ModConsole.Error("<b>[SecureWarningTriangle]</b> - an error occured while attempting to save part info.. see: " + ex.ToString());
            }
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
