using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestObserver : ObserverView {

    protected override void Awake()
    {
        base.Awake();
        TestModel testModel = TestModel.GetInsatance();
        AliveBinder.AddListener(testModel.count, AliveBinder_AddListener,DataListenerType.UPDATE);
        ActiveBinder.AddListener(testModel.count, ActiveBinder_AddListener, DataListenerType.UPDATE);
        ActiveBinder.AddBind(testModel.count, ActiveBinder_AddBind, DataListenerType.UPDATE);

        testModel.testInt.AddListener(TestInt_InitAndUpdate, DataListenerType.INIT_AND_UPDATE,this);
        testModel.testInt.AddListener(TestInt_Update, DataListenerType.UPDATE,this);
        testModel.testInt.AddListener(TestInt_Clear, DataListenerType.CLEAR, this);

        testModel.intList.AddListener(IntList_InitAndUpdate, DataListenerType.INIT_AND_UPDATE, this);
        testModel.intList.AddListener(IntList_Add, DataListenerType.ADD, this);
        testModel.intList.AddInsertListener(IntList_Insert, this);
        testModel.intList.AddListener(IntList_Clear, DataListenerType.CLEAR, this);
    }
    
    public void AliveBinder_AddListener(int num)
    {
        Debug.Log("AliveBinder.AddListener: " + num);
    }

    public void ActiveBinder_AddListener(int num)
    {
        Debug.Log("ActiveBinder.AddListener: " + num);
    }

    public void ActiveBinder_AddBind(int num)
    {
        Debug.Log("ActiveBinder.AddBind: " + num);
    }

    public void TestInt_InitAndUpdate(TestInt data)
    {
        Debug.Log("testInt INIT_AND_UPDATE: " + data.num);
    }

    public void TestInt_Update(TestInt data)
    {
        Debug.Log("testInt UPDATE: " + data.num);
    }

    public void TestInt_Clear(TestInt data)
    {
        Debug.Log("testInt CLEAR: " + data.num);
    }

    public string ListToString(List<int> list)
    {
        string result = "";
        bool isFirst = true;
        foreach(var num in list)
        {
            if (isFirst)
            {
                isFirst = false;
                result += num;
            }
            else
            {
                result += ", " + num;
            }            
        }
        return result;
    }

    public void IntList_InitAndUpdate(List<int> list)
    {
        Debug.Log("IntList_InitAndUpdate : " + ListToString(list));
    }

    public void IntList_Add(List<int> list)
    {
        Debug.Log("IntList_Add : " + ListToString(list));
    }

    public void IntList_Insert(List<int> list,int index)
    {
        Debug.Log("IntList_Insert : " + ListToString(list) + "; index : " + index);
    }

    public void IntList_Clear(List<int> list)
    {
        Debug.Log("IntList_Clear : " + ListToString(list));
    }
}
