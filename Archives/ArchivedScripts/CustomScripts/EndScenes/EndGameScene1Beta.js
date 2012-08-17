@script AddComponentMenu("Scripts/Possessed/EndGameScene1Beta")
function OnTriggerEnter(other:Collider)
{
//	//debug.log(other.gameObject.name);
	if(other.gameObject.name=="Player_William")
	{
		Application.LoadLevel("Ch2_Beta");
	}
}