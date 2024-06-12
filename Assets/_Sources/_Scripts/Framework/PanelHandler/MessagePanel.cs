
namespace Framework.Panel
{
    using UnityEngine;
    using TMPro;

    public class MessagePanel : Panel, IInit
    {
        public static MessagePanel Instance;
        [SerializeField] private TextMeshProUGUI messageText;

        public override void PostInit()
        {
            Instance = this;
        }

        public override void Setup(object obj)
        {
            messageText.text = (string)obj;
        }

        public void SetUp(string message, string title = "MESSAGE")
        {
            //messageText.font = Wugner.Localize.Localization.CurrentDefaultFont;
            messageText.text = message;
            titlePanel.text = title;
            Show();
            //GameController.Instance.canTap = false;
        }

        public void Init(object obj = null)
        {
            PostInit();
        }
    }

}