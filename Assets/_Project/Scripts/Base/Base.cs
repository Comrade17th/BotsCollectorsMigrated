using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Base : MonoBehaviour
{
    [SerializeField] private Sonar _sonar;
    [SerializeField] private float _orderDelay = 0.5f;

    [SerializeField] private List<Unit> _units;
    private List<Resource> _resources = new List<Resource>();

    private int _storedResources = 0;
    private int _valuePerResource = 1;

    private Coroutine _coroutine;
    private WaitForSeconds _waitOrder;

    public event Action<int> StoredResourcesChanged;

    private void Awake()
    {
        _waitOrder = new WaitForSeconds(_orderDelay);
        Assert.IsNotNull(_sonar);
    }

    private void OnEnable()
    {
        _sonar.ResourceFinded += WriteResource;
    }

    private void OnDisable()
    {
        _sonar.ResourceFinded -= WriteResource;
    }

    private void Start()
    {
        foreach (Unit unit in _units)
            InitUnit(unit);

        _coroutine = StartCoroutine(OrderingResources());
    }
    
    private bool TryGetRestUnit(out Unit result)
    {
        foreach (Unit unit in _units)
        {
            if (unit.WorkStatus == WorkStatuses.Rest)
            {
                result = unit;
                return true;
            }
        }

        result = null;
        return false;
    }
    
    private void Add(int value)
    {
        if(value <= 0)
            return;

        _storedResources += value;
        StoredResourcesChanged?.Invoke(_storedResources);
    }

    private void InitUnit(Unit unit)
    {
        unit.SetParentBase(transform.GetComponent<Base>());
    }

    private void OrderResource()
    {
        if (_resources.Count > 0)
        {
            if (TryGetRestUnit(out Unit unit))
            {
                unit.GetResource(_resources[0]);
                _resources.Remove(_resources[0]);
            }
        }
    }

    private void WriteResource(Resource resource)
    {
        if (_resources.Contains(resource) == false)
        {
            _resources.Add(resource);
        }
    }

    private IEnumerator OrderingResources()
    {
        while (true)
        {
            OrderResource();
            yield return _waitOrder;
        }
    }

    public void Store(Resource resource)
    {
        Add(_valuePerResource);
        resource.Store();
    }
}
