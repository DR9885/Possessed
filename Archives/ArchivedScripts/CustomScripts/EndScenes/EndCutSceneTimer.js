@script AddComponentMenu("Scripts/Possessed/EndGameScene1")
var movementRate = 0.5;

function Update () {

    transform.position.z+=movementRate*Time.deltaTime;
    
    }