using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// 观察对象数据基类
/// </summary>
public class ObservableDataBase
{
    //监听事件列表
    protected List<DataListener> mListenerList = new List<DataListener>();
    
    //通知监听事件数据的修改
    protected void FireEventImpl(object data, DataListenerType eventType)
    {
        if (data == null || mListenerList == null) return;
        foreach (var listener in mListenerList)
        {
            if ((eventType & listener.ListenerType) == eventType)
            {
                listener.FireEventForListener(data);
            }
        }
    }

    //添加监听事件
    public virtual void AddListener(DataListener listener)
    {
        mListenerList.Add(listener);
        //如果holder是IAutoObservable类型，则调用AddObservableData接口将该data记录
        if(listener.Holder != null && listener.Holder is IAutoObserver)
        {
            (listener.Holder as IAutoObserver).AddObservableData(this);
        }
    }

    //移除监听事件
    public virtual void RemoveListener(DataListener listener)
    {
        mListenerList.Remove(listener);
    }
    //通过holder移除监听对象
    public void RemoveListenerByHolder(object holder)
    {
        for (int i = mListenerList.Count - 1; i >= 0; i--)
        {
            if ((mListenerList[i].Holder == holder))
            {
                mListenerList.RemoveAt(i);
            }
        }
    }
}

/// <summary>
/// 单数据类型
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObservableData<T> : ObservableDataBase
{
    //数据
    private T mData;

    public ObservableData(T data = default(T))
    {
        mData = data;
    }

    //赋值操作
    public void Set(T data)
    {
        var oldData = mData;
        //值类型总是有值的，所以只发送更新事件
        if (typeof(T).IsValueType)
        {
            mData = data;
            FireEventImpl(data, DataListenerType.UPDATE);
        }
        //引用类型
        else
        {
            //将非空的值置为空值视为清除操作
            if (data == null)
            {
                mData = default(T);
                if (oldData != null)
                {
                    FireEventImpl(oldData, DataListenerType.CLEAR);
                }
            }
            else
            {
                //将空值置为非空值视为初始化操作
                mData = data;
                if (oldData == null)
                {
                    FireEventImpl(data, DataListenerType.INIT);
                }
                //修改非空值视为更新操作
                else
                {
                    FireEventImpl(data, DataListenerType.UPDATE);
                }
            }
        }
    }
    
    //取值操作
    public T Get()
    {
        return mData;
    }

    //添加监听
    public void AddListener(Action<T> listener,DataListenerType listenerType,object holder = null,bool fullUpdate = false)
    {
        AddListener(new DataActionListener<T>(listener, listenerType,holder,fullUpdate));
    }

    //添加监听
    public override void AddListener(DataListener listener)
    {
        base.AddListener(listener);
        if(mData != null && listener.FullUpdateFirst)
        {
            listener.FireEventForListener(mData);
        }
    }

    //根据委托进行监听的移除
    public void RemoveListener(Action<T> listener)
    {
        for(int i = 0; i < mListenerList.Count; i++)
        {
            if((mListenerList[i] as DataActionListener<T>).ListenerFunc == listener)
            {
                mListenerList.RemoveAt(i);
                break;
            }
        }
    }
}

/// <summary>
/// 列表数据类型
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObservableListData<T> : ObservableDataBase
{
    private List<T> mDataList;

    public ObservableListData(List<T> dataList = null)
    {
        mDataList = dataList;
    }

    //赋值操作
    public void Set(List<T> dataList)
    {
        var oldDataList = mDataList;
        //将非空值赋值为null或空表时视为清除操作
        if (dataList == null || dataList.Count == 0)
        {
            mDataList = dataList;
            if (oldDataList != null && oldDataList.Count != 0)
            {
                FireEventImpl(oldDataList, DataListenerType.CLEAR);
            }
        }
        else
        {           
            mDataList = dataList;
            //将空值赋值为非空值时视为初始化操作
            if (oldDataList == null || oldDataList.Count == 0)
            {
                FireEventImpl(mDataList, DataListenerType.INIT);
            }
            //修改非空值视为更新操作
            else
            {
                FireEventImpl(mDataList, DataListenerType.UPDATE);
            }
        }
    }

    //取值操作
    public List<T> Get()
    {
        return mDataList;
    }

    //批量添加操作
    public List<T> AddList(List<T> addedList)
    {
        if (mDataList == null || mDataList.Count == 0)
        {
            mDataList = addedList;
            FireEventImpl(mDataList, DataListenerType.INIT);
            FireEventImpl(addedList, DataListenerType.ADD);
        }
        else
        {
            mDataList.AddRange(addedList);
            FireEventImpl(mDataList, DataListenerType.UPDATE);
            FireEventImpl(addedList, DataListenerType.ADD);
        }
        return mDataList;
    }

    //插入操作
    public List<T> Insert(int index, T data)
    {
        if(mDataList != null)
        {
            if(mDataList.Count == 0)
            {
                mDataList.Insert(index,data);
                FireEventImpl(mDataList, DataListenerType.INIT);
                FireEventImpl(new ListInsertStruct<T>(mDataList, index), DataListenerType.INSERT);
            }
            else
            {
                mDataList.Insert(index, data);
                FireEventImpl(mDataList, DataListenerType.UPDATE);
                FireEventImpl(new ListInsertStruct<T>(mDataList, index), DataListenerType.INSERT);
            }
            return mDataList;
        }
        else
        {
            Debug.LogError("this list has not been initialize!!!");
            return null;
        }
    }

    //添加普通列表监听
    public void AddListener(ListCallback<T> listener, DataListenerType listenerType, object holder = null, bool fullUpdate = false)
    {
        AddListener(new DataListListener<T>(listener, listenerType, holder,fullUpdate));
    }

    //添加插入列表监听
    public void AddInsertListener(InsertCallback<T> listener,object holder = null)
    {
        AddListener(new DataInsertListener<T>(listener, DataListenerType.INSERT, holder));
    }

    //添加监听
    public override void AddListener(DataListener listener)
    {
        base.AddListener(listener);
        if (mDataList != null && mDataList.Count > 0 && listener.FullUpdateFirst)
        {
            listener.FireEventForListener(mDataList);
        }
    }

    //通过委托进行监听的移除
    public void removeListener(ListCallback<T> listener)
    {
        for (int i = 0; i < mListenerList.Count; i++)
        {
            if ((mListenerList[i] as DataListListener<T>).ListenerFunc == listener)
            {
                mListenerList.RemoveAt(i);
                break;
            }
        }
    }

    //通过委托进行监听的移除
    public void removeListener(InsertCallback<T> listener)
    {
        for (int i = 0; i < mListenerList.Count; i++)
        {
            if ((mListenerList[i] as DataInsertListener<T>).ListenerFunc == listener)
            {
                mListenerList.RemoveAt(i);
                break;
            }
        }
    }
}
