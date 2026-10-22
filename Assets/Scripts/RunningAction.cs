using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RunningAction", story: "[Self] running towards [Target]", category: "Action", id: "71cc7dbda002197efc172549147931f7")]
public partial class RunningAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    
    private SmoothMotion _smoothMotion;
    private Vector2 _startingVector;

    protected override Status OnStart()
    {
        Debug.Log("Running action");
        _smoothMotion = new SmoothMotion();
        _startingVector = Self.Value.transform.position;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        while (Self.Value.transform.position.y > Target.Value.transform.position.y)
        {
            Self.Value.transform.position = _smoothMotion.StoppableInterpolateVector(_startingVector, Target.Value.transform.position, 10);
            return Status.Running;
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

