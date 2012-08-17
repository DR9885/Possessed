using UnityEngine;

[RequireComponent(typeof(DialogueRemote), typeof(Speaker))]
public class DialogueStartTrigger : MonoBehaviour
{
    private TagFilter _Filter = null;

    private BoxCollider _Collider;
    private BoxCollider Collider
    {
        get
        {
            if (_Collider == null)
                _Collider = GetComponent<BoxCollider>();
            if (_Collider == null)
                _Collider = gameObject.AddComponent<BoxCollider>();
            return _Collider;
        }
    }
    
    private DialogueRemote _DialogueRemote;
    private DialogueRemote DialogueRemote
    {
        get
        {
            if (_DialogueRemote == null)
                _DialogueRemote = GetComponent<DialogueRemote>();
            return _DialogueRemote;
        }
    }
     
    private Speaker _Speaker;
    private Speaker Speaker
    {
        get
        {
            if (_Speaker == null)
                _Speaker = GetComponent<Speaker>();
            return _Speaker;
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        Speaker speaker = other.GetComponent<Speaker>();
        if (speaker == null) return;

        // if Is a Character
        if (other is CharacterController)
        {
            //debug.log("START TRIGGER: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }

}