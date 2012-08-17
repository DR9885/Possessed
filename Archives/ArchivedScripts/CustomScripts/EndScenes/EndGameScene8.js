@script AddComponentMenu("Scripts/Possessed/EndGameScene8")
function OnTriggerEnter(other:Collider)
{
//	//debug.log(other.gameObject.name);
	if(other.gameObject.name=="Player_Ghost")
	{
		Application.LoadLevel("Ch9_Alpha");
	}
}