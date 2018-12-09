# 使用
## 数据的定义
```cs
public class TestModel
{
    private static TestModel mInstance;
    public static TestModel GetInsatance()
    {
        if (mInstance == null) mInstance = new TestModel();
        return mInstance;
    }

    readonly public ObservableData<int> num = new ObservableData<int>();
}
```
直接定义在Model中即可，在定义时建议直接定义成readonly，防止出现ObservableData被重新赋值，因为监听都是绑定在ObservableData上的，ObservableData被重新赋值后之前的监听也会丢失。  
## 数据的修改
```cs
TestModel testModel = TestModel.GetInsatance();
testModel.count.Set(11);
```
对于ObservableData<T>，直接通过Set(T data),Get()函数进行数据的修改和获取，在修改的同时会发送修改事件出去。  
## 数据的监听与监听的移除
```cs
using UnityEngine;

public class TestObserver : MonoBehaviour
{
    void Awake()
    {
        TestModel testModel = TestModel.GetInstance();
        testModel.count.AddListener(TestListener,DataListenerType.UPDATE,this);
    }

    void TestListener(int num)
    {
        Debug.Log(num);
    }

    void OnDestroy()
    {
        testModel.count.RemoveListener(TestListener);
    }
}
```
**监听**
直接向对应的ObservableData添加监听、监听类型、holder即可（holder代表这个监听被哪个实例持有，便于监听的移除）。  
添加监听后，在修改ObservableData时，就会对对应类型的监听进行调用。 
**移除监听**
通过监听的委托或holder移除一个监听或所有监听。  
# 核心思路
1.使用泛型将类型封装，在修改数据的同时根据修改的类型通知事件出去。  
2.使用泛型将监听的回调封装，可以进行统一的监听和移除。  
3.通过辅助绑定类等辅助类增加ObservableData的易用性。  
4.在单数据的ObservableData还增加了List类型的Data，使用方法与单数据类似。  
# 其他
为了提高ObservableData的易用性，添加了AssistantBinder、ObserverView。  
**AssistantBinder:** 辅助绑定类，作为一个额外的holder，可对数据和监听进行统一的管理。  
**ObserverView:** 继承自MonoBehaviour，在被销毁时自动对监听进行释放。  
# 小结
整合和数据的存储和事件的通知，化简了观察者模式。  
1.易于扩展：
添加新类型时只用添加新的ObservableData和DataListener即可。  
2.易于修改：
比如在某些项目修改操作希望通知出不同的事件，可以直接修改对应的Set、Get函数。 

有什么设计不合理或者有bug的地方可以发送问题到我的邮箱[blueberryzzz525@gmail.com](blueberryzzz525@gmail.com)。  