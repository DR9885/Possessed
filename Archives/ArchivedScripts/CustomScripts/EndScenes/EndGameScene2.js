@script AddComponentMenu("Scripts/Possessed/EndGameScene2")
function OnTriggerEnter(other:Collider)
{
//	//debug.log(other.gameObject.name);
	if(other.gameObject.name=="Character_Animated_Seth")
	{
		Application.LoadLevel("Ch3_Alpha");
	}
}