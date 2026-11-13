using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    void Update()
    {
        transform.eulerAngles += new Vector3(0,5,0) * rotationSpeed * Time.deltaTime;
    }
}
