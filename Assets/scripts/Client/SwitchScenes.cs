using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using VideoClient;
public class SwitchScenes : MonoBehaviour
{
    public ClickNode[] clickNodes;

    public static SwitchScenes instance;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.AddListener(EventDefine.ShowVideo, HideInterAction);
        EventCenter.AddListener(EventDefine.ShowInteraction, ShowInterAction);
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HideInterAction()
    {
        Client_MediaCtr.instance.ShowVideo();

    }

    private void ShowInterAction()
    {
        Client_MediaCtr.instance.HideVideo();

    } 


}
