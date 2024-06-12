
    using UnityEngine;

    public class TimeController
    {
        private const float DefaultDelayQuestion = 3f;
        
        public float TimeRemain { get; set; }
        public float DelayTime { get; set; }
        
        public void Setup(float time)
        {
            TimeRemain = time;
        }

        public void UpdateTime()
        {
            if (TimeRemain > 0)
            {
                TimeRemain -= Time.deltaTime;
            }

            if (TimeRemain <= 0)
            {
                TimeRemain = 0;
                // TODO: timeout
            }

            if (DelayTime > 0)
            {
                DelayTime -= Time.deltaTime;
            }
        }

        public void ResetDelay()
        {
            DelayTime = DefaultDelayQuestion;
        }
    }
