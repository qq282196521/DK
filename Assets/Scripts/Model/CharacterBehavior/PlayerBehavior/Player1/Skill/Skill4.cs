﻿using KGCustom.Controller;

namespace KGCustom.Model {
    public class Skill4 : PlayerBehavior<Skill4>
    {
        public override void begin(KGCharacterController cc)
        {
            attackBegin((PlayerController)cc);
        }

        public override void execute(KGCharacterController cc)
        {
            if (damageCount((PlayerController)cc)) return;
            skillExecute((PlayerController)cc);
            base.execute((PlayerController)cc);
        }

        public override void end(KGCharacterController cc)
        {
            attackEnd(cc);
            CameraController.Instance.SetCameraEffect(CameraMode.Focus, false, -1f, -1f, -1f, null);
        }
    }
}
