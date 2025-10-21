using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace AutoAim
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private bool _isInit;
        private Harmony? _harmony;

        protected override void OnAfterSetup()
        {
            Debug.Log("OnAfterSetup");
            if (_isInit) return;
            _harmony = new Harmony("Braycep.AutoAim");
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            _isInit = true;
            Debug.Log("AutoAim Initialized");
        }

        protected override void OnBeforeDeactivate()
        {
            _harmony?.UnpatchAll();
            Debug.Log("OnBeforeDeactivate");
        }

        // Patch InputManager 的鼠标瞄准逻辑
        [HarmonyPatch(typeof(InputManager), "SetAimInputUsingMouse")]
        public static class AutoAimPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(InputManager __instance, Vector2 mouseDelta)
            {
                // 仅在玩家按下左键时触发自动瞄准
                // if (!Input.GetMouseButtonDown(0))
                //     return true;

                // 1. 获取玩家角色
                var player = CharacterMainControl.Main;
                if (player == null) return true;

                // 2. 查找最近的敌人（scav 队伍）
                CharacterMainControl? nearestEnemy = null;
                var nearestSqr = 225f; // 15^2

                foreach (var character in FindObjectsOfType<CharacterMainControl>())
                {
                    // 过滤：非空、非玩家、是敌人（scav）
                    if (character == null || character == player || character.Team != Teams.scav)
                        continue;

                    var sqrDist = (character.transform.position - player.transform.position).sqrMagnitude;
                    if (!(sqrDist < nearestSqr)) continue;

                    Debug.Log(
                        $"Found Enemy: {character} at {character.transform.position}, me at: {player.transform.position}");

                    nearestSqr = sqrDist;
                    nearestEnemy = character;
                }

                // 3. 锁定敌人
                if (nearestEnemy != null)
                {
                    Vector3 aimPoint = GetEnemyHeadPosition(nearestEnemy);
                    if (aimPoint == Vector3.zero)
                    {
                        // 如果没找到头部，退化为身体中心
                        aimPoint = nearestEnemy.transform.position;
                    }

                    __instance.characterMainControl.SetAimPoint(aimPoint);
                    Debug.Log($"【AutoAim】锁定敌人头部: {nearestEnemy.name} at {aimPoint}");
                    return false; // 跳过原始鼠标逻辑
                }

                return true; // 无敌人，继续原始逻辑
            }
        }

        private static Vector3 GetEnemyHeadPosition(CharacterMainControl enemy)
        {
            // 方法1：直接找 HeadCollider 组件（推荐）
            var headCollider = enemy.GetComponentInChildren<HeadCollider>();
            if (headCollider != null)
            {
                return headCollider.transform.position;
            }

            // 方法2：通过层 Raycast（模拟游戏原逻辑）
            int headLayer = LayerMask.NameToLayer("HeadCollider");
            if (headLayer != -1)
            {
                Vector3 direction = enemy.transform.position - CharacterMainControl.Main.transform.position;
                direction.y = 0;
                direction = direction.normalized;

                // 从玩家向敌人方向射线，检测头部
                Ray ray = new Ray(CharacterMainControl.Main.transform.position + Vector3.up * 1.5f, direction);
                if (Physics.Raycast(ray, out RaycastHit hit, 50f, 1 << headLayer))
                {
                    return hit.point;
                }
            }

            return Vector3.zero;
        }
        
        [HarmonyPatch(typeof(CharacterMainControl), "GunScatterMultiplier", MethodType.Getter)]
        public static class GunScatterMultiplierPatch
        {
            [HarmonyPostfix]
            public static void Postfix(CharacterMainControl __instance, ref float __result)
            {
                // 如果处于 ADS 状态，将散射乘数减半（可调）
                if (__instance.IsInAdsInput)
                {
                    __result *= 0.1f; // 例如：原为 1.0 → 变为 0.3
                }
            }
        }
    }
}