using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Dropdown))]
public class QualityDropdownUI : MonoBehaviour
{
    private Dropdown m_dropdown;

    void Start()
    {
        m_dropdown = GetComponent<Dropdown>();
        m_dropdown.ClearOptions();

        string[] names = QualitySettings.names;
        int qualityLevel = QualitySettings.GetQualityLevel();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < names.Length; i++)
        {
            Dropdown.OptionData data = new Dropdown.OptionData(names[i]);
            options.Add(data);
        }
        m_dropdown.AddOptions(options);
        m_dropdown.value = qualityLevel;
    }

}
