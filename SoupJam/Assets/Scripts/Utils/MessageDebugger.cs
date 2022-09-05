using UnityEngine;

public class MessageDebugger : MonoBehaviour
{
    public void DebugMessage(string msg)
    {
        Debug.Log(transform.name + ": " + msg);
    }
}
