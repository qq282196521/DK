﻿using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
    [TaskCategory("Basic/Input")]
    [TaskDescription("Returns success when the specified button is pressed.")]
    public class IsButtonDown : Conditional
    {
        [Tooltip("The name of the button")]
        public SharedString buttonName;

        public override TaskStatus OnUpdate()
        {
#if CROSSPLATFORMINPUT
            return CrossPlatformInputManager.GetButtonDown(buttonName.Value) ? TaskStatus.Success : TaskStatus.Failure;
#else
            return Input.GetButtonDown(buttonName.Value) ? TaskStatus.Success : TaskStatus.Failure;
#endif
        }

        public override void OnReset()
        {
            buttonName = "Fire1";
        }
    }
}