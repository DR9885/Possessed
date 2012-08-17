using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceLoader<T> : IEnumerable where T : UnityEngine.Object
{
    private string _Address;

    private Object[] _Objects;
    protected Object[] Objects
    {
        get
        {
            if (_Objects == null)
                _Objects = Resources.LoadAll(_Address, typeof (T));
            return _Objects;
        }
    }

    public void Load()
    {
        if (Objects != null) return;          
    }

    public T this[string argValue]
    {
        get { return Objects.FirstOrDefault(x => x != null && x.name.ToLower() == argValue.ToLower()) as T; }
    }

    public ResourceLoader() { _Address = ""; }
    public ResourceLoader(string resourceSubFolder) { _Address = resourceSubFolder; }
    


    public IEnumerator GetEnumerator()
    {
        return Objects.GetEnumerator();
    }
}