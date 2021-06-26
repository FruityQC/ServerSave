using System;
using Newtonsoft.Json;

namespace Oxide.Plugins
{
    [Info("Server Save", "DaGamer", "1.0.5")]
    [Description("A plugin that will allow granted user/group to save the server")]

    class ServerSave : RustPlugin
    {
        #region Config
        private ConfigData configData;
        class ConfigData
        {
            [JsonProperty(PropertyName = "Broadcast Save Message")]
            public bool on = false; //Send message on save ?

            [JsonProperty(PropertyName = "Message color (Hex Color)")]
            public string color = "<color=#ffbf00>"; //Msg color

            [JsonProperty(PropertyName = "Save message")]
            public string msg = "Server Saved"; //Custom message
        }

        private bool LoadConfigVariables()
        {
            try
            {
                configData = Config.ReadObject<ConfigData>();
            }

            catch
            {
                return false;
            }

            SaveConfig(configData);
            return true;
        }

        protected override void LoadDefaultConfig()
        {
            Puts("Creating new config file.");
            configData = new ConfigData();
            SaveConfig(configData);
        }

        void SaveConfig(ConfigData config)
        {
            Config.WriteObject(config, true);
        }

        #endregion

        void Init()
        {
            permission.RegisterPermission("ServerSave.Save", this);
            if (!LoadConfigVariables())
            {
                //If its false there is an error. so we will print that to console.
                Puts("Config file issue detected. Please delete file, or check syntax and fix.");
                return;
            }
        }


        void OnServerSave()
        {
            if (configData.on)
            {
                PrintToChat(configData.color + configData.msg + " </color>"); //Printed to chat
            }
            PrintToConsole("<color=#ffbf00> Server Saved </color>"); //console
        }

        [ChatCommand("Save")]
        void ServerSaveCommand(BasePlayer player)
        {
            if (permission.UserHasPermission(player.UserIDString, "ServerSave.Save"))
            {
                rust.RunServerCommand("save"); // Player has permission, do special stuff for them
            }
            else
            {
                SendReply(player, "You do not have pemission to use this command");
            }
        }


    }
}