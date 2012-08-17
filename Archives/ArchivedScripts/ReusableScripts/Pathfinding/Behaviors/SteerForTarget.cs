using UnityEngine;

[AddComponentMenu("UnitySteer/SteerForTarget")]
public class SteerForTarget : Steering {
	
	/// <summary>
	/// Target the behavior will aim for
	/// </summary>
	public Transform Target = null;
	
	
	void Awake()
	{
        //if (Target == null)
        //{
        //    Destroy(this);
        //    throw new System.Exception("SteerForTarget need a target transform. Dying.");
        //}
	}


    void Start()
    {

    }

	/// <summary>
	/// Calculates the force to apply to a vehicle to reach a target transform
	/// </summary>
	/// <returns>
	/// Force to apply <see cref="Vector3"/>
	/// </returns>
	protected override Vector3 CalculateForce()
	{
		return (Target != null)? Vector3.zero : Vehicle.GetSeekVector(Target.position);
	}
}