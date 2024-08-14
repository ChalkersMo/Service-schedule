using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private GameObject personPanel;
    [SerializeField] private GameObject instrumentPanel;

    [SerializeField] private Transform instrumentContent;

    [SerializeField] private GameObject instrumentPrefab;
    [SerializeField] private GameObject personPrefab;

    [SerializeField] private List<InstrumentScriptable> instruments;

    private RoomSavingData roomSavingData;

    private void Start()
    {
        Invoke(nameof(DelayedStart), 1);
    }

    private void DelayedStart()
    {
        roomSavingData = GetComponent<RoomSavingData>();
        if (roomSavingData.Instruments != null)
        {
            foreach (var instrument in roomSavingData.Instruments)
            {
                AddInstrument(instrument);
            }
        }
    }
    public void AddInstrument(string _name)
    {
        GameObject tempInstrument = Instantiate(instrumentPrefab, instrumentContent);
        InstrumentController tempInstController = tempInstrument.GetComponent<InstrumentController>();
        AddScriptableToInstrument(tempInstController, _name);
        instruments.Add(tempInstController.instrument);
    }
    private void AddScriptableToInstrument(InstrumentController _instrument, string _name)
    {
        foreach (var instrument in instruments)
        {
            if (instrument.Type == _name) 
            {
                _instrument.instrument = instrument;
                return;
            }
        }

        Debug.LogWarning("Entered invalid instrument name");
    }
}
