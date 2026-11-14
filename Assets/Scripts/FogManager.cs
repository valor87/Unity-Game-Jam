using System.Data.SqlTypes;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    void Start()
    {
        EventCore.instance.UE_FoggyLevel.AddListener(SetFogValues);
        EventCore.instance.UE_EnterEndElevator.AddListener(TurnOffFog);
    }
    void SetFogValues()
    {
        RenderSettings.fog = true;
    }

    void TurnOffFog(Vector3 arg)
    {
        RenderSettings.fog = false;
    }
}
