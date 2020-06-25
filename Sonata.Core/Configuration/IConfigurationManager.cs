#region Namespace Sonata.Core.Configuration
//	The Sonata.Core.Configuration namespace contains the types that provide the programming model for handling configuration data.
#endregion


namespace Sonata.Core.Configuration
{
    public interface IConfigurationManager
    {
        #region Methods

        void Initialize(string configurationFile);

        string Get(string key);

        T Get<T>(string key);

        #endregion
    }
}
