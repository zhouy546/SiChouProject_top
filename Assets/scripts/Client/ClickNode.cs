using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ClickNode : MonoBehaviour
{

    public Animator DefaultStateAnimator;
    public Animator OnClickAnimator;

    //public Animator EffectAnimator;
    //public Image handImage;

    public HandCtr handCtr;
    public Image IslandImage;

    private bool isFirstShow;
    private bool isShowClick;
    private bool timeLocker;

    public List<Vector3> Handpos=new List<Vector3>();

    Coroutine HideCoroutine;

    public void Onclick()
    {
        if (!isFirstShow)
        {
            FirstShowClick();
        }else if (!isShowClick)
        {
            if (timeLocker == false)
            {
                SecondShowClick();
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

}
