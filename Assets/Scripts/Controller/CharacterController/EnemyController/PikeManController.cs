﻿using KGCustom.Model.Behavior.EnemyBehavior.PikeManBehavior;
using KGCustom.Model.Character.Enemy;
using System.Collections.Generic;
using UnityEngine;

namespace KGCustom.Controller.CharacterController.EnemyController
{
    public class PikeManController : KGEnemyController
    {
#if UNITY_EDITOR
        public static PikeManController instance;
        protected override void SetAnim(int index)
        {
            string animName = (index == -1)? "&&" : m_behaviors[index].animName;
            foreach (AttackEffect ae in character.m_skills.skillList) {
                if (ae.name == animName)
                {
                    ae.pRange = 10000;
                }
                else ae.pRange = 0;
            }
        }
#endif
        private Dictionary<string, CharacterBehavior> animToState = new Dictionary<string, CharacterBehavior>()
        {
            { "atk_1" ,new ATK_1() },
            { "atk_2", new ATK_2() },
            { "move",  new Move() },
            { "defence", new Defence()},
            { "idle", new Idle()},
            { "dead", new Dead()},
            { "damage", new Damage()},
        };

        void Start() {
#if UNITY_EDITOR
            if (instance == null) instance = this;
#endif
            if (!initFinished)
            {
                init();
                initFinished = true;
            }
        }
        void Update()
        {
#if UNITY_EDITOR
            if (m_behaviors.Count != 0)
            {
                for (int i = 0; i < m_behaviors.Count; i++)
                {
                    if (animToState.ContainsKey(m_behaviors[i].animName)) {
                        animToState[m_behaviors[i].animName].animCurve = m_behaviors[i].curve;
                        animToState[m_behaviors[i].animName].xTransfer = m_behaviors[i].xTransfer;
                    }
                    else Debug.LogError("AnimationCurve Update Error: No " + m_behaviors[i].animName);
                }
            }
#endif
            ECUpdate();
        }

        protected override void init()
        {
            character = new PikeMan();
            for (int i = 0; i < m_behaviors.Count; i++) {
                if (animToState.ContainsKey(m_behaviors[i].animName))
                    animToState[m_behaviors[i].animName].animCurve = m_behaviors[i].curve;
                else if (m_behaviors[i].animName == "")
                {
                    Debug.LogError("动画名不能为空");
                }
                else Debug.LogError("动画名" + m_behaviors[i].animName + "不存在");
            }
            character.xDirection = Global.GlobalValue.XDIRECTION_RIGHT;
            transform.localScale = Vector3.right * transform.localScale.x * -character.xDirection + Vector3.one - Vector3.right;
            base.init();
        }

        public override void DoDamage()
        {
            Model.Attack atk = hitAttacks.Pop();
            character.hp -= atk.m_AttackEffect.getDamageValue();
            
            character.xDirection = -atk.direction;
            transform.localScale = new Vector3(-character.xDirection, 1, 1);
            GameObject hiteffect = (GameObject)Instantiate(HitEffect, atk.hitPos, HitEffect.transform.rotation);
            hiteffect.GetComponent<HitEffect>().PlayHitEffect(3);
            if (hitAttacks.Count != 0) {
                DoDamage();
            }
            if (character.hp <= 0)
            {
                DoDead();
                return;
            }
            if (character.curState == animToState["damage"])
            {
                m_SkeletonAnim.state.SetAnimation(0, "damage", false);
                return;
            }
            m_SkeletonAnim.AnimationName = "damage";
            m_SkeletonAnim.state.GetCurrent(0).loop = false;
            ChangeState();
            base.DoDefence();
        }
        public override void DoDead()
        {
            if (character.curState == animToState["dead"])
            {
                return;
            }
            m_SkeletonAnim.AnimationName = "dead";
            m_SkeletonAnim.state.GetCurrent(0).loop = false;
            ChangeState();
        }
        public override void DoIdle()
        {
            if (character.curState == animToState["idle"])
            {
                return;
            }
            m_SkeletonAnim.AnimationName = "idle";
            m_SkeletonAnim.state.GetCurrent(0).loop = true;
            ChangeState();
        }

        public override void DoMove() {
            if (character.curState == animToState["move"]) {
                return;
            }
            m_SkeletonAnim.AnimationName = "move";
            m_SkeletonAnim.state.GetCurrent(0).loop = true;
            ChangeState();
        }

        public override void DoAttack(AttackEffect ae) {
            if (character.curState == animToState[ae.name])
            {
                return;
            }
            base.DoAttack(ae);
        }

        public override void DoDefence()
        {
            Model.Attack atk = hitAttacks.Pop();
            character.xDirection = -atk.direction;
            transform.localScale = new Vector3(-character.xDirection, 1, 1);
            GameObject hiteffect = (GameObject)Instantiate(HitEffect, atk.hitPos, HitEffect.transform.rotation);
            hiteffect.GetComponent<HitEffect>().PlayHitEffect(3);
            if (hitAttacks.Count != 0) {
                DoDefence();
            }
            if (character.curState == animToState["defence"])
            {
                m_SkeletonAnim.state.SetAnimation(0, "defence", false);
                return;
            }
            m_SkeletonAnim.AnimationName = "defence";
            m_SkeletonAnim.state.GetCurrent(0).loop = false;
            ChangeState();
        }

        protected override CharacterBehavior GetState(string animName)
        {
            if (animToState.ContainsKey(animName)) {
                return animToState[animName];
            }
            return null;
        }
    }
}

