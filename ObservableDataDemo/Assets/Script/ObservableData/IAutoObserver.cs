//自动处理监听对象和监听事件的接口
public interface IAutoObserver
{
    void AddObservableData(ObservableDataBase data);
    void RemoveObservableData();
}
