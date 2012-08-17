using UnityEngine;

public class DialogueEventTrigger : MonoBehaviour
{
    [SerializeField] private DialogueEvent[] _DialogueEvents;
    [SerializeField] private TagFilter _TagFilter;

    private void OnTriggerEnter(Collider other)
    {
        Speaker speaker = other.GetComponent<Speaker>();
        if(speaker == null) return;

        if(_TagFilter.Accepted(speaker.Tags))
            foreach(DialogueEvent e in _DialogueEvents)
                e.Execute();
    }

}