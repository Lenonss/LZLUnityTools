using System.ComponentModel;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LZLUnityTool.Plugins.InventoryPlugin.System.TaskSystem.Scripts.SO.Charactor
{
    [CreateAssetMenu(fileName = "char sm params", menuName = "LZLUnityTool/ScriptableObjects/角色/角色状态机参数")]
    public class CharacterSMParams : ScriptableObject
    {
        [SerializeField, Title("行走速度")]
        private float walkSpeed = 5;
        public float WalkSpeed => walkSpeed;

        [SerializeField, Title("闪现距离")]
        private float dashDistance = 5;
        public float DashDistance => dashDistance;

        [SerializeField, Title("翻滚速度曲线")]
        private AnimationCurve rollSpeedCurve = new AnimationCurve(new Keyframe(0, 30), new Keyframe(1, 0));
        public AnimationCurve RollSpeedCurve => rollSpeedCurve;

        [SerializeField, Title("翻滚速度生效时间"), MinMaxSlider(0, 1)]
        private Vector2 rollEffectedTime = Vector2.up;
        public Vector2 RollEffectedTime => rollEffectedTime;

        [SerializeField, Title("可提前退出翻滚时间"), MinMaxSlider("rollEffectedTime")]
        private Vector2 rollCanMoveTime = new Vector2(0.75f, 0.85f);
        public Vector2 RollCanMoveTime => rollCanMoveTime;
    }
}