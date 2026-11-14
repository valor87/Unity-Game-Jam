
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
public class SlotMachine : MonoBehaviour
{
    public static SlotMachine Instance;
    public bool interactable = true;
    [SerializeField] float timeForFirstNum, timeForSecondNum, timeForThirdNum, endTime;
    public int firstNumber, secondNumber, thirdNumber;
    private Coroutine slotMachineCR;
    [SerializeField] SlotMachineInteract _slotMachineInteract;
    [SerializeField] TokenPreview _tokenPreview;
    private SoundManager.SlotResult rolledResult;
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
        RollRandomNumbers();
            if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySlotMachineThenElevator(rolledResult, transform);
        yield return new WaitForSeconds(timeForFirstNum);
        _tokenPreview.UpdateFirstNumberMaterial(firstNumber);
        yield return new WaitForSeconds(timeForSecondNum);
        _tokenPreview.UpdateSecondNumberMaterial(secondNumber);
        yield return new WaitForSeconds(timeForThirdNum);
        _tokenPreview.UpdateThirdNumberMaterial(thirdNumber);

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
            rolledResult = SoundManager.SlotResult.Jackpot777;
            PhobiaEvents.RaisePhobiaAdded(PhobiaEvents.Phobia777);
            //777
            return;
        } else if (randomNum > 98)
        {
            Debug.Log("Rolled 666");
            rolledResult = SoundManager.SlotResult.Ominous666;
            PhobiaEvents.RaisePhobiaAdded(PhobiaEvents.Phobia666);
            //666
            return;
        } else
        {
            firstNumber = numbers[(int)(randomNum % numbers.Count)];
            numbers.Remove(firstNumber);
            rolledResult = SoundManager.SlotResult.Normal;
        }
        randomNum = Random.Range(0, 100);
        secondNumber = numbers[(int)(randomNum % numbers.Count)];
        numbers.Remove(secondNumber);
        randomNum = Random.Range(0, 100);
        thirdNumber = numbers[(int)(randomNum % numbers.Count)];
        PhobiaManager.instance.AddPhobia(thirdNumber);
        Debug.Log($"rolled: {firstNumber} {secondNumber} {thirdNumber}");
    }
    
    public void ResetSlotMachineInteracts()
    {
        _slotMachineInteract.ResetValues();
        //endMachine.ResetValues();
    }
}


