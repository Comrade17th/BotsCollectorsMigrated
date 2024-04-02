using UnityEngine;

public class Resource : MonoBehaviour
{
    private Transform _container;
    
    public void Grab(Transform parent, Transform holdPoint)
    {
        transform.parent = parent;
        transform.position = holdPoint.position;
    }

    public void Store()
    {
        transform.parent = _container;
        gameObject.SetActive(false);
    }
}
