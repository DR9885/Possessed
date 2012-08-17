using UnityEngine;

[AddComponentMenu("Possessed/Being/Animal/Squirrel")]
public class Squirrel : AbstractAnimal
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
        ThirdPersonCamera.height = 0.05f;
        ThirdPersonCamera.distance = 0.8f;

        CharacterController.height = 0.15f;
        CharacterController.radius = 0.15f;
        CharacterController.center = Vector3.up * 0.32f;
    }
}