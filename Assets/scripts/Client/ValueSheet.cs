using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VideoClient
{
    public static class ValueSheet
    {
        public static ClientRoot clientRoot;

        
        public static ConfigRoot ConfigRoot;


         public static Dictionary<string, ClientvideoItem> udp_videoinfo = new Dictionary<string, ClientvideoItem>();


        public static string getPBUDP()
        {
            foreach (var item in udp_videoinfo)
            {
                if (item.Value.iscreenprotect)
                {
                    return item.Key;
                }
            }
            return "100000";
        }

    }

    public class ClientvideoItem
    {

        public int VideoIndex { get; set; }

        public string udp { get; set; }

        public int loop { get; set; }

        public string url { get; set; }

        public bool isBackToScreenProtect { get; set; }

        public bool iscreenprotect { get; set; }
    }

    public class ClientRoot
    {

        public List<ClientvideoItem> Clientvideo { get; set; }

        public int clientSyncRange { get; set; }

        public string serverIP { get; set; }

        public int OnPlayDelay { get; set; }

        public string TriggerInteractionUDP;

        public string TriggerVideoUDP;

        public int InteractionUDPport { get; set; }

        public int LocalUdpPort { get; set; }

        public int ServerTcpPort { get; set; }

        public string STOPVIDEO { get; set; }

        public string pause { get; set; }
    }


    public class JsonNode
    {


        public string textURL { get; set; }

        public string TextureURL { get; set; }

        public string TitleURL { get; set; }
    }

    public class nodes
    {
        public bool isInfoComplete { get; set; }

        public string introURL { get; set; }
        public string name { get; set; }


        public List<JsonNode> JsonNodes { get; set; }

    }

    public class ConfigRoot
    {
        public nodes left { get; set; }

        public nodes right { get; set; }
    }
}