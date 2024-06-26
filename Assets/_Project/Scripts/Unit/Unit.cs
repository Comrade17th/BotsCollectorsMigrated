using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _holdPoint;
    
    private Resource _resource;
    private Transform _resourceTransform;
    private Base _base;
    private Transform _baseTransform;
    private Coroutine _coroutine;
    
    public WorkStatuses WorkStatus { get; private set; }

    private void Awake()
    {
        WorkStatus = WorkStatuses.Rest;
    }
    
    public void SetParentBase(Base basement)
    {
        _base = basement;
        _baseTransform = basement.GetComponent<Transform>();
    }
    
    public void GetResource(Resource resource)
    {
        _resource = resource;
        _resourceTransform = resource.GetComponent<Transform>();
        WorkStatus = WorkStatuses.GoResource;
        
        LaunchCoroutine(CollectingResource());
    }

    private IEnumerator CollectingResource()
    {
        yield return MovingTo(_resourceTransform);
        
        Grab(_resource);
        LaunchCoroutine(GoingBase());
    }
    
    private IEnumerator GoingBase()
    {
        yield return MovingTo(_baseTransform);
        Store();
    }

    private IEnumerator MovingTo(Transform target)
    {
        while (transform.position != target.position)
        {
            FollowTarget(target);
            yield return Time.deltaTime;
        }
    }

    private void LaunchCoroutine(IEnumerator routine)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        
        _coroutine = StartCoroutine(routine);
    }

    private void Store()
    {
        _base.Store(_resource);
        WorkStatus = WorkStatuses.Rest;
    }
    
    private void Grab(Resource resource)
    {
        resource.transform.parent = transform;
        resource.transform.position = _holdPoint.position;
        WorkStatus = WorkStatuses.GoBase;
    }

    private void FollowTarget(Transform target)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position, 
            _speed * Time.deltaTime);
    }
}
