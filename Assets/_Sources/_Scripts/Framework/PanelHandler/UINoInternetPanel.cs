namespace Framework.Panel
{
   
    public class UINoInternetPanel : Panel
    {
        public static UINoInternetPanel Instance;

        public override void PostInit()
        {
            Instance = this;
        }

        public override void Setup(object o)
        {
            Show();
        }

    }

}