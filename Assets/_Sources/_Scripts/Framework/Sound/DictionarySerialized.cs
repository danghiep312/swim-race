namespace Framework.Audio
{
    #if UNITY_EDITOR
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class DictionarySerialized : SerializedMonoBehaviour
    {
        [Searchable] public Dictionary<string, AudioClip> audioMapping;

        public void GetMapping(List<AudioClip> clips)
        {
            foreach (AudioClip clip in clips)
            {
                audioMapping.Add(clip.name, clip);
            }
        }
    }

#endif
}