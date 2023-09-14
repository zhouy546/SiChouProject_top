using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HandCtr : MonoBehaviour
{
    public Animator EffectAnimator;
    public Image handImage;
    public ClickNode clickNode;
    
    public void SetPosition(Vector3 pos)
    {
        this.transform.localPosition = pos;
    }
    public async void OnFisetClick()
    {
        hideHand();
        triggerEffects();

        await Task.Delay(1000);

        SetPosition(clickNode.Handpos[1]);
        showHand();
    }

    public async void OnSecondClick()
    {
        hideHand();
        triggerEffects();

        await Task.Delay(1000);

        SetPosition(clickNode.Handpos[0]);
    }


    private void triggerEffects()
    {
        EffectAnimator.SetTrigger("show");
    }

    public void hideHand()
    {
        handImage.enabled = false;
    }

    public void showHand()
    {
        handImage.enabled = true;
    }
}
