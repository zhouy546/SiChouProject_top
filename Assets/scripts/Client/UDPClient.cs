using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace VideoClient
{
    public class UDPClient : MonoBehaviour
    {

     
        UdpClient udpServer;
        string receivedMessage=" ";

        private void Awake()
        {
            EventCenter.AddListener(EventDefine.ini, INI);
        }

        void INI()
        {
            //����UDP������
            udpServer = new UdpClient(ValueSheet.clientRoot.LocalUdpPort);
            udpServer.BeginReceive(new System.AsyncCallback(ReceiveCallback), null);
            Debug.Log("UDP Server started on port " + ValueSheet.clientRoot.LocalUdpPort);
        }

        void ReceiveCallback(System.IAsyncResult asyncResult)
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] receivedBytes = udpServer.EndReceive(asyncResult, ref remoteEndPoint);
            receivedMessage = Encoding.GetEncoding("gb2312").GetString(receivedBytes);

            //�����յ�����Ϣ
            Debug.Log("Message received from " + remoteEndPoint.Address + ":" + remoteEndPoint.Port + " - " + receivedMessage);

            //��ʼ������һ�����ݰ�
            udpServer.BeginReceive(new System.AsyncCallback(ReceiveCallback), null);
        }

        private void Update()
        {
            if (receivedMessage != " ")
            {
                dealwithMsg(receivedMessage);
                receivedMessage = " ";
            }
        }

        void dealwithMsg(string s)
        {
            Debug.Log("Running");

            if (ValueSheet.udp_videoinfo.ContainsKey(s))
            {
                Client_MediaCtr.instance.playVideo(s);
            }else if (s == ValueSheet.clientRoot.TriggerInteractionUDP)
            {
                EventCenter.Broadcast(EventDefine.ShowInteraction);

            }
            else if(s == ValueSheet.clientRoot.TriggerVideoUDP)
            {
                EventCenter.Broadcast(EventDefine.ShowVideo);
            }

            else if (s == ValueSheet.clientRoot.TriggerpbUDP)
            {
                EventCenter.Broadcast(EventDefine.ShowPb);
            }
        }

        void OnApplicationQuit()
        {
            //�ر�UDP������
            udpServer.Close();
        }
    }


}
