using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Singleton for easy access
    private static SaveData _current;
    public static SaveData Current
    {
        get
        {
            if(_current == null)           
                _current = new SaveData();
            return _current;
            
        }
        set
        {
            if(_current != value)
            {
                _current = value;
            }
        }
        
    }


    //Data i wish to save
    public List<ObjectData> objects = new List<ObjectData>();

}
