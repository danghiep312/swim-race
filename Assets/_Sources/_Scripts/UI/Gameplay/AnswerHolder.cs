
    using System.Collections.Generic;
    using UnityEngine;

    public class AnswerHolder : MonoBehaviour
    {
        [SerializeField] private List<AnswerOptionUI> options;

        public void Setup(List<int> answerOptions)
        {
            foreach (AnswerOptionUI option in options)
            {
                option.Setup(new AnswerOption
                {
                    Value = answerOptions[options.IndexOf(option)]
                });
            }
        }
    }
