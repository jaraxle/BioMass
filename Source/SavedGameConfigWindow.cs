/**
 * Thunder Aerospace Corporation's Life Support for Kerbal Space Program.
 * Written by Taranis Elsu.
 * 
 * (C) Copyright 2013, Taranis Elsu
 * 
 * Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This
 * project is in no way associated with nor endorsed by Squad.
 * 
 * This code is licensed under the Attribution-NonCommercial-ShareAlike 3.0 (CC BY-NC-SA 3.0)
 * creative commons license. See <http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode>
 * for full details.
 * 
 * Attribution — You are free to modify this code, so long as you mention that the resulting
 * work is based upon or adapted from this code.
 * 
 * Non-commercial - You may not use this work for commercial purposes.
 * 
 * Share Alike — If you alter, transform, or build upon this work, you may distribute the
 * resulting work only under the same or similar license to the CC BY-NC-SA 3.0 license.
 * 
 * Note that Thunder Aerospace Corporation is a ficticious entity created for entertainment
 * purposes. It is in no way meant to represent a real entity. Any similarity to a real entity
 * is purely coincidental.
 */

using KSP.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Tac;

namespace KSPBioMass
{
    class SavedGameConfigWindow : Window<SavedGameConfigWindow>
    {
        private Settings globalSettings;
        private SaveGame saveGame;
        private GUIStyle labelStyle;
        private GUIStyle editStyle;
        private GUIStyle headerStyle;
        private GUIStyle headerStyle2;
        private GUIStyle warningStyle;
        private GUIStyle buttonStyle;


        private bool showBioMassConsumption = false;
 
        private readonly string version;

        public SavedGameConfigWindow(Settings globalSettings, SaveGame saveGame)
            : base("BioMass Settings", 400, 300)
        {
            base.Resizable = false;
            this.globalSettings = globalSettings;
            this.saveGame = saveGame;

            version = Utilities.GetDllVersion(this);
        }

        protected override void ConfigureStyles()
        {
            base.ConfigureStyles();

            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.alignment = TextAnchor.MiddleLeft;
                labelStyle.fontStyle = FontStyle.Normal;
                labelStyle.normal.textColor = Color.white;
                labelStyle.wordWrap = false;

                editStyle = new GUIStyle(GUI.skin.textField);
                editStyle.alignment = TextAnchor.MiddleRight;

                headerStyle = new GUIStyle(labelStyle);
                headerStyle.fontStyle = FontStyle.Bold;

                headerStyle2 = new GUIStyle(headerStyle);
                headerStyle2.wordWrap = true;

                buttonStyle = new GUIStyle(GUI.skin.button);

                warningStyle = new GUIStyle(headerStyle2);
                warningStyle.normal.textColor = new Color(0.88f, 0.20f, 0.20f, 1.0f);
            }
        }

        protected override void DrawWindowContents(int windowId)
        {
            GUILayout.Label("Version: " + version, labelStyle);
            GUILayout.Label("Configure BioMass", headerStyle);

            GUILayout.Space(10);
            BioMassConsumptionRates();

            if (GUI.changed)
            {
                SetSize(10, 10);
            }
        }

        private void BioMassConsumptionRates()
        {
            showBioMassConsumption = GUILayout.Toggle(showBioMassConsumption, "BioMass Consumption Rates", buttonStyle);

            if (showBioMassConsumption)
            {
                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label("These settings affect all saves. Restart KSP for changes to take effect.", warningStyle);
                GUILayout.Label("The following values are in units per day (24 hours).", headerStyle);

                globalSettings.WaterConsumptionRate = Utilities.ShowTextField("Water Consumption Rate", labelStyle,
                    globalSettings.WaterConsumptionRate * globalSettings.MaxDeltaTime, 30, editStyle, GUILayout.MinWidth(150)) / globalSettings.MaxDeltaTime;
                globalSettings.WasteWaterConsumptionRate = Utilities.ShowTextField("Waste Water Consumption Rate", labelStyle,
                    globalSettings.WasteWaterConsumptionRate * globalSettings.MaxDeltaTime, 30, editStyle, GUILayout.MinWidth(150)) / globalSettings.MaxDeltaTime;
                globalSettings.OxygenConsumptionRate = Utilities.ShowTextField("Oxygen Consumption Rate", labelStyle,
                    globalSettings.OxygenConsumptionRate * globalSettings.MaxDeltaTime, 30, editStyle, GUILayout.MinWidth(150)) / globalSettings.MaxDeltaTime;
                globalSettings.CO2ConsumptionRate = Utilities.ShowTextField("CarbonDioxide Consumption Rate", labelStyle,
                    globalSettings.CO2ConsumptionRate * globalSettings.MaxDeltaTime, 30, editStyle, GUILayout.MinWidth(150)) / globalSettings.MaxDeltaTime;

                GUILayout.Space(5);
                
                globalSettings.WaterProductionRate = Utilities.ShowTextField("Water Production Rate", labelStyle,
                    globalSettings.WaterProductionRate * globalSettings.MaxDeltaTime, 30, editStyle, GUILayout.MinWidth(150)) / globalSettings.MaxDeltaTime;
                globalSettings.CO2ProductionRate = Utilities.ShowTextField("CarbonDioxide Production Rate", labelStyle,
                    globalSettings.CO2ProductionRate * globalSettings.MaxDeltaTime, 30, editStyle, GUILayout.MinWidth(150)) / globalSettings.MaxDeltaTime;               
                globalSettings.OxygenProductionRate = Utilities.ShowTextField("Oxygen Production Rate", labelStyle,
                    globalSettings.OxygenProductionRate * globalSettings.MaxDeltaTime, 30, editStyle, GUILayout.MinWidth(150)) / globalSettings.MaxDeltaTime;

                GUILayout.Space(5);

                globalSettings.MaxDeltaTime = (int)Utilities.ShowTextField("Max Delta Time", labelStyle, globalSettings.MaxDeltaTime,
                    30, editStyle, GUILayout.MinWidth(150));
                globalSettings.ElectricityMaxDeltaTime = (int)Utilities.ShowTextField("Max Delta Time (Electricity)", labelStyle,
                    globalSettings.ElectricityMaxDeltaTime, 30, editStyle, GUILayout.MinWidth(150));

                GUILayout.EndVertical();
            }
        }

    }
}
