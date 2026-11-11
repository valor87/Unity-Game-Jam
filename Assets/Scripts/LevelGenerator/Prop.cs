using System.Collections;
using UnityEngine;

public class Prop : MonoBehaviour
{
    private Collider _thisCollider;
    [HideInInspector] public LevelGenerator lg;
    [HideInInspector] public propType pT;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _thisCollider = GetComponent<Collider>();
        StartCoroutine(LateStart());
    }
    
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(.25f);
        _thisCollider.isTrigger = false;
    }

    private void OnTriggerStay(Collider other)
    {
        Destroy(gameObject);
    }

    private void moveToARandomWorldSpace()
    {
        Vector3 newPos = new Vector3();
    }
    // Update is called once per frame
    void Update()
    {
        
    }


}
