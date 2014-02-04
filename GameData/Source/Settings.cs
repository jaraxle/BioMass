using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;
using UnityEngine;
using Tac;

namespace KSPBioMass
{
    public class Settings
    {
        //Static
        public static String AddonName = "BioMass+Science";
        public static String PathKSP = KSPUtil.ApplicationRootPath.Replace("\\", "/");
        public static String PathPlugin = string.Format("{0}GameData/{1}", PathKSP, AddonName);
        public static String PathPluginData = string.Format("{0}/PluginData", PathPlugin);
        public static String PathTextures = string.Format("{0}/Textures", PathPlugin);
        public static String GlobalConfigFile = string.Format("{0}/BioMass.cfg", PathPluginData);
        
        public ConfigNode globalSettingsNode = new ConfigNode();
        public List<Component> controller = new List<Component>();


        private const string configNodeName = "GlobalSettings";
        private const int SECONDS_PER_MINUTE = 60;
        private const int SECONDS_PER_HOUR = 60 * SECONDS_PER_MINUTE;
        private const int SECONDS_PER_DAY = 24 * SECONDS_PER_HOUR;

        public int MaxDeltaTime { get; set; }
        public int ElectricityMaxDeltaTime { get; set; }

        public string Food { get; private set; }
        public string Water { get; private set; }
        public string Oxygen { get; private set; }
        public string Electricity { get { return "ElectricCharge"; } }
        public string CO2 { get; private set; }
        public string Waste { get; private set; }
        public string WasteWater { get; private set; }

        public string Seeds { get; private set; }
        public string BioMass { get; private set; }
        public string Light { get; private set; }
        public string BioCake { get; private set; }
        public string CompressedCO2 { get; private set; }
        public string Hydrogen { get; private set; }

        public int FoodId
        {
            get
            {
                return PartResourceLibrary.Instance.GetDefinition(Food).id;
            }
        }
        public int WaterId
        {
            get
            {
                return PartResourceLibrary.Instance.GetDefinition(Water).id;
            }
        }
        public int OxygenId
        {
            get
            {
                return PartResourceLibrary.Instance.GetDefinition(Oxygen).id;
            }
        }
        public int ElectricityId
        {
            get
            {
                return PartResourceLibrary.Instance.GetDefinition(Electricity).id;
            }
        }
        public int CO2Id
        {
            get
            {
                return PartResourceLibrary.Instance.GetDefinition(CO2).id;
            }
        }
        public int WasteId
        {
            get
            {
                return PartResourceLibrary.Instance.GetDefinition(Waste).id;
            }
        }
        public int WasteWaterId
        {
            get
            {
                return PartResourceLibrary.Instance.GetDefinition(WasteWater).id;
            }
        }

        public int SeedsId
        {
            get
            {
                return PartResourceLibrary.Instance.GetDefinition(Seeds).id;
            }
        }
        public int BioMassId
        {
            get
            {
                return PartResourceLibrary.Instance.GetDefinition(BioMass).id;
            }
        }
        public int LightId
        {
            get
            {
                return PartResourceLibrary.Instance.GetDefinition(Light).id;
            }
        }
        public int BioCakeId
        {
            get
            {
                return PartResourceLibrary.Instance.GetDefinition(BioCake).id;
            }
        }
        public int CompressedCO2Id
        {
            get
            {
                return PartResourceLibrary.Instance.GetDefinition(CompressedCO2).id;
            }
        }
        public int HydrogenId
        {
            get
            {
                return PartResourceLibrary.Instance.GetDefinition(Hydrogen).id;
            }
        }

        public Settings()
        {
            this.Log_DebugOnly("Constructor Settings");
            this.Log_DebugOnly("ConfigFile: " + GlobalConfigFile);

            this.globalSettingsNode = new ConfigNode();

            MaxDeltaTime = SECONDS_PER_DAY; // max 1 day (24 hour) per physics update, or 50 days (4,320,000 seconds) per second
            ElectricityMaxDeltaTime = 1; // max 1 second per physics update

            Food = "Food";
            Water = "Water";
            Oxygen = "Oxygen";
            CO2 = "CarbonDioxide";
            Waste = "Waste";
            WasteWater = "WasteWater";
            Seeds = "Seeds";
            BioMass = "BioMass";
            Light = "Light";
            BioCake = "BioCake";
            CompressedCO2 = "CompressedCO2";
            Hydrogen = "Hydrogen";
        }
        
        public Boolean FileExists(string FileName)
        {
            return (System.IO.File.Exists(FileName));
        }

        public Boolean Load()
        {
            return this.Load(GlobalConfigFile);
        }       

        public Boolean Load(String fileFullName)
        {
            Boolean blnReturn = false;
            try
            {
                if (FileExists(fileFullName))
                {
                    //Load the file into a config node
                    globalSettingsNode = ConfigNode.Load(fileFullName);
                    this.LoadFromNode(globalSettingsNode);
                    foreach (ISavable s in controller.Where(c => c is ISavable))
                    {
                        s.Load(globalSettingsNode);
                    }
                    this.Log_DebugOnly("Load GlobalSettings");
                    blnReturn = true;
                }
                else
                {
                    this.Log_DebugOnly(String.Format("File could not be found to load({0})", fileFullName));
                    blnReturn = false;
                }
            }
            catch (Exception ex)
            {
                this.LogError(String.Format("Failed to Load Configfile({0})-Error:{1}", fileFullName, ex.Message));
                blnReturn = false;
            }
            return blnReturn;
        }

        private void LoadFromNode(ConfigNode node)
        {
            if (node.HasNode(configNodeName))
            {
                ConfigNode settingsNode = node.GetNode(configNodeName);

                MaxDeltaTime = Utilities.GetValue(settingsNode, "MaxDeltaTime", MaxDeltaTime);
                ElectricityMaxDeltaTime = Utilities.GetValue(settingsNode, "ElectricityMaxDeltaTime", ElectricityMaxDeltaTime);

                Food = Utilities.GetValue(settingsNode, "FoodResource", Food);
                Water = Utilities.GetValue(settingsNode, "WaterResource", Water);
                Oxygen = Utilities.GetValue(settingsNode, "OxygenResource", Oxygen);
                CO2 = Utilities.GetValue(settingsNode, "CarbonDioxideResource", CO2);
                Waste = Utilities.GetValue(settingsNode, "WasteResource", Waste);
                WasteWater = Utilities.GetValue(settingsNode, "WasteWaterResource", WasteWater);

                Seeds = Utilities.GetValue(settingsNode, "SeedsResource", Seeds);
                BioMass = Utilities.GetValue(settingsNode, "BioMassResource", BioMass);
                Light = Utilities.GetValue(settingsNode, "LightResource", Light);
                BioCake = Utilities.GetValue(settingsNode, "BioCakeResource", BioCake);
                CompressedCO2 = Utilities.GetValue(settingsNode, "CompressedCO2Resource", CompressedCO2);
                Hydrogen = Utilities.GetValue(settingsNode, "HydrogenResource", Hydrogen);
            }
        }

        public Boolean Save()
        {
            return this.Save(GlobalConfigFile);
        }

        public Boolean Save(String fileFullName)
        {
            Boolean blnReturn = false;
            try
            {
                //Encode the current object
                SaveToNode(globalSettingsNode);
                foreach (ISavable s in controller.Where(c => c is ISavable))
                {
                    s.Save(globalSettingsNode);
                }
                globalSettingsNode.Save(GlobalConfigFile);
                this.Log_DebugOnly("Save GlobalSettings");
                blnReturn = true;
            }
            catch (Exception ex)
            {
                this.LogError(String.Format("Failed to Save ConfigNode to file({0})-Error:{1}", fileFullName, ex.Message));
                blnReturn = false;
            }
            return blnReturn;
        }

        private void SaveToNode(ConfigNode node)
        {
            ConfigNode settingsNode;
            if (node.HasNode(configNodeName))
            {
                settingsNode = node.GetNode(configNodeName);
                settingsNode.ClearData();
            }
            else
            {
                settingsNode = node.AddNode(configNodeName);
            }

            settingsNode.AddValue("MaxDeltaTime", MaxDeltaTime);
            settingsNode.AddValue("ElectricityMaxDeltaTime", ElectricityMaxDeltaTime);

            settingsNode.AddValue("FoodResource", Food);
            settingsNode.AddValue("WaterResource", Water);
            settingsNode.AddValue("OxygenResource", Oxygen);
            settingsNode.AddValue("CarbonDioxideResource", CO2);
            settingsNode.AddValue("WasteResource", Waste);
            settingsNode.AddValue("WasteWaterResource", WasteWater);

            settingsNode.AddValue("SeedsResource", Seeds);
            settingsNode.AddValue("BioMassResource", BioMass);
            settingsNode.AddValue("LightResource", Light);
            settingsNode.AddValue("BioCakeResource", BioCake);
            settingsNode.AddValue("CompressedCO2Resource", CompressedCO2);
            settingsNode.AddValue("HydrogenResource", Hydrogen);
        }
    }
}
