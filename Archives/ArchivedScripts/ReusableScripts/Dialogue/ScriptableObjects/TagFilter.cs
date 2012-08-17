using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TagFilter : ScriptableObject
{
    [SerializeField]
    private List<Tag> _Scans = new List<Tag>();
    public List<Tag> Scans
    {
        get { return _Scans; }
    }

    public bool Accepted(IEnumerable<Tag> argTags)
    {
        foreach (Tag scan in _Scans)
        {
            if (argTags.Contains(scan))
                return true;
        }
        return false;


        /*
        foreach (string key in _Scans.Keys())
        {

            if (!_Scans.Values(key).Any(x => argTags.Values(key).Any(y => x == y)))
                return false;
        }
        return true;
         */
    }


    //public bool Pass(TagFilter argFilter)
    //{
    //    bool pass = true;
    //    foreach (string key in GetKeys())
    //        if (!GetValues(key).Any(x => argFilter.GetValues(key).Contains(x)))
    //            return false;
    //    return pass;
    //}


    //public bool ContainsKey(string argKey)
    //{
    //    foreach (Tag tag in _Scans)
    //        if (tag._Key == argKey)
    //            return true;
    //    return false;
    //}

    //public bool ContainsTag(Tag argTag)
    //{
    //    return ContainsTag(argTag._Key, argTag._Value);
    //}

    //public bool ContainsTag(string argKey, string argValue)
    //{
    //    foreach (Tag tag in _Scans)
    //        if (tag._Key == argKey && tag._Value == argValue)
    //            return true;
    //    return false;
    //}

}