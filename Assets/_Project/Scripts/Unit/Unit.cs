using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public WorkStatuses WorkStatus { get; private set; }

    [SerializeField] private float _speed;
    [SerializeField] private Transform _holdPoint;
    
    [SerializeField] private Resource _resource;
    private Transform _resourceTransform;
    private Base _base;
    private Transform _baseTransform;

    private void Awake()
    {
        WorkStatus = WorkStatuses.Rest;
    }

    private void Update()
    {
        if (WorkStatus == WorkStatuses.GoResource)
        {
            FollowTarget(_resourceTransform);

            if (transform.position == _resourceTransform.position)
            {
                _resource.Grab(transform, _holdPoint);
                WorkStatus = WorkStatuses.GoBase;
            }
        } else if (WorkStatus == WorkStatuses.GoBase)
        {
            FollowTarget(_baseTransform);
            
            if (transform.position == _baseTransform.position)
            {
                _base.Store(_resource);
                WorkStatus = WorkStatuses.Rest;
            }
        }
    }

    private void FollowTarget(Transform target)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position, 
            _speed * Time.deltaTime);
    }

    public void GetResource(Resource resource)
    {
        _resource = resource;
        _resourceTransform = resource.GetComponent<Transform>();
        WorkStatus = WorkStatuses.GoResource;
    }

    public void SetParentBase(Base basement)
    {
        _base = basement;
        _baseTransform = basement.GetComponent<Transform>();
    }
}
