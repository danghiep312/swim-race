using Framework.DesignPattern.Observer;

namespace Framework.Panel
{
    using System;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingTransition : Panel, IInit
    {
        public static LoadingTransition Instance;

        [Title("Loading Transition")] [SerializeField]
        Transform loadingIcon;

        public float elapsedTime;
        private bool _isShowing;
        private bool _isHiding;

        public bool firstTime;

        private CanvasGroup _group;

        private void Awake()
        {
            PostInit();
        }

        public override void PostInit()
        {
            if (Instance == null) Instance = this;
            //firstTime = true;
            _group = GetComponent<CanvasGroup>();
        }

        public override void Setup(object o)
        {
        }

        private async void Start()
        {
            _group.alpha = 1f;
            if (firstTime)
            {
                await UniTask.Delay(1000);
                firstTime = false;
                Hide();
            }
        }

        private void OnEnable()
        {
            // GetComponent<Image>().sprite = SpriteStorage.GetHomeBackground(MemoryAccess.GetInformation(MemoryKey.Theme, 0).ParseEnum<ThemeType>());
            // Debug.Log(SpriteStorage.GetCurrentBackground().name);
        }

        public void ShowImmediately()
        {
            base.Show();
            loadingIcon.DORotate(Vector3.forward * -500f, 3f, RotateMode.WorldAxisAdd)
                .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);

            _group.alpha = 1f;
        }

        public void Show(Action onAfterLoading = null)
        {
            //if (firstTime) return;
            base.Show();
            //_group.alpha = 0f;
            _group.DOFade(1f, elapsedTime).SetEase(Ease.Linear).OnComplete(() => { onAfterLoading?.Invoke(); });
            loadingIcon.DORotate(Vector3.forward * -360f, 3f, RotateMode.WorldAxisAdd)
                .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }

        public static void Active(Action onAfterLoading = null)
        {
            Instance.Show(onAfterLoading);
        }

        public static void Deactive()
        {
            Instance.Hide();
        }

        [Button("End Loading")]
        public void Hide()
        {
            _group.DOKill();
            _group.alpha = 1f;
            _group.DOFade(0f, elapsedTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                gameObject.SetActive(false);
                this.PostEvent(EventID.CloseLoading);
            });
        }


        private void OnDestroy()
        {
            DOTween.KillAll();
        }

        public void Init(object obj = null)
        {
            PostInit();
        }
    }
}