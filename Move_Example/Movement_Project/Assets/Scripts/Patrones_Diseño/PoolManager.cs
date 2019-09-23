using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region Singleton PoolManager
    private static PoolManager instance;
    public static PoolManager Instance {get => instance;}
    #endregion

    public List<GameObject> objPrefab;
    public string objName;

    Queue<GameObject> poolObj;
    public int amountObjs;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this);

        
        poolObj = new Queue<GameObject>();
        for (int i = 0; i < amountObjs; i++)
        {
            GameObject tempObj = Instantiate(objPrefab[0]);
            tempObj.SetActive(false);
            poolObj.Enqueue(tempObj);
        }
        
    }

    public void GetObj(string objName2, Vector3 newPosObj, Quaternion newRot)
    {
        //Mandar llamar este metodo cuando necesite (Balas, enemigos, efectos, etc)
        if(objName2 != objName)
        {
            Debug.LogError("Ese objeto no existe");
            return;
        }

        GameObject newObjTemp = poolObj.Dequeue();

        newObjTemp.transform.SetPositionAndRotation(newPosObj, newRot);
        newObjTemp.SetActive(true);
    }

    //Este metodo llamarlo en el prefab (Balas, enemigos, pasado un tiempo, etc.)
    public void BackPool(GameObject obj)
    {
        obj.SetActive(false);
        poolObj.Enqueue(obj);
    }
}
