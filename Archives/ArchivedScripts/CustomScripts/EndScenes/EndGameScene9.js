@script AddComponentMenu("Scripts/Possessed/EndGameScene9")
function OnTriggerEnter(other:Collider)
{
//	//debug.log(other.gameObject.name);
	if(other.gameObject.name=="Wolf")
	{
		Application.LoadLevel("Ch1_Alpha");
	}
}