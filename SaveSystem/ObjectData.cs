using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectData
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;

    public ObjectData (string nam, Vector3 pos, Quaternion rot)
    {
        name = nam;
        position = pos;
        rotation = rot;
    }
}
