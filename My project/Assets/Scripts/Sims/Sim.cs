using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Overlays;
using UnityEngine;

//Monobehaviour script that manages the behaviour of a Sim entity in the simulation.
public class Sim : MonoBehaviour, ISaveable
{
    //Text element used to show the name of the Sim
    [SerializeField]
    private TMPro.TMP_Text nameText;

    //Text element used to show the traits of the Sim
    [SerializeField]
    private TMPro.TMP_Text traitsText;

    //Prefab of the needs visual used for visualizing the needs of the Sim
    [SerializeField]
    private GameObject needVisualPrefab;

    //Transform used to hold the need visuals that are instantiated
    [SerializeField]
    private Transform needVisualsHolder;

    //The visualization of the body - used for updating what the Sim looks like
    [SerializeField]
    private SpriteRenderer bodyVisualRenderer;

    //How fast the Sim can move
    [SerializeField]
    private float speed;

    //How often the Sim changes direction
    [SerializeField]
    private float moveDirectionChangeFrequency;

    private Vector3 moveDirection = Vector3.zero;

    //The time used to keep track of when the Sim should change directions
    private float timeSinceLastDirectionChange = 0f;

    //The data used to initialize and manage the behaviour of the Sim
    private SimData simData;

    //A dictionary used to manage the visuals of the need as the character updates over time.
    private Dictionary<NeedData.NeedType, NeedVisual> needVisualMap = new Dictionary<NeedData.NeedType, NeedVisual>();

    private Vector2 screenMin, screenMax;

    //Takes in data and initializes the properties of this particular Sim entity.
    public void Initialize(SimData inSimData)
    {
        simData = inSimData;
        bodyVisualRenderer.color = simData.simColour;
        nameText.text = simData.simName;
        nameText.color = simData.simColour;

        string traitsValue = "";
        foreach (SimData.Trait trait in simData.traits)
        {
            traitsValue += trait.ToString() + ", ";
        }
        traitsText.text = traitsValue;

        needVisualMap.Clear();

        //Iterates over each of the needs for the sim
        foreach (KeyValuePair<NeedData.NeedType, NeedData> need in simData.needsMap)
        {
            GameObject needObject = Instantiate(needVisualPrefab, needVisualsHolder);
            NeedVisual needVisual = needObject.GetComponent<NeedVisual>();
            if (need.Value.initialValue > 0f && need.Value.decrementValue < 0f)
            {
                //making sure that the need data is initialized before using it
                if (!need.Value.IsInitialized)
                {
                    need.Value.Initialize();
                }

                //Initializes the need and adds it to the visual map so that it can be easily accessed later
                needVisual.Initialize(need);
                needVisualMap.Add(need.Key, needVisual);
            }
        }

        moveDirection = UnityEngine.Random.insideUnitCircle;
        screenMin = Camera.main.ScreenToWorldPoint(Vector3.zero);
        screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
    }


    private void Update()
    {
        //Iterate over each of the needs
        foreach (NeedData need in simData.needsMap.Values)
        {
            //Decrement the value of the need
            need.currentValue += need.decrementValue * Time.deltaTime;

            //Handling for the character's hunger
            bool needRequiresExtraChecks = need.needType == NeedData.NeedType.hunger || need.needType == NeedData.NeedType.social;
            if (needRequiresExtraChecks)
            {
                if (!CheckNeed(need))
                {
                    continue;
                }
            }
        }

        timeSinceLastDirectionChange += Time.deltaTime;
        if (timeSinceLastDirectionChange > moveDirectionChangeFrequency)
        {
            timeSinceLastDirectionChange = 0f;
            moveDirection = UnityEngine.Random.insideUnitCircle;
        }
        transform.position += moveDirection.normalized * speed * Time.deltaTime;

        if (transform.position.x < screenMin.x ||
            transform.position.x > screenMax.x)
        {
            moveDirection.x *= -1;
        }
        if (transform.position.y < screenMin.y ||
            transform.position.y > screenMax.y)
        {
            moveDirection.y *= -1;
        }
    }

    private bool CheckNeed(NeedData need)
    {
        bool needShouldShowWarning = need.hasWarning && need.currentValue <= 0.4f && needVisualMap.ContainsKey(need.needType);

        if (needShouldShowWarning)
        {
            needVisualMap[need.needType].ShowWarning();
        }
        else
        {
            needVisualMap[need.needType].HideWarning();
        }

        return true;
    }

    public bool IsNeedLow(NeedData.NeedType needType)
    {
        if (simData.needsMap.ContainsKey(needType))
        {
            return simData.needsMap[needType].currentValue < 0.4f;
        }
        return false;
    }

    public void IncreaseNeed(NeedData.NeedType needType, float increment)
    {
        if (simData.needsMap.ContainsKey(needType))
        {
            simData.needsMap[needType].IncreaseValue(increment);
        }
    }

    public string Serialize()
    {
        simSaveData saveData = new simSaveData(simData, transform.position, moveDirection);
        return JsonUtility.ToJson(saveData);
    }

    public void Deserialize(string jsonData)
    {
        simSaveData s = JsonUtility.FromJson<simSaveData>(jsonData);
        transform.position = s.position;
        moveDirection = s.moveDirection;
        simData = s.simData;
        Initialize(simData);
    }

    [System.Serializable]
    public class simSaveData
    {
        public string prefabName = "Sim";
        public SimData simData;
        public Vector3 position;
        public Vector3 moveDirection;
        
        public simSaveData (SimData inSaveData, Vector3 inPosition, Vector3 inMoveDirection)
        {
            simData=inSaveData;
            position=inPosition;
            moveDirection=inMoveDirection;
        }
    }

}
