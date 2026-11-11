using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Monobehaviour script that manages the visualization of a specific Need of a Sim.
public class NeedVisual : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text needText;

    [SerializeField]
    private float cycleTime;

    private enum State
    {
        idle, reddening, unreddening
    }
    private State currentState = State.idle;
    private float timeInCurrentState = 0f;
    private NeedData need;
    
    //Takes in a Need and initializes the visuals.
    public void Initialize(KeyValuePair<NeedData.NeedType, NeedData> inNeed)
    {
        need = inNeed.Value;
        needText.text = need.needType.ToString();
        needText.color = new Color(needText.color.r, needText.color.g, needText.color.b, need.initialValue);
    }

    public void ShowWarning()
    {
        currentState = State.reddening;
    }
    public void HideWarning()
    {
        currentState = State.idle;
    }

    private void Update()
    {
        switch(currentState)
        {
            case State.idle:
                needText.color = new Color(needText.color.r, needText.color.g, needText.color.b, need.currentValue / need.maxValue);
                break;
            case State.reddening:
                timeInCurrentState += Time.deltaTime;
                needText.color = new Color(needText.color.r, 1-timeInCurrentState / cycleTime, 1-timeInCurrentState / cycleTime, need.currentValue / need.maxValue);
                if (timeInCurrentState > cycleTime)
                {
                    timeInCurrentState = 0f;
                    currentState = State.unreddening;
                }
                break;
            case State.unreddening:
                timeInCurrentState += Time.deltaTime;
                needText.color = new Color(needText.color.r, timeInCurrentState / cycleTime, timeInCurrentState / cycleTime, need.currentValue / need.maxValue);
                if (timeInCurrentState > cycleTime)
                {
                    timeInCurrentState = 0f;
                    currentState = State.reddening;
                }
                break;
        }
    }
}
