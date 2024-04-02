using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class StoreView : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TextMeshProUGUI _textMeshPro;

    private void Awake()
    {
        Assert.IsNotNull(_base);
        Assert.IsNotNull(_textMeshPro);
    }

    private void OnEnable()
    {
        _base.StoredResourcesChanged += UpdateNumber;
    }

    private void OnDisable()
    {
        _base.StoredResourcesChanged -= UpdateNumber;
    }

    private void UpdateNumber(int value)
    {
        _textMeshPro.text = $"{value}";
    }
}
