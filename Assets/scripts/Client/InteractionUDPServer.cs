using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace VideoClient
{
    public class InteractionUDPServer : MonoBehaviour
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
            udpServer = new UdpClient(ValueSheet.clientRoot.InteractionUDPport);
            udpServer.BeginReceive(new System.AsyncCallback(ReceiveCallback), null);
            //Debug.Log("UDP Server started on port " + port);
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

            int num;
            if(isStringNum(s, out num))
            {
                if(num<=SwitchScenes.instance.clickNodes.Length)
                {
                    SwitchScenes.instance.clickNodes[num - 1].Onclick();

                }
            }
        }

        void OnApplicationQuit()
        {
            //�ر�UDP������
            udpServer.Close();
        }


        bool isStringNum(string s,out int num)
        {
            if (int.TryParse(s, out num))
            {
                // ���ת���ɹ�result�����ת�����ֵ
                return true;
            }
            else
            {
                return false;
            }
        }
    }


}
