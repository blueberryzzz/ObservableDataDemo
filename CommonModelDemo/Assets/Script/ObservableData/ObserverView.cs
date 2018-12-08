//在ObserverViewBase的基础上添加了协助绑定类，与MonoBehaviour的生命周期相关
public class ObserverView : ObserverViewBase
{
    //绑定周期为Start-OnDestroy的协助绑定器
    public AssistantBinder AliveBinder;
    //绑定周期为OnEnable-OnDisable的协助绑定器
    public AssistantBinder ActiveBinder;

    protected virtual void Awake()
    {
        AliveBinder = new AssistantBinder();
        ActiveBinder = new AssistantBinder();
    }
    
    protected virtual void Start()
    {
        AliveBinder.AddToData();
    }

    protected virtual void OnEnable()
    {
        ActiveBinder.AddToData();
    }

    protected virtual void OnDisable()
    {
        ActiveBinder.RemoveListener();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        AliveBinder.ClearData();
        AliveBinder.ClearData();
    }
}
