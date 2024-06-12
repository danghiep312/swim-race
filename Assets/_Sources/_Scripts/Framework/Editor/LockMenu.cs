using UnityEditor;

namespace Framework.Editor
{
    public class LockMenu : UnityEditor.Editor
    {
        [MenuItem("Tools/Toggle Inspector Lock &q")] // Ctrl + L
        public static void ToggleInspectorLock()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
            ActiveEditorTracker.sharedTracker.ForceRebuild();
        }
    }
}