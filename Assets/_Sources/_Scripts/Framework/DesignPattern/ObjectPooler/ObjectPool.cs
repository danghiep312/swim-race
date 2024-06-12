using System;


namespace Framework.DesignPattern.ObjectPool
{
    
    using System.Collections.Generic;
    using Singleton;
    using Sirenix.OdinInspector;
    using UnityEngine;


    [Serializable]
    public class PoolObject
    {
        public ItemID itemID;
        public GameObject gameObject;
        public int count; // restrict number of this game object
        public bool expandable;
    }

    public class ObjectPool : Singleton<ObjectPool>
    {
        public List<PoolObject> poolObjects;

        [ReadOnly] public List<GameObject> pooledGameObjects;

        public override void Awake()
        {
            base.Awake();
            pooledGameObjects = new List<GameObject>();

            foreach (var item in poolObjects)
                for (int i = 0; i < item.count; i++)
                    pooledGameObjects.Add(CreateGobject(item.gameObject, item.itemID));
        }

        public GameObject Spawn(string tagOfObject, string nameObject = "")
        {
            foreach (var t in pooledGameObjects)
            {
                if (nameObject != "")
                {
                    if (t.gameObject.name != nameObject)
                    {
                        continue;
                    }
                }

                if (!t.activeSelf && t.CompareTag(tagOfObject))
                {
                    t.SetActive(true);
                    return t;
                }
            }

            foreach (var item in poolObjects)
            {
                if (nameObject != "")
                {
                    if (item.gameObject.name != nameObject)
                    {
                        continue;
                    }
                }

                if (item.gameObject.CompareTag(tagOfObject))
                    if (item.expandable)
                    {
                        GameObject obj = CreateGobject(item.gameObject, item.itemID);
                        pooledGameObjects.Add(obj);
                        obj.SetActive(true);
                        return obj;
                    }
            }

            return null;
        }

        public GameObject Spawn(ItemID itemID)
        {
            foreach (var t in pooledGameObjects)
            {
                if (!t.activeSelf && t.name.Contains("_itemid_" + itemID))
                {
                    t.SetActive(true);
                    return t;
                }
            }

            foreach (var item in poolObjects)
            {
                if (item.itemID == itemID)
                    if (item.expandable)
                    {
                        GameObject obj = CreateGobject(item.gameObject, itemID);
                        pooledGameObjects.Add(obj);
                        obj.SetActive(true);
                        return obj;
                    }
            }

            return null;
        }

        private GameObject CreateGobject(GameObject item, ItemID itemId)
        {
            GameObject gObject = Instantiate(item, transform);
            gObject.SetActive(false);
            if (itemId != ItemID.None) gObject.name += ("_itemid_" + itemId);
            return gObject;
        }

        public void ReleaseObject(GameObject item)
        {
            //Debug.Log("Release " + item.name);
            item.SetActive(false);
            item.transform.SetParent(transform);
        }

        public void Release(GameObject item)
        {
            //Debug.Log("Release " + item.name);
            item.SetActive(false);
            item.transform.SetParent(transform);
        }

        public void ReleaseAll()
        {
            foreach (var item in pooledGameObjects)
            {
                ReleaseObject(item);
            }
        }

    }


}