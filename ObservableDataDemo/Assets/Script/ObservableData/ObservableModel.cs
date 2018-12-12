using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ObservableModel
{
    List<ObservableDataBase> ObservableDataList;

    public void AddDataToModel(ObservableDataBase observableData)
    {
        if(ObservableDataList == null)
        {
            ObservableDataList = new List<ObservableDataBase>();
        }
        ObservableDataList.Add(observableData);
    }

    public void RemoveListener(object holder)
    {
        if(ObservableDataList != null){
            foreach(var observableData in ObservableDataList)
            {
                observableData.RemoveListenerByHolder(holder);
            }
        }
    }
}
