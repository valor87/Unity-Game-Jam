
using UnityEngine;
using System.Collections.Generic;
public class SlotMachine : MonoBehaviour
{
    public static SlotMachine Instance;
    public int firstNumber, secondNumber, thirdNumber;
    private void Start()
    {
        Instance = this;
    }

    public void RollRandomNumbers()
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        float randomNum = Random.Range(0, 100);
        if (randomNum > 99)
        {
            Debug.Log("Rolled 777");
            //777
            return;
        } else if (randomNum > 98)
        {
            Debug.Log("Rolled 666");
            //666
            return;
        } else
        {
            firstNumber = numbers[(int)(randomNum % numbers.Count)];
            numbers.Remove(firstNumber);
        }
        randomNum = Random.Range(0, 100);
        secondNumber = numbers[(int)(randomNum % numbers.Count)];
        numbers.Remove(secondNumber);
        randomNum = Random.Range(0, 100);
        thirdNumber = numbers[(int)(randomNum % numbers.Count)];
        Debug.Log($"rolled: {firstNumber} {secondNumber} {thirdNumber}");
    }
}


