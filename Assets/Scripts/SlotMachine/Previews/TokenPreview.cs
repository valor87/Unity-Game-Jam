using System.Net;
using System.Reflection;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class TokenPreview : MonoBehaviour
{

    public int Rollresult;

    public GameObject Slot1;
    public GameObject Slot2;
    public GameObject Slot3;

    public GameObject[] slots;

    //numbers
    public Material Num1;
    public Material Num2;
    public Material Num3;
    public Material Num4;
    public Material Num5;
    public Material Num6;
    public Material Num7;
    public Material Num8;
    public Material Num9;
    public Material Trnasparent;


    public int SlotNum;
  
    public string rollresult; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SlotNum = 0;
        Slot1.GetComponent<Renderer>().material = Trnasparent;
        Slot2.GetComponent<Renderer>().material = Trnasparent;
        Slot3.GetComponent<Renderer>().material = Trnasparent;

    }


    public void updateRollmaterials()
    {
      
        string scoreStr = rollresult.ToString();

        foreach (char digitChar in scoreStr)
        {

            // Stop if there are more digits than available slots
            if (SlotNum < 3) break;

            string digitString = digitChar.ToString();
            Debug.Log(digitString); // Outputs "1", "2", "3", "4", "5"

            SlotNum += 1;

            //Which slot wheel is this on?
            string StringSlotNum = "Slot" + SlotNum.ToString();

            //Whats the number that was picked?
            string StringMaterial = "Num" + digitString;

          


            GameObject CurrentSlot = GameObject.Find(StringSlotNum);


            if (StringMaterial == "Num1") CurrentSlot.GetComponent<Renderer>().material = Num1;
            if (StringMaterial == "Num2") CurrentSlot.GetComponent<Renderer>().material = Num2;
            if (StringMaterial == "Num3") CurrentSlot.GetComponent<Renderer>().material = Num3;
            if (StringMaterial == "Num4") CurrentSlot.GetComponent<Renderer>().material = Num4;
            if (StringMaterial == "Num5") CurrentSlot.GetComponent<Renderer>().material = Num5;
            if (StringMaterial == "Num6") CurrentSlot.GetComponent<Renderer>().material = Num6;
            if (StringMaterial == "Num7") CurrentSlot.GetComponent<Renderer>().material = Num7;
            if (StringMaterial == "Num8") CurrentSlot.GetComponent<Renderer>().material = Num8;
            if (StringMaterial == "Num9") CurrentSlot.GetComponent<Renderer>().material = Num9;


        }



        //reset

        SlotNum = 0;
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
