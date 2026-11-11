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
    }

    private void OnTriggerStay(Collider other)
    {
        if (pT == propType.Floating)
        {
            lg._amountOfFloatingProps++;
        }
        else if(pT == propType.Ground)
        {
            lg._amountOfGroundProps++;
        }
        else if (pT == propType.Roof)
        {
            lg._amountOfRoofProps++;
        }

        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }


}
