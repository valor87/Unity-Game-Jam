using System.Net;
using System.Reflection;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class TokenPreview : MonoBehaviour
{
    public MeshRenderer[] slots = new MeshRenderer[3];

    [SerializeField] Material[] materials = new Material[10];

    void Start()
    {
        foreach(MeshRenderer r in slots)
        {
            if (r != null)
                r.material = materials[9];
        }

    }

    public void setSlotsTransparent()
    {
        slots[0].material = materials[9];
        slots[1].material = materials[9];
        slots[2].material = materials[9];
    }

    public void UpdateFirstNumberMaterial(int number)
    {
        slots[0].material = materials[number];
    }
    public void UpdateSecondNumberMaterial(int number)
    {
        slots[1].material = materials[number];
    }
    public void UpdateThirdNumberMaterial(int number)
    {
        slots[2].material = materials[number];
    }

}
