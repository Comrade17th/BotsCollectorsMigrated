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

    // private void Update()
    // {
    //     // if (WorkStatus == WorkStatuses.GoResource)
    //     // {
    //     //     FollowTarget(_resourceTransform);
    //     //
    //     //     if (transform.position == _resourceTransform.position)
    //     //     {
    //     //         _resource.Grab(transform, _holdPoint);
    //     //         WorkStatus = WorkStatuses.GoBase;
    //     //     }
    //     // } else if (WorkStatus == WorkStatuses.GoBase)
    //     // {
    //     //     FollowTarget(_baseTransform);
    //     //     
    //     //     if (transform.position == _baseTransform.position)
    //     //     {
    //     //         _base.Store(_resource);
    //     //         WorkStatus = WorkStatuses.Rest;
    //     //     }
    //     // }
    // }
    
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
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        
        _coroutine = StartCoroutine(CollectingResource(_resource.transform));
    }

    private IEnumerator CollectingResource(Transform target)
    {
        while (transform.position != target.position)
        {
            FollowTarget(target);
            yield return Time.deltaTime;
        }
        
        _resource.Grab(transform, _holdPoint);
        WorkStatus = WorkStatuses.GoBase;
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        
        _coroutine = StartCoroutine(GoingBase(_baseTransform));
        yield break;
    }
    
    private IEnumerator GoingBase(Transform target)
    {
        while (transform.position != target.position)
        {
            FollowTarget(target);
            yield return Time.deltaTime;
        }
        
        _base.Store(_resource);
        WorkStatus = WorkStatuses.Rest;
        yield break;
    }

    private void FollowTarget(Transform target)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position, 
            _speed * Time.deltaTime);
    }
}
