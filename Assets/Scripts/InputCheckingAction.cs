using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "InputCheckingAction", story: "Checking if [ControllerObject] is pressed [isInputPressed]",
    category: "Action", id: "1a513664d144b2a599dbf687234e5873")]
public partial class InputCheckingAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> ControllerObject;
    [SerializeReference] public BlackboardVariable<bool> IsInputPressed;

    private Controller _controller;

    private int _minFrameCount = 1000;
    private int _maxFrameCount = 2000;
    private int _attackingDuration;

    protected override Status OnStart()
    {
        _controller = ControllerObject.Value.gameObject.GetComponent<Controller>();
        _attackingDuration = Random.Range(_minFrameCount, _maxFrameCount);
        Debug.Log("starting checking");
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        _attackingDuration -= 1;
        
        if (_controller.IsInputPressed)
        {
            _controller.DisableControls();
            IsInputPressed.Value = true;
            Debug.Log($"{IsInputPressed.Value} forced ending");
            return Status.Success;
        }

        if (_attackingDuration <= 0)
        {
            Debug.Log("ended checking");
            Debug.Log($"{IsInputPressed.Value} normal ending");
            return Status.Success;
        }
        
        //Debug.Log(_attackingDuration);

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}