﻿using System;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

/*
    class OptionsFile
        OptionsFile()
        loadConfigFile()
        saveConfigFile()
        getPortableConfigFilePath()
        getGlobalConfigFileDirectory()
        getGlobalConfigFilePath()
        openConfigDir()
        
*/

namespace Diagram
{
    /// <summary>    
    /// </summary>
    public class OptionsFile
    {
        /*************************************************************************************************************************/

        public String configFileDirectory = "Diagram"; // name of directory for save global configuration file

#if DEBUG
        // global configuration file name in debug mode
        public String configFileName = "diagram.debug.json";
#else
        // global configuration file name
        public String configFileName = "diagram.json";
#endif

        public String optionsFilePath = ""; // full path to global options json file

        public ProgramOptions parameters = null;

        /*************************************************************************************************************************/

        /// <summary>
        /// load global config file from portable file configuration or global file configuration
        /// </summary>
        /// <param name="parameters">reference to parameter object</param>
        public OptionsFile(ProgramOptions parameters)
        {
            this.parameters = parameters;

            this.optionsFilePath = this.GetPortableConfigFilePath();

            // use global if portable version not exist
            if (!Os.FileExists(this.optionsFilePath))
            {
                this.optionsFilePath = this.GetFullGlobalConfigFilePath();
            }

            if (Os.FileExists(this.optionsFilePath))
            {
                this.LoadConfigFile();
            }
            else
            {
                // create global config directory if not exist
                if (!Os.DirectoryExists(this.GetGlobalConfigFileDirectory()))
                {
                    Os.CreateDirectory(this.GetGlobalConfigFileDirectory());
                }

                this.SaveConfigFile();
            }
        }


        /// <summary>
        /// load global config file from json file</summary>
        private void LoadConfigFile()
        {
            try
            {
                Program.log.write("loadConfigFile: path:" + this.optionsFilePath);
                string inputJSON = Os.ReadAllText(this.optionsFilePath);
                this.parameters.SetParams(JsonConvert.DeserializeObject<ProgramOptions>(inputJSON));
            }
            catch (Exception ex)
            {
                Program.log.write("loadConfigFile: " + ex.Message);
            }
        }

        /// <summary>
        /// save global config file as json</summary>
        public void SaveConfigFile()
        {
            try
            {
                string outputJSON = JsonConvert.SerializeObject(this.parameters);
                Os.WriteAllText(this.optionsFilePath, outputJSON);
            }
            catch (Exception ex)
            {
                Program.log.write("saveConfigFile: " + ex.Message);
            }
        }

        /*************************************************************************************************************************/

        /// <summary>
        /// get config file directory when diagram app is used in portable mode</summary> 
        public String GetPortableConfigFilePath()
        {
            return Os.Combine(
                Os.GetDirectoryName(Application.ExecutablePath),
                this.configFileName
            );
        }

        /// <summary>
        /// get config file directory</summary> 
        private string GetGlobalConfigFileDirectory()
        {
			string folderPath = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
			string configDirectory = Os.Combine(
				folderPath,
                this.configFileDirectory
            );

			return configDirectory;
        }

        /// <summary>
        /// get full config file path</summary> 
        private string GetFullGlobalConfigFilePath()
        {
            return Os.Combine(
                this.GetGlobalConfigFileDirectory(),
                this.configFileName
            );
        }

        /// <summary>
        /// open directory with configuration file</summary> 
        public void ShowDirectoryWithConfiguration()
        {
            Os.ShowDirectoryInExternalApplication(Os.GetDirectoryName(optionsFilePath));
        }
    }
}
