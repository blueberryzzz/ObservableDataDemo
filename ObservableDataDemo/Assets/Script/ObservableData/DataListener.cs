using System;
using System.Collections.Generic;

//通知事件的类型
[Flags]
public enum DataListenerType
{
    INIT             = 2 << 0,
    UPDATE           = 2 << 1,
    ADD              = 2 << 2,
    REMOVE           = 2 << 3,
    CLEAR            = 2 << 4,

    INSERT           = 2 << 5,

    INIT_AND_UPDATE  = INIT | UPDATE,
}

/// <summary>
/// 数据监听事件的基类
/// </summary>
public abstract class DataListener
{
    //添加该监听事件时，是否需要根据已有的数据调用一次监听事件
    public bool FullUpdateFirst;
    //事件类型
    public DataListenerType ListenerType;

    public object Holder;

    public DataListener(DataListenerType listenerType, object holder = null, bool fullupdateFirst = false)
    {
        ListenerType = listenerType;
        Holder = holder;
        FullUpdateFirst = fullupdateFirst;
    }

    //根据事件类型进行通知
    public abstract void FireEventForListener(object data);
}

/// <summary>
/// 单数据修改监听事件类
/// </summary>
public class DataActionListener<T> : DataListener
{
    public Action<T> ListenerFunc;

    public DataActionListener(Action<T> listener, DataListenerType listenerType,object holder = null,bool fullupdateFirst = false):base(listenerType,holder,fullupdateFirst)
    {
        ListenerFunc = listener;
    }

    //通知监听事件
    public override void FireEventForListener(object data)
    {
        if (data != null && ListenerFunc != null)
        {
            ListenerFunc((T)data);
        }
    }
}

//列表数据修改监听
public delegate void ListCallback<T>(List<T> dataList);
public class DataListListener<T> : DataListener
{
    
    public ListCallback<T> ListenerFunc;

    public DataListListener(ListCallback<T> listener, DataListenerType listenerType, object holder = null, bool fullupdateFirst = false) : base(listenerType, holder, fullupdateFirst)
    {
        ListenerFunc = listener;
    }

    //通知监听事件
    public override void FireEventForListener(object data)
    {
        if (data != null && ListenerFunc != null)
        {
            ListenerFunc((List<T>)data);
        }
    }
}

//数据插入监听
public delegate void InsertCallback<T>(List<T> dataList, int index);
public class ListInsertStruct<T>
{
    public List<T> DataList;
    public int Index;

    public ListInsertStruct(List<T> dataList,int index)
    {
        DataList = dataList;
        Index = index;
    }
}
public class DataInsertListener<T> : DataListener
{

    public InsertCallback<T> ListenerFunc;

    public DataInsertListener(InsertCallback<T> listener, DataListenerType listenerType, object holder = null, bool fullupdateFirst = false) : base(listenerType, holder, fullupdateFirst)
    {
        ListenerFunc = listener;
    }

    //通知监听事件
    public override void FireEventForListener(object data)
    {
        ListInsertStruct<T> insertStruct = data as ListInsertStruct<T>;
        if (insertStruct != null && insertStruct.DataList != null && ListenerFunc != null)
        {
            ListenerFunc(insertStruct.DataList,insertStruct.Index);
        }
    }
}