using Framework.DesignPattern.Observer;

namespace Framework.Panel
{
    using System.Linq;
    using System.Threading;
    using DG.Tweening;
    using GeneralResources;
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;

#if UNITY_EDITOR
    using Unity.VisualScripting;
    using UnityEditor;
    using UnityEditor.AddressableAssets;
    using UnityEditor.AddressableAssets.Settings;
#endif


    public abstract class Panel : MonoBehaviour, IBack
    {
        //[HideInInspector] public RectTransform rect;

        public PanelType panelType;
        public Animator ani;
        [Title("Title")] public TextMeshProUGUI titlePanel;

        [Title("Delay Active")] public Transform[] delayActiveObjects;
        public bool sequence;
        public float showDuration = .5f;

        [PropertySpace(SpaceAfter = 20, SpaceBefore = 0)]
        public float delayTime = 1.5f;

        [Title("Other")] [ReadOnly, HideLabel] public GameObject dummyToMakeTitle;
        private CancellationTokenSource _cts;

        public void Init()
        {
            PostInit();
        }

        public virtual void Show(string title = "", bool overrideBack = true, bool activeImmediately = false)
        {

            if (!title.Equals(""))
            {
                if (titlePanel) titlePanel.text = title;
            }

            BackButtonController.Instance.Register(this, overrideBack);


            if (!activeImmediately)
            {
                _cts = new CancellationTokenSource();
                Util.Delay(delayTime, () =>
                {
                    foreach (var delayActiveObj in delayActiveObjects)
                    {
                        DelayActive(delayActiveObj,
                            sequence ? delayActiveObjects.ToList().IndexOf(delayActiveObj) * delayTime : 0);
                    }
                }, _cts);
            }

            // Cancel 1 time delay active
            if (ani)
            {
                ani.SetTrigger("Open");
            }

            gameObject.SetActive(true);
        }

        public void CancelDelayActive()
        {
            _cts?.Cancel();
            foreach (var delayActiveObj in delayActiveObjects)
            {
                delayActiveObj.gameObject.SetActive(true);
            }
        }
        
        public void ContinueDelayActive()
        {
            foreach (var delayActiveObj in delayActiveObjects)
            {
                DelayActive(delayActiveObj,
                    sequence ? delayActiveObjects.ToList().IndexOf(delayActiveObj) * delayTime : 0);
            }
        }

        public virtual void ShowAfterAd()
        {
            // GameController.ShowAd(AdType.Interstitial, status => Show());
        }

        public virtual void CloseAfterAd()
        {
            // GameController.ShowAd(AdType.Interstitial, status => Close());
        }

        public virtual void Close()
        {
            if (!gameObject.activeSelf) return;
            _cts?.Cancel();
            // BackButtonController.Instance.UnRegister(this);


            var popupEffect = GetComponentInChildren<Framework.Effect.Popup.PopupEffect>();
            if (popupEffect)
            {
                popupEffect.onDisappearComplete.RemoveListener(PostEventClose);
                popupEffect.onDisappearComplete.AddListener(PostEventClose);
                popupEffect.Disappear();
                return;
            }

            if (ani)
            {
                ani.SetTrigger("Close");
                return;
            }

            gameObject.SetActive(false);
        }

        private void PostEventClose()
        {
            this.PostEvent(EventID.ClosePanel, panelType);
        }

        private void DelayActive(Transform obj, float delay)
        {
            obj.localScale = Vector3.zero;
            obj.DOScale(Vector3.one, showDuration).SetEase(Ease.OutBack).SetDelay(delay).OnStart(() =>
            {
                obj.gameObject.SetActive(true);
            });
        }

        public abstract void PostInit();

        // public virtual void Setup(object o) {}

        public abstract void Setup(object o);

        public virtual void OnBack(bool adBreak = false)
        {
            if (adBreak)
            {
                CloseAfterAd();
            }
            else
            {
                Close();
            }
        }


#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (ani == null)
            {
                ani = GetComponentInChildren<Animator>();
            }

            if (panelType == PanelType.None) return;
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            string assetPath = AssetDatabase.GetAssetPath(gameObject);
            //string directoryPath = System.IO.Path.GetDirectoryName(assetPath);
            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            AddressableAssetEntry entry = settings.FindAssetEntry(guid);
            if (entry != null)
            {
                if (panelType.ToString().Equals(entry.address)) return;
                // Change the address of the asset
                if (panelType != PanelType.None)
                {
                    entry.address = panelType.ToString();
                }
            }
            else return;

            Debug.Log(assetPath + " " + gameObject.name, gameObject);
            if (!string.IsNullOrEmpty(assetPath))
            {
                AssetDatabase.RenameAsset(assetPath, panelType.ToString() + ".prefab");
                AssetDatabase.SaveAssets();
            }

        }
#endif
    }

}