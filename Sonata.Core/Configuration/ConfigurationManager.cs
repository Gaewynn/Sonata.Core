#region Namespace Sonata.Core.Configuration
//	The Sonata.Core.Configuration namespace contains the types that provide the programming model for handling configuration data.
#endregion

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Sonata.Core.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {
        #region Members

        private XDocument _configFile;
        private IEnumerable<XElement> _appSettings;

        #endregion

        #region Methods

        #region IConfigurationService Members

        public void Initialize(string configurationFile)
        {
            _configFile = XDocument.Load(configurationFile);
            _appSettings = _configFile.Descendants("appSettings");
        }

        public string Get(string key)
        {
            return (from addNode in _appSettings.Descendants().Where(e => e.NodeType == System.Xml.XmlNodeType.Element)
                    where addNode.Attribute("key")?.Value == key
                    select addNode.Attribute("value")?.Value).FirstOrDefault();
        }

        public T Get<T>(string key)
        {
            return JsonConvert.DeserializeObject<T>(Get(key));
        }

        #endregion

        #endregion
    }
}
