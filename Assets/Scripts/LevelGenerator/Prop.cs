using System.Collections;
using UnityEngine;

public class Prop : MonoBehaviour
{
    [HideInInspector] public LevelGenerator lg;
    [HideInInspector] public propType pT;
    private bool _stopDestroy = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LateStart());
    }
    
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(.25f);
        _stopDestroy = true;
        if(GetComponent<BoxCollider>() != null)
            GetComponent<BoxCollider>().isTrigger = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(!_stopDestroy)
            moveToARandomWorldSpace();
    }

    private void moveToARandomWorldSpace()
    {
        transform.position = lg.ReplaceObsticale(pT);
    }
    // Update is called once per frame
    void Update()
    {
        
    }


}
