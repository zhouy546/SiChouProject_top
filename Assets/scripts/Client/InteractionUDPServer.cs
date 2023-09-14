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
            //创建UDP服务器
            udpServer = new UdpClient(ValueSheet.clientRoot.InteractionUDPport);
            udpServer.BeginReceive(new System.AsyncCallback(ReceiveCallback), null);
            //Debug.Log("UDP Server started on port " + port);
        }

        void ReceiveCallback(System.IAsyncResult asyncResult)
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] receivedBytes = udpServer.EndReceive(asyncResult, ref remoteEndPoint);
            receivedMessage = Encoding.GetEncoding("gb2312").GetString(receivedBytes);

            //处理收到的消息
            Debug.Log("Message received from " + remoteEndPoint.Address + ":" + remoteEndPoint.Port + " - " + receivedMessage);

            //开始接收下一个数据包
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
            //关闭UDP服务器
            udpServer.Close();
        }


        bool isStringNum(string s,out int num)
        {
            if (int.TryParse(s, out num))
            {
                // 如果转换成功result会包含转换后的值
                return true;
            }
            else
            {
                return false;
            }
        }
    }


}
