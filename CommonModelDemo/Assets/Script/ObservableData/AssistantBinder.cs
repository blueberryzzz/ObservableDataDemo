using System;
using System.Collections.Generic;

//绑定信息结构
public class BindItem
{
    public ObservableDataBase ObservableDataInstance;
    public DataListener DataListenerInstance;

    public BindItem(ObservableDataBase observableData,DataListener DataListener)
    {
        ObservableDataInstance = observableData;
        DataListenerInstance = DataListener;
    }
}

//辅助绑定器
public class AssistantBinder
{
    public List<BindItem> bindItemList = new List<BindItem>();

    //将监听加到ObserableData中
    public void AddToData()
    {
        foreach (var bindItem in bindItemList)
        {
            bindItem.ObservableDataInstance.AddListener(bindItem.DataListenerInstance);
        }
    }

    //移除所有监听
    public void RemoveListener()
    {
        foreach (var bindItem in bindItemList)
        {
            bindItem.ObservableDataInstance.RemoveListener(bindItem.DataListenerInstance);
        }
    }

    //移除监听并清空绑定数据
    public void ClearData()
    {
        RemoveListener();
        bindItemList.Clear();
    }

    //单数据
    public void AddListener<T>(ObservableData<T> observableData, Action<T> listener, DataListenerType listenerType)
    {
        DataActionListener<T> DataListener = new DataActionListener<T>(listener, listenerType, this,false);
        bindItemList.Add(new BindItem(observableData, DataListener));
    }

    public void AddBind<T>(ObservableData<T> observableData, Action<T> listener, DataListenerType listenerType)
    {
        DataActionListener<T> DataListener = new DataActionListener<T>(listener, listenerType, this,true);
        bindItemList.Add(new BindItem(observableData, DataListener));
    }

    //列表类型
    public void AddListListener<T>(ObservableListData<T> observableData, ListCallback<T> listener, DataListenerType listenerType,bool fullUpdateFirst = false)
    {
        DataListListener<T> DataListener = new DataListListener<T>(listener, listenerType, this, fullUpdateFirst);
        bindItemList.Add(new BindItem(observableData, DataListener));
    }
    //列表插入类型
    public void AddListInsertListener<T>(ObservableListData<T> observableData, InsertCallback<T> listener, DataListenerType listenerType, bool fullUpdateFirst = false)
    {
        DataInsertListener<T> DataListener = new DataInsertListener<T>(listener, listenerType, this, fullUpdateFirst);
        bindItemList.Add(new BindItem(observableData, DataListener));
    }
}
