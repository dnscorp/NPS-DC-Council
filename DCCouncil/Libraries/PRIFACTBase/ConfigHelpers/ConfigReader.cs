using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PRIFACT.PRIFACTBase.ConfigHelpers
{
    delegate void ConfigChangeHandler(object o, ConfigChangedEventArgs e);

    public class ConfigReader
    {
        private static ConfigReaderInternal publicReader;
        private static ConfigReaderInternal appSpecificReader;

        private static bool bIsLoaded = false;
        private static object lockobj = new object();

        //Avoid instances
        private ConfigReader() { }

        static void EnsureLoaded()
        {
            if (bIsLoaded)
            {
                return;
            }

            lock (lockobj)
            {
                if (bIsLoaded)
                    return;
                DoLoad();
                bIsLoaded = true;
            }
        }

        static void DoLoad()
        {
            string public_configFilePath = ConfigurationManager.AppSettings["PRIFACT_CONFIG_FILE_PATH"];
            string app_configFilePath = ConfigurationManager.AppSettings["PRIFACT_CONFIG_FILE_PATH_OVERRIDE"];
            publicReader = new ConfigReaderInternal(public_configFilePath);
            if (app_configFilePath != null)
            {
                appSpecificReader = new ConfigReaderInternal(app_configFilePath);
            }
            return;
        }

        /// <summary>
        /// Retrives a value from config based on a key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>string or null</returns>
        public static string GetValue(string key)
        {
            EnsureLoaded();
            key = key.ToLower();
            string result = null;
            if (appSpecificReader != null)
            {
                result = appSpecificReader.GetValue(key);
                if (result != null)
                    return result;
            }
            return publicReader.GetValue(key);
        }

        /// <summary>
        /// Retrives a string array based on a key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>string[] or null</returns>
        public static string[] GetValues(string key)
        {
            EnsureLoaded();
            key = key.ToLower();
            string[] results = null;
            if (appSpecificReader != null)
            {
                results = appSpecificReader.GetValues(key);
                if (results != null)
                    return results;
            }
            return publicReader.GetValues(key);
        }

        /// <summary>
        /// Gets a XMLElement from file based on xpath
        /// </summary>
        /// <param name="xpath">Xpath</param>
        /// <returns>XMLElement</returns>
        public static XmlElement GetXmlElement(string xpath)
        {
            EnsureLoaded();
            XmlElement el = null;
            if (appSpecificReader != null)
            {
                el = appSpecificReader.GetXmlElement(xpath);
                if (el != null)
                    return el;
            }
            return publicReader.GetXmlElement(xpath);
        }
    }

    /// <summary>
    /// Configuration File Change Event Argument
    /// </summary>
    class ConfigChangedEventArgs : EventArgs
    {
        public readonly string LastEvent;
        public ConfigChangedEventArgs(string lastEvent)
        {
            this.LastEvent = lastEvent;
        }
    }

    /// <summary>
    /// Custom Configuration File Reader 
    /// </summary>
    /// <remarks>
    /// Requires the config file to  be present to read the data
    /// WARNING (Known Bug) : does not detect changes made with Visual studio as it seems to supress them.
    /// so please edit in notepad or any other editor :)
    /// </remarks>
    class ConfigReaderInternal
    {
        #region Private  Static Members

        private Hashtable internalHash = new Hashtable();
        private FileSystemWatcher watcher;
        private string m_loadedConfigFilePath = null;
        private bool bChanged = false;

        private XmlDocument m_doc = null;

        #endregion

        #region Event Handler

        public event ConfigChangeHandler ChangeEvent;  // Change Event handler


        private readonly string m_configFilePath;
        public ConfigReaderInternal(string configFilePath)
        {
            m_configFilePath = configFilePath;
        }

        #endregion

        #region Public Static Functions
        /// <summary>
        /// Retrieves the string value based on a key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>string or null</returns>
        public string GetValue(string key)
        {
            EnsureLoaded();

            key = key.ToLower();
            if (internalHash.ContainsKey(key))
            {
                string value = internalHash[key].ToString();
                if (value.StartsWith("$$") && value.EndsWith("$$"))
                {
                    value = value.ToLower();

                    if (Convert.ToString(internalHash[value.Replace("$$", "").Trim()]).StartsWith("$$") && Convert.ToString(internalHash[value.Replace("$$", "").Trim()]).EndsWith("$$"))
                    {
                        return GetValue(Convert.ToString(internalHash[value.Replace("$$", "").Trim()]).Replace("$$", ""));
                    }
                    else
                    {
                        return Convert.ToString(internalHash[value.Replace("$$", "").Trim()]);
                    }
                }
                else
                    return Convert.ToString(internalHash[key]);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void OnFireEvent(ConfigChangedEventArgs e)
        {
            if (ChangeEvent != null)
                ChangeEvent(new object(), e);
        }

        /// <summary>
        /// retrives the values based on a comma seperated value
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>string[]</returns>
        public string[] GetValues(string key)
        {
            EnsureLoaded();

            key = key.ToLower();
            if (internalHash.ContainsKey(key))
            {
                if (internalHash[key].ToString().Split(',').GetType() == typeof(string[]))
                {
                    return internalHash[key].ToString().Split(',');
                }
            }
            return null;
        }


        /// <summary>
        /// retrives an xmlelement from config file
        /// </summary>
        /// <param name="xpath">xpath</param>
        /// <returns>XmlElement</returns>
        public XmlElement GetXmlElement(string xpath)
        {
            EnsureLoaded();
            XmlElement el = (XmlElement)m_doc.SelectSingleNode("appSettings/" + xpath);
            return el;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Event handler for file watcher, reloads the hash table
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void OnConfigChanged(object o, FileSystemEventArgs e)
        {
            bChanged = true;
            _Reset();
        }

        private void _Reset()
        {
            m_loadedConfigFilePath = null;
            internalHash = null;
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Changed -= new FileSystemEventHandler(OnConfigChanged);
                watcher.Dispose();
                watcher = null;
            }
        }
        #endregion

        #region Private Static Functions
        /// <summary>
        /// To check if required objects are initialized
        /// </summary>
        void EnsureLoaded()
        {
            if (m_configFilePath == null)
            {
                throw new Exception("Config file path entry is not specified in the application configuration file.");
            }

            bool bConfigFileChanged = (m_configFilePath != m_loadedConfigFilePath);

            if (!bConfigFileChanged && internalHash != null && watcher != null)
                return;

            _Reset();

            internalHash = LoadSettings(m_configFilePath);
            m_loadedConfigFilePath = m_configFilePath;
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>HashTable with filled contents</returns>
        /// <exception cref="ArgumentException">Key already exists</exception>
        /// <exception cref="Exception">Config File Not Set</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private Hashtable LoadSettings(string configFilePath)
        {
            Hashtable hsh = new Hashtable();
            m_doc = new XmlDocument();
            try
            {
                m_doc.Load(configFilePath);
            }
            catch (XmlException ex)
            {
                throw;
            }

            XmlNodeList nodes = m_doc.SelectNodes("appSettings/add");
            if (nodes == null) return null;

            foreach (XmlNode node in nodes)
            {
                string key = node.Attributes.GetNamedItem("key").Value;
                StringBuilder value = new StringBuilder();
                if (key != null)
                {
                    if (node.Attributes.Count == 2)
                    {
                        value = new StringBuilder(node.Attributes.GetNamedItem("value").Value);
                        // TODO: Check for key exceptions
                        try
                        {
                            hsh.Add(key.ToLower(), value.ToString());
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                    else
                    {
                        XmlAttributeCollection colls = node.Attributes;
                        foreach (XmlAttribute attrib in colls)
                        {
                            if (attrib.Name != "key")
                            {
                                if (value.Length > 1)
                                    value.Append(",");
                                value.AppendFormat(attrib.Value);
                            }
                        }
                        // TODO : Check for identical key exceptions
                        try
                        {
                            hsh.Add(key.ToLower(), value.ToString().Split(','));
                        }
                        catch (ArgumentException ex)
                        {
                            // key already exists
                            throw;
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }
            }

            FileInfo configfile = new FileInfo(configFilePath);
            watcher = new FileSystemWatcher(configfile.DirectoryName);
            watcher.Filter = configfile.Name;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += new FileSystemEventHandler(OnConfigChanged);
            watcher.EnableRaisingEvents = true;
            m_loadedConfigFilePath = configFilePath;
            if (bChanged)
            {
                bChanged = false;
                OnFireEvent(new ConfigChangedEventArgs("Changed"));
            }
            return hsh;
        }
        #endregion
    }
}
