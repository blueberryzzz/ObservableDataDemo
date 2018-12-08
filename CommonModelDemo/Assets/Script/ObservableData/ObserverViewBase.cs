using System.Collections.Generic;
using UnityEngine;

//可以自动释放ObserverData的MonoBehaviour
public class ObserverViewBase : MonoBehaviour,IAutoObserver
{
    List<ObservableDataBase> ObservableDataList = new List<ObservableDataBase>();

    public void AddObservableData(ObservableDataBase data)
    {
        if (!ObservableDataList.Contains(data))
        {
            ObservableDataList.Add(data);
        }
    }
    public void RemoveObservableData()
    {
        foreach(var observableData in ObservableDataList)
        {
            observableData.RemoveListenerByHolder(this);
        }
        ObservableDataList.Clear();
    }

    //销毁时自动释放
    protected virtual void OnDestroy()
    {
        RemoveObservableData();
    }
}
