namespace Framework.DesignPattern.Singleton
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public bool keepAlive = false;

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var singletonObj = new GameObject();
                    singletonObj.name = typeof(T).ToString();
                    _instance = singletonObj.AddComponent<T>();
                }

                return _instance;
            }
        }

        public virtual void Awake()
        {
            if (_instance != null)
            {
                Common.LogWarning("SingleAccessPoint, Destroy duplicate instance " + name + " of " + Instance.name,
                    gameObject);
                Destroy(gameObject);
                return;
            }

            _instance = GetComponent<T>();

            if (keepAlive)
            {
                DontDestroyOnLoad(gameObject);
            }

            if (_instance == null)
            {
                Common.LogWarning("SingleAccessPoint<" + typeof(T).Name + "> Instance null in Awake", gameObject);
                return;
            }

            Common.Log("SingleAccessPoint instance found " + Instance.GetType().Name);
        }
    }
}