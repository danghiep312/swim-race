using Framework.DesignPattern.Singleton;

namespace Framework.Panel
{
    using System.Collections.Generic;
    using UnityEngine;
    
    public class PanelManager : Singleton<PanelManager>
    {
        [SerializeField]
        private List<Panel> panels = new List<Panel>();
        // Start is called before the first frame update
        void Start()
        {
            Init();
        }
    
        public void Init()
        {
            Transform holder = transform;
            for(int i = 0; i < panels.Count; i++)
            {
                panels[i].Init();
            }
            for(int i = 0; i < holder.childCount; i++)
            {
                if (!holder.GetChild(i).TryGetComponent(out Panel panel)) continue;
                panels.Add(panel);
                try
                {
                    panel.Init();
                }
                catch (System.Exception e){ Debug.LogError(e); }
            }
        }
        public Panel GetPanel(int index)
        {
            return panels[index];
        }
    
        public Panel GetPanel(string panelName)
        {
            foreach (Panel panel in panels)
            {
                if (panel.name.Contains(panelName)) return panel;
            }
            Common.LogWarning(this, $"Not exist panel {panelName}");
            return null;
        }
    
        public Panel GetPanel(PanelType panelType)
        {
            foreach (Panel panel in panels)
            {
                if (panel.panelType.Equals(panelType)) return panel;
            }
            Common.LogWarning(this, $"Not exist panel {panelType}");
            return null;
        }
        
        public void CloseAllPanel()
        {
            foreach (Panel panel in panels)
            {
                panel.Close();
            }
        }
    }

}