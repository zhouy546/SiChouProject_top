using DemolitionStudios.DemolitionMedia;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace VideoClient
{
    public class Client_MediaCtr : MonoBehaviour
    {
        public Media mediaPlayer;

        public RenderToUGUI renderToUGUI;

        ClientvideoItem CurrentClientvideoItem;

        public static Client_MediaCtr instance;

        public int currentFrame;

        public void Awake()
        {
            EventCenter.AddListener(EventDefine.ini, ini);

            EventCenter.AddListener<string>(EventDefine.checkFrame, checkFrame);

        }


        public void ini()
        {
            instance = this;

            playVideo(ValueSheet.getPBUDP());
        }


        public async void playVideo(string udp)
        {
            await PlayVideo(udp);
        }


        private async Task PlayVideo(string udp)
        {
            CurrentClientvideoItem = ValueSheet.udp_videoinfo[udp];

            string path = Application.streamingAssetsPath + "/" + CurrentClientvideoItem.url;

            Debug.Log(path);

            mediaPlayer.Open(path);

            mediaPlayer.Loops = CurrentClientvideoItem.loop;

            renderToUGUI.color = Color.black;

            await Task.Delay(ValueSheet.clientRoot.OnPlayDelay);

            renderToUGUI.color = Color.white;

            mediaPlayer.Play();


        }

        private void checkFrame(string frame)
        {
           // Debug.Log("checkFrameRunning");

            int serverFrame = int.Parse(frame);

            currentFrame = mediaPlayer.VideoCurrentFrame;

            if (serverFrame != currentFrame)
            {

                if (DeltaFtame(currentFrame, serverFrame) > ValueSheet.clientRoot.clientSyncRange)
                {

                    mediaPlayer.SeekToFrame((int)serverFrame+1);

                    mediaPlayer.Play();
                }
            }
        }

        private int DeltaFtame(int _currentFrame, int _serverFrame)
        {
            int temp = (int)Mathf.Abs(_currentFrame - _serverFrame);

            return temp;
        }
        public void HideVideo()
        {
            renderToUGUI.color = new Color(1, 1, 1, 0);
        }

        public void ShowVideo()
        {
            renderToUGUI.color = new Color(1, 1, 1, 1);

        }
    }
}
