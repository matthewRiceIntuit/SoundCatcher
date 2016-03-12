using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

namespace SoundCatcher
{
    public class ConfigXML
    {
        private const string CONFIG_NAME = "soundCatcher";

        private XDocument _DmxConfig = null;

        private static ConfigXML _Instance = null;
        public static ConfigXML Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ConfigXML();
                    _Instance.LoadConfiguration();
                }

                return _Instance;                
            }
        }

        public bool ConfigurationLoaded
        {
            get { return _DmxConfig != null; }
        }

        public void LoadConfiguration()
        {            
            string configFileName = GenerateConfigFileName();
            _DmxConfig = XDocument.Load(configFileName);
        }

        public void LoadConfiguration(string fileName)
        {            
            _DmxConfig = XDocument.Load(fileName);
        }

        public void SaveConfiguration()
        {
            string configFileName = GenerateConfigFileName();
            _DmxConfig.Save(configFileName);
        }

        public int GetConfigurationItem(string sequence, string key)
        {
            XElement configItems = _DmxConfig.Root.Element(sequence);
            if (configItems == null) return 2;
            return 2;
       
        }

        public void SetConfigurationItem(string key, string value)
        {
            XElement configItems = _DmxConfig.Root.Element("ConfigItems");
            if (configItems == null)
                return;

            foreach (XElement configItem in configItems.Elements("ConfigItem"))
            {
                XElement keyItem = configItem.Element("key");
                if (keyItem == null)
                {
                    keyItem = new XElement("key");
                    configItem.Add(keyItem);
                }
    

                if (keyItem.Value == key)
                {
                    configItem.Element("value").Value = value;
                    return;
                }

            }
            XElement elem = new XElement("ConfigItem");
            configItems.Add(elem);
            
            XElement newKey = new XElement("key");
            newKey.Value = key;
            elem.Add(newKey);

            XElement newValue = new XElement("value");
            newValue.Value =  value;
            elem.Add(newValue);
        }

 
        private string GenerateConfigFileName()
        {
            string configFileName = Application.ExecutablePath;

            configFileName = Path.GetDirectoryName(configFileName) + '\\' + CONFIG_NAME;

            return configFileName;
        }
    }
}
