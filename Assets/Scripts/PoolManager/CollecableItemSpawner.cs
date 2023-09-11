using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watermelon;

public class CollecableItemSpawner : MonoBehaviour
{
    [SerializeField] float maxBoundX = 45f;
    [SerializeField] float maxBoundZ = 45f;
    [SerializeField] float maxBoundY = 20f;
    private Coroutine _coroutine;
    private Pool _pool;
    private void Start()
    {
    }
    public void StartSpawn()
    {
        _pool = PoolManager.GetPoolByName(PoolNameConst.COLLECTABLE_WEAPON);
        if (_coroutine != null) _coroutine = null;
        _coroutine = StartCoroutine(ISpawnCollecableItem());
    }
    public void StopSpawn()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
    private IEnumerator ISpawnCollecableItem(int amount = 1)
    {
        while (true)
        {
            for (int i = 0; i < amount; i++)
            {
                var randomPos = new Vector3(Random.Range(-maxBoundX, maxBoundX), maxBoundY, Random.Range(-maxBoundZ, maxBoundZ));
                var collectableWeapon = _pool.GetPooledObject(true);
                collectableWeapon.transform.position = randomPos;
            }
            yield return new WaitForSeconds(3f);
        }
    }
}
