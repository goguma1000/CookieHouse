using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Fusion;
using System.Reflection;
using System;

public class SessionProps : MonoBehaviour
{
    public string RoomName = "AutoHouse";
    public int PlayerLimit = 2;
    public bool AllowLateJoin = false;

    public SessionProps()
    {

    }
    public SessionProps(ReadOnlyDictionary<string, SessionProperty> props)
    {
        if(props == null)
        {
            Debug.LogError("Property collection is null!");
            return;
        }
        foreach(FieldInfo field in GetType().GetFields())
        {
            if(props.TryGetValue(field.Name, out var name))
            {
                field.SetValue(this, ConvertFromSessionProp(name, field.FieldType));
            }
            else
            {
                Debug.Log($"No property value for field [{field.Name}]");
            }
        }
    }

    public Dictionary<string, SessionProperty> properties
    {
        get
        {
            Dictionary<string, SessionProperty> props = new Dictionary<string, SessionProperty>();
            foreach (FieldInfo field in GetType().GetFields())
            {
                props[field.Name] = ConvertToSessionProp(field.GetValue(this));
            }
            return props;
        }
    }

    private object ConvertFromSessionProp(SessionProperty sp, Type toType)
    {
        if (toType == typeof(bool))
            return (int)sp == 1;
        if (sp.IsString)
            return (string)sp;
        return (int)sp;
    }

    private SessionProperty ConvertToSessionProp(object value)
    {
        if (value is string)
            return SessionProperty.Convert(value);
        if (value is bool b)
            return b ? 1 : 0;
        return (int)value;
    }
}
