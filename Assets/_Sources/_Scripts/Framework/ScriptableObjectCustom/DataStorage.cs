using Framework.DesignPattern.Singleton;

namespace Framework.DataObject
{
    public class DataStorage : Singleton<DataStorage>
    {
        public ScriptableObjectInstance[] dataObjects;

        public override void Awake()
        {
            base.Awake();
            foreach (var obj in dataObjects)
            {
                obj.Init();
            }
        }

    }
}

