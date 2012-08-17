using UnityEngine;
using System.Linq;

public class DialogueRemoteEvent : DialogueEvent
{
    enum Power { On, Off }
    enum Remote { Possession, Dialogue, Pickup, Climb }

    [SerializeField] private Power _Power;
    [SerializeField] private Remote _Remote;
    [SerializeField] private TagFilter _RemoteTarget;


    public override void Execute(Speaker[] speakers)
    {
        foreach (Speaker speaker in speakers)
        {
            Object[] remTargets = FindObjectsOfType(typeof (Speaker));
            foreach (Speaker remTarget in remTargets)
            {
                if (_RemoteTarget.Accepted(remTarget.Tags))
                {
                    AbstractRemote remote = null;
                    switch (_Remote)
                    {
                        case Remote.Dialogue:
                            remote = speaker.GetComponent<DialogueRemote>();
                            break;
                        case Remote.Possession:
                            remote = speaker.GetComponent<PossessionRemote>();
                            break;
                        case Remote.Pickup:
                            remote = speaker.GetComponent<PickupRemote>();
                            break;
                        case Remote.Climb:
                            remote = speaker.GetComponent<SquirrelClimbRemote>();
                            break;

                    }

                    AbstractEntity entity = remTarget.GetComponent<AbstractEntity>();
                    if (remote != null && remote.TargetCheck(entity))
                        speaker.StartCoroutine(_Power == Power.On
                                                   ? remote.TurnOn(entity)
                                                   : remote.TurnOff(entity));
                }
            }

        }
    }
}