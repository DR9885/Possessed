using UnityEngine;

[AddComponentMenu("Possessed/EndScene")]
public class EndScene : MonoBehaviour
{
    [SerializeField] private GameObject _CollisionObject = null;
    [SerializeField] private string _LevelToLoad = "";

    void OnTriggerEnter(Collider collider)
    {
        if(_LevelToLoad != "")
            if (collider.gameObject == _CollisionObject)
                Application.LoadLevel(_LevelToLoad);
    }
}