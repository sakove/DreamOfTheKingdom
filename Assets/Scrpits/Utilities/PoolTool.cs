using UnityEngine;
using UnityEngine.Pool;

//代码执行优先级
[DefaultExecutionOrder(-100)]
public class PoolTool:MonoBehaviour
{
    public GameObject objprefab;
    
    private ObjectPool<GameObject> objPool;

    private void Awake()
    {
        objPool = new ObjectPool<GameObject>(
            () => Instantiate(objprefab),
            (obj) => obj.SetActive(true),
            (obj) => obj.SetActive(false),
            (obj) => Destroy(obj),
            false,
            20,
            50
        );
        
        PreFillPool(7);
    }

    private void PreFillPool(int count)
    {
        var preFillArray=new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            preFillArray[i]=objPool.Get();
        }

        foreach (var VARIABLE in  preFillArray)
        {
            objPool.Release(VARIABLE);
        }
    }

    public GameObject GetObjectFromPool()
    {
        return objPool.Get();
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        objPool.Release(obj);
    }
}
