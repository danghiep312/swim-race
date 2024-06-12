using Framework.DesignPattern.Observer;
using Sirenix.OdinInspector;
using UnityEngine;

public class PostEventOnly : MonoBehaviour
{
    [ShowInInspector]
    public EventID eventID;

    
    [Button]
    public void Post()
    {
        this.PostEvent(eventID);
    }
}
