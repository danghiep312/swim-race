
    using UnityEngine;

    public class TimeController
    {
        public float TimeRemain { get; set; }
        
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
        }
    }
