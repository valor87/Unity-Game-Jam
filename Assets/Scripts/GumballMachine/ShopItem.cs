using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour
{
    [SerializeField] int cost;
    [SerializeField] Vector3 direction;
    [SerializeField] float floatSpeed, changeDirectionTime, currentTime;

    private void Start()
    {
        ChooseNewDirection();   
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > changeDirectionTime)
        {
            currentTime = 0;
            ChooseNewDirection();
        }
    }
    private void ChooseNewDirection()
    {
        direction = new Vector3(Random.Range(0, 1),0, Random.Range(0, 1));
        //rb.linearVelocity = direction * floatSpeed;
    }
    public virtual void purchase()
    {
        CoinCollect.instance.currentCoins -= cost;
        Destroy(gameObject);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Ive been clicked");
        if (CoinCollect.instance.currentCoins > cost) purchase();
    }
}
