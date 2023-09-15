using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace VideoClient
{
    public class Client_fetchjson : MonoBehaviour
    {

        
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(LoadJson());
        }

        // Update is called once per frame
        void Update()
        {

        }
        IEnumerator LoadJson()
        {
            string url = Application.streamingAssetsPath + "/Client.json";

            WWW www = new WWW(url);

            yield return www;

            string jsonString = Encoding.UTF8.GetString(www.bytes);

            ValueSheet.clientRoot = JsonMapper.ToObject<ClientRoot>(jsonString.ToString());

            foreach (var item in ValueSheet.clientRoot.Clientvideo)
            {
                ValueSheet.udp_videoinfo.Add(item.udp, item);
            }

            string configurl = Application.streamingAssetsPath + "/config.json";

            WWW Configwww = new WWW(configurl);

            yield return Configwww;

            string ConfigjsonString = Encoding.UTF8.GetString(Configwww.bytes);

            ValueSheet.ConfigRoot = JsonMapper.ToObject<ConfigRoot>(ConfigjsonString);

            EventCenter.Broadcast(EventDefine.ini);
        }
    }
}

