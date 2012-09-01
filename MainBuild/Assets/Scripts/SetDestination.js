
var Target : Transform;
private var NavComponent: NavMeshAgent;

function Start () 
{
	NavComponent = this.transform.GetComponent(NavMeshAgent);
	NavComponent.SetDestination(Target.position);
}

function Update () 
{
	//navmeshAgent.setDestination();
}