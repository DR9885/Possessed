using UnityEngine;

[AddComponentMenu("Possessed/Being/Animal/Cat")]
public class Cat : AbstractAnimal
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
        ThirdPersonCamera.height = 0.15f;
        ThirdPersonCamera.distance = 0.7f;

        CharacterController.height = 0.1227f;
        CharacterController.radius = 0.18f;
        CharacterController.center = Vector3.up * 0.26f;
    }
}