using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using VideoClient;

public class ClickNode : MonoBehaviour
{

    public Animator DefaultStateAnimator;
    public Animator OnClickAnimator;

    //public Animator EffectAnimator;
    //public Image handImage;

    public HandCtr handCtr;
    public Image IslandImage;

    public Image introImage;
    public List<Image> text_Images = new List<Image>();

    public List<Image> texture_Images = new List<Image>();

    public List<Image> Title_Images = new List<Image>();

    private bool isFirstShow;
    private bool isShowClick;
    private bool timeLocker;

    public List<Vector3> Handpos=new List<Vector3>();

    Coroutine HideCoroutine;

    public enum Interaction_position { left, right }

    public Interaction_position interaction_Position;

    public bool isInfoComplete = true;

    public Sprite introSprite;
    public List<Sprite> text_Sprite = new List<Sprite>();
    public List<Sprite> texture_Sprite = new List<Sprite>();
    public List<Sprite> Title_Sprite = new List<Sprite>();


    public void Awake()
    {
        EventCenter.AddListener(EventDefine.ini, setupDataFromJson);
    }

    public void setupDataFromJson()
    {
        switch (interaction_Position)
        {
            case Interaction_position.left:

                setup(ValueSheet.ConfigRoot.left);
                break;
            case Interaction_position.right:
                setup(ValueSheet.ConfigRoot.right);

                break;
            default:
                break;
        }
    }

    public void Onclick()
    {
        if (!isFirstShow)
        {
            FirstShowClick();
        }else if (!isShowClick)
        {
            if (timeLocker == false)
            {
                if (isInfoComplete)
                {
                    SecondShowClick();
                }

            }
        }
    }

    private async void FirstShowClick()
    {
        Debug.Log("第一次点击");
        isFirstShow = true;

        timeLocker = true;


        handCtr.OnFisetClick();

        DefaultStateAnimator.SetBool("Show", true);

        OnClickAnimator.SetBool("Show", false);

        HideCoroutine = StartCoroutine(HideOnClick());

        await Task.Delay(2000);

        timeLocker = false;
    }
    private async void SecondShowClick()
    {
        if (HideCoroutine != null) {
            StopCoroutine(HideCoroutine);
        }

        handCtr.OnSecondClick();

        Debug.Log("第二次点击");
        IslandImage.enabled = false;
        isShowClick = true;

        await Task.Delay(1000);

        DefaultStateAnimator.SetBool("Show", false);

        OnClickAnimator.SetBool("Show", true);

        HideCoroutine =StartCoroutine(HideOnClick());
    }



    public IEnumerator HideOnClick()
    {
        yield return new WaitForSeconds(15f);

        isShowClick = false;
        isFirstShow = false;
        IslandImage.enabled = true;
        OnClickAnimator.SetBool("Show", false);
        DefaultStateAnimator.SetBool("Show", false);

        handCtr.SetPosition(Handpos[0]);

        handCtr.showHand();
    }

    public async void setup(nodes _nodes)
    {
        string path = Application.streamingAssetsPath;
        isInfoComplete = _nodes.isInfoComplete;

        introImage.sprite = introSprite = await getTexture(path + _nodes.introURL);

        foreach (var item in _nodes.JsonNodes)
        {
            int index = _nodes.JsonNodes.IndexOf(item);

            Sprite texturesprite = await getTexture(path + item.TextureURL);

            texture_Images[index].sprite = texturesprite;

            texture_Sprite.Add(texturesprite);


            Sprite textsprite = await getTexture(path + item.textURL);

            text_Images[index].sprite = textsprite;

            texture_Sprite.Add(textsprite);


            Sprite titlesprite = await getTexture(path + item.TitleURL);

            Title_Images[index].sprite = titlesprite;

            Title_Sprite.Add(titlesprite);
        }
    }


    public async Task<Sprite> getTexture(string url)
    {
        Texture2D _texture = await GetRemoteTexture(url);
        Sprite sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));

        return sprite;
    }
    public async Task<Texture2D> GetRemoteTexture(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            // begin request:
            var asyncOp = www.SendWebRequest();

            // await until it's done: 
            while (asyncOp.isDone == false)
                await Task.Delay(1000 / 30);//30 hertz

            // read results:
            if (www.isNetworkError || www.isHttpError)
            {
                // log error:
#if DEBUG
                Debug.Log($"{www.error}, URL:{www.url}");
#endif

                // nothing to return on error:
                return null;
            }
            else
            {
                // return valid results:
                return DownloadHandlerTexture.GetContent(www);
            }
        }
    }
}
