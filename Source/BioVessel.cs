using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tac;

namespace KSPBioMass
{
    public class BioVessel
    {
        public const string ConfigNodeName = "BioVessel";

        public string vesselName;
        public string vesselType;
        public double BioMass;
        public int numOccupiedParts;

        public double lastUpdate;

        public double MaxBioMass;

        public BioVessel(string vesselName, double currentTime)
        {
            this.vesselName = vesselName;
            lastUpdate = currentTime;
        }

        public static BioVessel Load(ConfigNode node)
        {
            string vesselName = Utilities.GetValue(node, "vesselName", "Unknown");
            double lastUpdate = Utilities.GetValue(node, "lastUpdate", 0.0);

            BioVessel info = new BioVessel(vesselName, lastUpdate);
            info.vesselType = Utilities.GetValue(node, "vesselType", "Unknown");
            info.numOccupiedParts = Utilities.GetValue(node, "numOccupiedParts", 0);
            info.BioMass = Utilities.GetValue(node, "bioMass", 0.0);
            return info;
        }

        public ConfigNode Save(ConfigNode config)
        {
            ConfigNode node = config.AddNode(ConfigNodeName);
            node.AddValue("vesselName", vesselName);
            node.AddValue("vesselType", vesselType);
            node.AddValue("bioMass", BioMass);
            node.AddValue("numOccupiedParts", numOccupiedParts);

            node.AddValue("lastUpdate", lastUpdate);
            return node;
        }

        public void ClearAmounts()
        {
            numOccupiedParts = 0;
            BioMass = 0.0;
            MaxBioMass = 0.0;
        }

        public enum Status
        {
            GOOD,
            LOW,
            CRITICAL
        }
}
