using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Base : MonoBehaviour
{
    [SerializeField] private Sonar _sonar;
    [SerializeField] private List<Unit> _units;
    
    [SerializeField] private float _orderDelay = 0.5f;
    
    private HashSet<Resource> _resources;
    private int _storedResources = 0;
    private int _valuePerResource = 1;

    private Coroutine _coroutine;
    private WaitForSeconds _waitOrder;

    public event Action<int> StoredResourcesChanged;

    private void Awake()
    {
        _resources = new HashSet<Resource>();
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
    
    public void Store(Resource resource)
    {
        Add(resource.Value);
        resource.transform.parent = transform;
        resource.gameObject.SetActive(false);
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
                Resource resource = _resources.ElementAt(0);
                unit.GetResource(resource);
                _resources.Remove(resource);
            }
        }
    }

    private void WriteResource(Resource resource)
    {
        _resources.Add(resource);
    }

    private IEnumerator OrderingResources()
    {
        while (true)
        {
            OrderResource();
            yield return _waitOrder;
        }
    }
}
