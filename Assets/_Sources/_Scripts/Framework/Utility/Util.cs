using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;
#if SPINE_INSTALLED
using Spine.Unity;
using Spine;
#endif


public static class Util //: MonoBehaviour
{
    public static void Swap<T>(this List<T> list, int i, int j)
    {
        (list[i], list[j]) = (list[j], list[i]);
    }

    public static void Shuffle<T>(this List<T> list)
    {
        int n = list.Count;
        for (int i = 0; i < n; i++)
        {
            int newPos = Random.Range(0, n);
            list.Swap(i, newPos);
        }
    }

    public static async void Delay(float time, Action action, CancellationTokenSource cts = default)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: cts?.Token ?? CancellationToken.None);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Cancel " + action.Method.Name);
            return;
        }

        action?.Invoke();
    }

    public static async void DelayFrame(int frame, Action action, CancellationTokenSource cts = default)
    {
        try
        {
            await UniTask.DelayFrame(frame, cancellationToken: cts?.Token ?? CancellationToken.None);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Cancel " + action.Method.Name);
            return;
        }

        action?.Invoke();
    }

    public static async void ActivateLastFrame(Action action)
    {
        await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
        action?.Invoke();
    }

    // Mark vibration handler
    public static void Vibrate(Action action)
    {
        action?.Invoke();
    }

    public static string RemoveSpace(string objectName)
    {
        var res = objectName.Trim();
        return Regex.Replace(res, "\\s+", "");
    }

    public static void BringToTop(this Transform transform)
    {
        var position = transform.position;
        position += -(Vector3.forward * position.z);
        transform.position = position;
    }

    #region Check Click Over UI

    public static bool ClickOverUI()
    {
        return IsPointerOverUIElement();
    }

    private static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    private static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaycastResults)
    {
        int uiLayer = LayerMask.NameToLayer("UI");
        for (int index = 0; index < eventSystemRaycastResults.Count; index++)
        {
            RaycastResult curRaycastResult = eventSystemRaycastResults[index];
            if (curRaycastResult.gameObject.layer == uiLayer)
                return true;
        }

        return false;
    }

    //Gets all event system raycast results of current mouse or touch position.
    private static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }

    //Returns 'true' if we touched or hovering on Unity UI element.

    #endregion

    #region Spine Animation Support

#if SPINE_INSTALLED
    // How to mark define: Project Setting -> Player -> Other Settings -> Scripting Define Symbols


    public static TrackEntry PlayAnimation(this SkeletonAnimation animation, string animationName, int trackIndex = 0,
        bool loop = false, Action onComplete = null, Action onDisruption = null)
    {
        TrackEntry lastEntry = animation.state.SetAnimation(trackIndex, animationName, loop);

        void completeHandle(TrackEntry entry)
        {
            onComplete?.Invoke();
            lastEntry.Complete -= completeHandle;
        }

        if (onComplete != null)
        {
            lastEntry.Complete += completeHandle;
        }

        return lastEntry;
    }

    public static void PlayAnimationQueue(this SkeletonAnimation animation, string[] animationNames,
        float[] delayTime = null, bool loop = false, bool separateTracks = false, Action onComplete = null)
    {
        int track = 0;
        TrackEntry lastTrackEntry = null;

        for (int i = 0; i < animationNames.Length; i++)
        {
            lastTrackEntry = animation.state.AddAnimation(separateTracks ? track++ : 0, animationNames[i], loop,
                (delayTime != null && i < delayTime.Length) ? delayTime[i] : 0f);
        }

        if (lastTrackEntry != null)
            lastTrackEntry.Complete += completeEventHandle;
        return;

        void completeEventHandle(TrackEntry trackEntry)
        {
            if (trackEntry == lastTrackEntry)
            {
                onComplete?.Invoke();
                // Debug.Log("Complete");
                lastTrackEntry.Complete -= completeEventHandle;
            }
        }
    }

    public static void InvokeEventAfterAnimation(this SkeletonAnimation animation, string animName, Action action)
    {
        animation.state.Complete += checkTrackEntry;
        return;

        void checkTrackEntry(TrackEntry entry)
        {
            if (entry.Animation.Name == animName)
            {
                action?.Invoke();
                entry.Complete -= checkTrackEntry;
            }
        }
    }


    public static void PlayAnimation(this SkeletonGraphic animation, string animationName, bool loop = false)
    {
        animation.AnimationState.SetAnimation(0, animationName, loop);
    }

    public static void PlayAnimationQueue(this SkeletonGraphic animation, string[] animationNames,
        float[] delayTime = null, bool loop = false, bool separateTracks = false, Action onComplete = null)
    {
        int track = 0;
        TrackEntry lastTrackEntry = null;

        for (int i = 0; i < animationNames.Length; i++)
        {
            lastTrackEntry = animation.AnimationState.AddAnimation(separateTracks ? track++ : 0, animationNames[i],
                loop, (delayTime != null && i < delayTime.Length) ? delayTime[i] : 0f);
        }

        if (lastTrackEntry != null)
            lastTrackEntry.Complete += completeEventHandle;
        return;

        void completeEventHandle(TrackEntry trackEntry)
        {
            if (trackEntry == lastTrackEntry)
            {
                onComplete?.Invoke();
                // Debug.Log("Complete");
                lastTrackEntry.Complete -= completeEventHandle;
            }
        }
    }
#endif

    #endregion

    public static T ParseEnum<T>(this string sample)
    {
        return (T)Enum.Parse(typeof(T), sample);
    }

    public static T ParseEnum<T>(this int index)
    {
        return (T)Enum.Parse(typeof(T), index.ToString());
    }


    public static int GetIndex<T>(this T sample)
    {
        return (int)(object)sample;
    }


    public static void SetStatusCollider2D(this GameObject go, bool status)
    {
        foreach (Collider2D col in go.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = status;
        }
    }

    public static void FlipX(this Transform transform)
    {
        transform.localScale = new Vector3(-transform.localScale.x, 0, 0);
    }

    public static void SetActive(this MonoBehaviour obj, bool status)
    {
        obj.gameObject.SetActive(status);
    }

    public static void SetStatus<T>(this IEnumerable<T> list, bool status) where T : MonoBehaviour
    {
        foreach (var obj in list)
        {
            obj.SetActive(status);
        }
    }

    public static async UniTask<GameObject> GenerateAsset(this AssetReference assetRef, Transform parent = null)
    {
        UniTask<GameObject> handle = assetRef.InstantiateAsync(parent).Task.AsUniTask();
        GameObject go = await handle;
        return go;
    }

    public static async UniTask<GameObject> GenerateAsset(this string key, Transform parent = null)
    {
        UniTask<GameObject> handle = Addressables.InstantiateAsync(key, parent).Task.AsUniTask();
        GameObject go = await handle;
        return go;
    }

    public static void SetSprite<T>(this IEnumerable<T> images, Sprite sprite) where T : Component
    {
        foreach (var image in images)
        {
            switch (image)
            {
                case Image img:
                    img.sprite = sprite;
                    break;
                case SpriteRenderer sr:
                    sr.sprite = sprite;
                    break;
                default:
                    throw new ArgumentException("Component must be a SpriteRenderer or Image");
            }
        }
    }

    // public static void SetSprite(this List<Image> images, Sprite sprite)
    // {
    //     foreach (Image image in images) image.sprite = sprite;
    // }
    //
    // public static void SetSprite(this Image[] images, Sprite sprite)
    // {
    //     foreach (Image image in images) image.sprite = sprite;
    // }

    public static void SetAlpha(this Image image, float alpha)
    {
        Color currentColor = image.color;
        // Set the alpha value of the color
        currentColor.a = alpha;
        // Assign the modified color back to the image
        image.color = currentColor;
    }

    public static DateTime ParseDateTime(this string time, string format = "MM/dd/yyyy")
    {
        return DateTime.ParseExact(time, format, System.Globalization.CultureInfo.InvariantCulture);
    }

    public static T GetRandom<T>(this Type t) where T : Enum
    {
        var list = Enum.GetValues(t).Cast<T>().ToArray();
        return list[Random.Range(0, list.Length)];
    }

    public static bool IsInternetConnected()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
    
    public static string GetFormattedTime (this TimeSpan span)
    {
        return $"{span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}";
    }
}