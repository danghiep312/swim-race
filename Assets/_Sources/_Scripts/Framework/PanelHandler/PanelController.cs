
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Framework.DesignPattern.Singleton;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

namespace Framework.Panel
{
    using UnityEngine;

    public class PanelController : Singleton<PanelController>
    {
        [ShowInInspector] [ReadOnly] private static Stack<Panel> _panelStack;
        [ShowInInspector] [ReadOnly] private static List<Panel> _panels;
        [ShowInInspector] [ReadOnly] private static List<Panel> _releaseQueue;

        private void Start()
        {
            _panels = new List<Panel>();
            _panelStack = new Stack<Panel>();
            _releaseQueue = new List<Panel>();
        }

        public static async void ShowPanel(PanelType panelType, object data = null, bool overlay = false,
            Action<Panel> showCallback = null)
        {
            var panel = await GetPanel(panelType, data);
            if (showCallback != null) showCallback.Invoke(panel);
            else panel.Show();
        
            Debug.Log("Show: " + panel.gameObject.name);

            if (!overlay)
            {
                ReleasePanel(panel);
            }

            AddPanel(panel);
        }


        public static async UniTask<Panel> GetPanel(PanelType panelType, object data)
        {
            if (!TryGetPanel(panelType, out var panel))
                panel = await CreatePanel(panelType, Instance.transform);
            panel.SetActive(false);
            if (data != null) panel.Setup(data);
            return panel;
        }

        private static async UniTask<Panel> CreatePanel(PanelType panelType, Transform parent)
        {
            UniTask<GameObject> handle = Addressables.InstantiateAsync(panelType.ToString(), parent).Task.AsUniTask();
            Panel panel = (await handle).GetComponent<Panel>();
            panel.SetActive(false);
            panel.Init();
            return panel;
        }

        public static bool TryGetPanel(PanelType panelType, out Panel panel)
        {
            panel = _panels.Find(p => p.panelType == panelType);
            return panel != null;
        }

        [Button]
        private static void ReleasePanel(Panel exceptPanel)
        {
            while (_panelStack.Count > 0)
            {
                _panelStack.TryPop(out var panel);
                if (panel == null) continue;
                _releaseQueue.Add(panel);
                _panels.Remove(panel);
            }

            while (_releaseQueue.Count > 0)
            {
                if (_releaseQueue[0] != exceptPanel) Addressables.ReleaseInstance(_releaseQueue[0].gameObject);
                _releaseQueue.RemoveAt(0);
            }
            
            _panelStack.Push(exceptPanel);
        }

        private static void AddPanel(Panel panel)
        {
            if (!_panels.Contains(panel)) _panels.Add(panel);
            if (!_panelStack.Contains(panel)) _panelStack.Push(panel);
        }
        
        [Button]
        public void TestShow(PanelType panelType)
        {
            ShowPanel(panelType);
        }
    }
}