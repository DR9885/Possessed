@script AddComponentMenu("Scripts/Possessed/EndGameScene7")
function OnTriggerEnter(other:Collider)
{
//	//debug.log(other.gameObject.name);
	if(other.gameObject.name=="Prisoner")
	{
		Application.LoadLevel("Ch8_Alpha");
	}
}