using UnityEngine;

public interface ISaveable 
{
    public string Serialize();
    public void Deserialize(string jsonData);

}
