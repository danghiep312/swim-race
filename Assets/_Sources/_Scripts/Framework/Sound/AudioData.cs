namespace Framework.Audio
{
    using System;
    using DataObject;
    using RotaryHeart.Lib.SerializableDictionary;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Data Object", menuName = "ScriptableObjects/AudioData", order = 1)]
    public class AudioData : ScriptableObjectInstance
    {
        public static AudioData Instance;

        public override void Init()
        {
            Instance = this;
        }

        [Searchable] public SerializableDictionaryBase<AudioIndex, Audio> audioClips;

        public static Audio GetAudio(AudioIndex index)
        {
            return Instance.audioClips[index];
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Enum.GetNames(typeof(AudioIndex)).Length <= audioClips.Count) return;
            foreach (var key in Enum.GetNames(typeof(AudioIndex)))
            {
                AudioIndex audioKey = key.ParseEnum<AudioIndex>();
                if (!audioClips.TryGetValue(audioKey, out var audio))
                {
                    audioClips.Add(audioKey, new Audio());
                }
            }
        }


#endif
    }

    public enum AudioIndex
    {
        NONE = 0,
        BACKGROUND = 1,
        PICK_ITEM = 2,
        DROP_ITEM = 3,
        CLICK = 4,
        WIN = 5,
        LOSE = 6,
        FREEZE = 7,
        STRIKE = 8,
        TRANSFORMATION = 9,
        EARTHQUAKE = 10,
        REWARD = 11,
        COMBO_1 = 12,
        COMBO_2 = 13,
        COMBO_3 = 14,
        COMBO_4 = 15,
        COMBO_5 = 16,
        CHEST_OPEN = 17,
        CHEST_PROGRESS = 18,
        TRUMPET = 19,
        TIME_TICK = 20,
        STAR_GET = 21,
        STAR_COLLECT = 22,
        SPIN_REWARD = 23,

        COMBO_6 = 24,
        COMBO_7 = 25,
        COMBO_8 = 26,
        COMBO_9 = 27,
        COMBO_10 = 28,

        BACKGROUND_HOME = 29,
    }
}