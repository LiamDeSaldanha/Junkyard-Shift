using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStatsHUD : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    public static PlayerStatsHUD Instance { get; private set; }

    private VisualElement _healthFill;
    private VisualElement _fuelFill;


    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;
        _healthFill = root.Q<VisualElement>("health-fill");
        _fuelFill = root.Q<VisualElement>("fuel-fill");
    }

    public void Start()
    {
        SetHealth(75f, 100f);
        SetFuel(66f, 100f);
    }


    public void SetHealth(float current, float max)
    {
        _healthFill.style.width = Length.Percent(current / max * 100f);
    }

    public void SetFuel(float current, float max)
    {
        _fuelFill.style.width = Length.Percent(current / max * 100f);
    }
}