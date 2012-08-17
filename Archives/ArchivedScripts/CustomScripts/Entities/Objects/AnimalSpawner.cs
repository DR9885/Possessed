using UnityEngine;
using System.Collections;

public class AnimalSpawner : MonoBehaviour {

    [SerializeField]
    private AbstractAnimal watchedAnimal;

    private Transform _Transform;
    private Transform Transform
    {
        get
        {
            if (_Transform == null)
                _Transform = transform;
            return _Transform;
        }
    }

    public IEnumerator Start()
    {
        while (Application.isPlaying)
        {
            // Respawn animal
            if (watchedAnimal.isDead())
            {
                watchedAnimal.Renderer.material.shader = Shader.Find("Transparent/Diffuse");
                watchedAnimal.LerpAlpha(0, 5);
                watchedAnimal._isDead = 0;
                watchedAnimal.transform.position = this.gameObject.transform.position;
                watchedAnimal.LerpAlpha(100, 5); 
                watchedAnimal.Renderer.material.shader = Shader.Find("Diffuse");
                watchedAnimal.Animation.CrossFade(AnimationResources.Idle);
                watchedAnimal._isDead = 0;
            }

            yield return new WaitForSeconds(5);
        }
    }
}
