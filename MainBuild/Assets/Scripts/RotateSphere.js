function spin(){
    //yield WaitForSeconds(3.0);
    transform.Rotate(15* Time.deltaTime, 15 * Time.deltaTime, 15 * Time.deltaTime);
}

function Update(){
    spin();
}