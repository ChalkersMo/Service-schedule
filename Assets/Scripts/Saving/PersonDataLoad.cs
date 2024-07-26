using System.IO;
using UnityEngine;

public class PersonDataLoad : MonoBehaviour
{
    public static PersonDataLoad Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public PersonSetUp LoadData()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/PersonData.json");
        return JsonUtility.FromJson<PersonSetUp>(json);
    }
}
