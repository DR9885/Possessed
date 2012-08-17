using UnityEngine;

public class Tag : ScriptableObject
{
    [SerializeField] private string _Key = "Name";
    [SerializeField] private string _Value = "Speaker";

    public string Key
    {
        get { return _Key; }
        set { _Key = value; }
    }

    public string Value
    {
        get { return _Value; }
        set { _Value = value; }
    }

    public override bool Equals(object o)
    {
        if (ReferenceEquals(null, o)) return false;
        if (ReferenceEquals(this, o)) return true;
        return Equals(o as Tag);
    }

    public bool Equals(Tag other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && Equals(other._Key, _Key) && Equals(other._Value, _Value);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = base.GetHashCode();
            result = (result*397) ^ (_Key != null ? _Key.GetHashCode() : 0);
            result = (result*397) ^ (_Value != null ? _Value.GetHashCode() : 0);
            return result;
        }
    }
}