using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Behaviours;
using Assets.Scripts.Classes;

[RequireComponent(typeof(Dropdown))]
public class StatisticsDropdownUI : MonoBehaviour, IStatisticsEventTarget
{
    [SerializeField]
    private GameObject m_scrollView;

    private Dropdown m_dropdown;
    private GlobalStatistics m_global;
    private ModeStatistics m_mode;

    private const string c_global = "Global Statistics";

    void Start()
    {
        m_dropdown = GetComponent<Dropdown>();
        m_dropdown.ClearOptions();
        m_global = new GlobalStatistics();
        m_mode = new ModeStatistics();

        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        //global options are always first (index 0)
        Dropdown.OptionData data = new Dropdown.OptionData(c_global);
        options.Add(data);

        for (int i = 0; i < PreferencesConfig.Preferences.Length; i++)
        {
            data = new Dropdown.OptionData(PreferencesConfig.Preferences[i].Name);
            options.Add(data);
        }
        m_dropdown.AddOptions(options);
        m_dropdown.onValueChanged.AddListener((x) => OnChange(x));
        m_dropdown.value = 0;
        OnChange(m_dropdown.value); //needed for some reason as event is not executed yet
    }

    public void OnChange(int value)
    {
        ExecuteEvents.Execute<IResultEventTarget>(m_scrollView, null, (x, y) => x.OnReset());

        Dictionary<string, string> results;
        Dropdown.OptionData option = m_dropdown.options[value];
        string name = option.text; //it is expected dropdown carries exact preferences name
        if (name == c_global)
        {
            //global stats
            m_global.Load();
            results = m_global.Report();
        }
        else
        {
            //mode stats
            m_mode.Load(name.ToLower());
            results = m_mode.Report();
        }
        ExecuteEvents.Execute<IResultEventTarget>(m_scrollView, null, (x, y) => x.OnAddResults(results));
    }

    public void OnShow()
    {
        m_dropdown.value = 0;
        OnChange(m_dropdown.value); //WHY! D:
    }
}
