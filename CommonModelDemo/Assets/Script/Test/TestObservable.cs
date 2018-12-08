using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TestObservable : MonoBehaviour {

    public GameObject testObserver;

	// Use this for initialization
	void Start () {
        StartCoroutine(ChangeData());
	}
	
    IEnumerator ChangeData()
    {
        yield return new WaitForEndOfFrame();
        TestModel testModel = TestModel.GetInsatance();
        //值类型
        Debug.Log("<color=#00ffffff>-------------值类型-------------</color>");
        testModel.count.Set(11);
        yield return new WaitForSeconds(1);
        //引用类型
        Debug.Log("<color=#00ffffff>-------------引用类型-------------</color>");
        TestInt a = new TestInt();
        a.num = 21;
        testModel.testInt.Set(a);
        a.num = 22;
        testModel.testInt.Set(a);
        testModel.testInt.Set(null);
        a.num = 23;
        testModel.testInt.Set(a);
        yield return new WaitForSeconds(1);
        //协助绑定
        Debug.Log("<color=#00ffffff>-------------协助绑定类-------------</color>");
        Debug.Log("-------------OnDisable-----------------");
        testObserver.SetActive(false);
        testModel.count.Set(12);
        testModel.count.Set(13);
        Debug.Log("-------------OnEnable-----------------");
        testObserver.SetActive(true);
        testModel.count.Set(14);
        yield return new WaitForSeconds(1);
        //列表类型
        Debug.Log("<color=#00ffffff>-------------列表类型-------------</color>");
        testModel.intList.Set(new List<int> { 1, 2, 3 });
        testModel.intList.Set(new List<int> { 2, 3, 4 });
        testModel.intList.AddList(new List<int> { 5, 6 });
        testModel.intList.Insert(2, 999);
        testModel.intList.Set(null);
        //销毁ObserverView，在OnDestroy时自动解绑定
        GameObject.Destroy(testObserver);
        yield return new WaitForSeconds(1);
        Debug.Log("<color=#00ffffff>-------------ObserverView销毁后自动解绑定-------------</color>");
        testModel.count.Set(14);
        a.num = 24;
        testModel.testInt.Set(a);
        testModel.intList.Set(new List<int> { 1, 2 });
    }
}
