@script AddComponentMenu("Scripts/Possessed/EndGameScene1")
function OnTriggerEnter(other:Collider)
{
	////debug.log(other.gameObject.name);
	if(other.gameObject.name=="Timer")
	{
		Application.LoadLevel("6_Act3");
	}
}