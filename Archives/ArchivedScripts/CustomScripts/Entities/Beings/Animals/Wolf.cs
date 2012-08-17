using UnityEngine;

[AddComponentMenu("Possessed/Being/Animal/Wolf")]
public class Wolf : AbstractAnimal
{
    new public void Reset()
    {
        base.Reset();
        SetupData();
    }

    new public void Awake()
    {
        base.Awake();
        SetupData();
    }

    private void SetupData()
    {
        ThirdPersonCamera.height = .3f;
        ThirdPersonCamera.distance = 1.5f;

        CharacterController.height = 0.5f;
        CharacterController.radius = 0.25f;
        CharacterController.center = Vector3.up * 0.25f;
    }
	
}