using System.Collections;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private float _delay;
    
    [SerializeField] private float _spawnPositionY;
    [SerializeField] private float _maxBoundX;
    [SerializeField] private float _maxBoundY;

    [SerializeField] private int _maxInstanceOnScene;
    
    private Pool<Resource> _pool;
    private WaitForSeconds _waitSpawn;

    private void Awake()
    {
        _pool = new Pool<Resource>(_prefab, transform, transform,3);
        _waitSpawn = new WaitForSeconds(_delay);
    }

    private void Start()
    {
        StartCoroutine(Spawning());
    }

    public void Reset()
    {
        _pool.Reset();
    }

    private IEnumerator Spawning()
    {
        while (enabled)
        {
            Spawn();
            yield return _waitSpawn;
        }
    }

    private void Spawn()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(-_maxBoundX, _maxBoundX),
            _spawnPositionY,
            Random.Range(-_maxBoundY, _maxBoundY));
      
        Resource resource = _pool.Peek();
        resource.transform.position = spawnPosition;
    }
}
