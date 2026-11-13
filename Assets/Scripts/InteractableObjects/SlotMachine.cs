
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class SlotMachine : MonoBehaviour
{
    public static SlotMachine Instance;
    public bool interactable = true;
    [SerializeField] float timeForFirstNum, timeForSecondNum, timeForThirdNum, endTime;
    public int firstNumber, secondNumber, thirdNumber;
    private Coroutine slotMachineCR;
    private void Start()
    {
        Instance = this;
    }
    public void StartSlotMachine()
    {
        if (slotMachineCR != null)
        {
            return;
        }
        slotMachineCR = StartCoroutine(RunSlotMachineCoroutine());
    }
    private IEnumerator RunSlotMachineCoroutine()
    {
        //start elevator sound
        RollRandomNumbers();
        yield return new WaitForSeconds(timeForFirstNum);
        //DISPLAY FIRST NUM CODE HERE
        yield return new WaitForSeconds(timeForSecondNum);
        //DISPLAY SECOND NUM CODE HERE
        yield return new WaitForSeconds(timeForThirdNum);
        //DISPLAY THIRD NUM CODE HERE
        yield return new WaitForSeconds(endTime);
        SlotMachineAnimationEnds();
    }
    private void SlotMachineAnimationEnds()
    {
        //play ding sound effect, elevator reached floor
        Elevator.instance.MakeStartDoorsInteractable();
        slotMachineCR = null;
    }
    private void RollRandomNumbers()
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


