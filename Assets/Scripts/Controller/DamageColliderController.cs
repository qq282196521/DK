﻿using UnityEngine;
using KGCustom.Controller;
using KGCustom.Model;

namespace KGCustom.Controller {
    public class DamageColliderController : MonoBehaviour
    {
        public KGCharacterController characterController;
        void OnTriggerEnter2D(Collider2D col) {
            Attack hitAttack = col.gameObject.GetComponent<AttackEffectController>().m_curAttack;
            if (!characterController.hitAttacks.Contains(hitAttack))
            {
                hitAttack.hitPos = (col.gameObject.transform.position + transform.position) / 2;
                characterController.hitAttacks.Push(hitAttack);
                CameraController.Instance.SetCameraEffect(CameraMode.Shake, 2f, 0.5f);
            }
        }
    }
}

