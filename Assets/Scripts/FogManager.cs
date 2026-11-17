using System.Data.SqlTypes;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    public float fogEndDistance;
    void Start()
    {
        EventCore.instance.UE_FoggyLevel.AddListener(SetFogValues);
        EventCore.instance.UE_EnterEndElevator.AddListener(TurnOffFog);
    }
    private void Update()
    {
        RenderSettings.fogEndDistance = fogEndDistance;
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
