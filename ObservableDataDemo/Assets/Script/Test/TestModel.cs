public class TestInt
{
    public int num;
}

class TestModel
{
    private static TestModel mInstance;
    public static TestModel GetInsatance()
    {
        if (mInstance == null) mInstance = new TestModel();
        return mInstance;
    }

    private TestModel()
    {
        count = new ObservableData<int>();
        testInt = new ObservableData<TestInt>();
        intList = new ObservableListData<int>();
    }

    readonly public ObservableData<int> count;
    readonly public ObservableData<TestInt> testInt;
    readonly public ObservableListData<int> intList;
}
