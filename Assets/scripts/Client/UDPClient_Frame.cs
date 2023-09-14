using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace VideoClient
{
    public class UDPClient_Frame : MonoBehaviour
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
            udpServer = new UdpClient(ValueSheet.clientRoot.ServerTcpPort);
            udpServer.BeginReceive(new System.AsyncCallback(ReceiveCallback), null);
            Debug.Log("UDP Server started on port " + ValueSheet.clientRoot.ServerTcpPort);
        }

        void ReceiveCallback(System.IAsyncResult asyncResult)
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] receivedBytes = udpServer.EndReceive(asyncResult, ref remoteEndPoint);
            receivedMessage = Encoding.GetEncoding("gb2312").GetString(receivedBytes);

            //�����յ�����Ϣ
            //Debug.Log("Message received from " + remoteEndPoint.Address + ":" + remoteEndPoint.Port + " - " + receivedMessage);

            //��ʼ������һ�����ݰ�
            udpServer.BeginReceive(new System.AsyncCallback(ReceiveCallback), null);
        }

        private void Update()
        {
            if (receivedMessage != " ")
            {

                //Debug.Log("Running");
                dealwithMsg(receivedMessage);
                receivedMessage = " ";
            }
        }

        void dealwithMsg(string s)
        {
           

            EventCenter.Broadcast(EventDefine.checkFrame, s);
        }

        void OnApplicationQuit()
        {
            //�ر�UDP������
            udpServer.Close();
        }
    }


}
