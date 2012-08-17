using System.Collections.Generic;
using System.Linq;

public static class TagExtensions
{
    public static IEnumerable<string> Keys(this IEnumerable<Tag> argTags)
    {
        List<string> keys = new List<string>();
        foreach (Tag t in argTags)
            if (t != null && !keys.Contains(t.Key))
                keys.Add(t.Key);
        return keys;
    }

    public static IEnumerable<string> Values(this IEnumerable<Tag> argTags, string argKey)
    {
        List<string> values = new List<string>();
        foreach (Tag t in argTags)
            if (t != null && t.Key.ToLower() == argKey.ToLower())
                    values.Add(t.Value);
        return values;
    }

    public static IEnumerable<Tag> Tags(this IEnumerable<Tag> argTags, string argKey)
    {
        List<Tag> values = new List<Tag>();
        foreach (Tag t in argTags)
            if (t != null && t.Key == argKey)
                values.Add(t);
        return values;
    }

    public static void ChangeTags(this IEnumerable<Tag> argTags, params Tag[] argTagChanges)
    {
        List<Tag> currentTags = new List<Tag>(argTags);
        foreach (string key in argTagChanges.Keys())
        {
            currentTags.RemoveAll(x => x.Key == key);
            currentTags.AddRange(argTags.Where(x => x.Key == key));
        }
        argTags = currentTags;
    }
}