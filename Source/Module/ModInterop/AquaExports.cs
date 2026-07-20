using Celeste.Mod.Aqua.Core;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.ModInterop;
using System;
using System.Collections.Generic;

namespace Celeste.Mod.Aqua.Module
{
    [ModExportName("Aqua")]
    public static class AquaExports
    {
        public static Entity GetGrapplingHook(Player player)
        {
            return player.GetGrappleHook();
        }

        public static int GetGrapplingHookState(Entity hook)
        {
            if (hook is GrapplingHook e)
                return (int)e.State;
            return -1;
        }

        public static Vector2 GetGrapplingHookRopeDirection(Entity hook)
        {
            if (hook is GrapplingHook e)
                return e.RopeDirection;
            return Vector2.Zero;
        }

        public static Vector2 GetGrapplingHookTangent(Entity hook)
        {
            if (hook is GrapplingHook e)
                return e.SwingDirection;
            return Vector2.Zero;
        }

        public static void ShootGrapplingHook(Entity hook, Level level, Vector2 direction, float speed, float speedCoefficient)
        {
            if (hook is GrapplingHook e && !e.Active)
                e.Emit(level, direction, speed, speedCoefficient);
        }

        public static void RevokeGrapplingHook(Entity hook)
        {
            if (hook is GrapplingHook e && e.Active && e.State != GrapplingHook.HookStates.Revoking)
                e.Revoke();
        }

        public static void AddPredictMovementForGrapple(Entity hook, Entity attachedEntity, Vector2 movement)
        {
            if (hook is GrapplingHook e && e.Active && e.State == GrapplingHook.HookStates.Fixed)
            {
                e.PivotsFollowAttachment(attachedEntity, movement, new Vector2((int)movement.X, (int)movement.Y));
            }
        }

        public static bool IsEntityHookable(Entity entity)
        {
            return entity.IsHookable();
        }

        public static void SetEntityHookable(Entity entity, bool hookable)
        {
            entity.SetHookable(hookable);
        }

        public static bool IsHookAttached(Entity entity)
        {
            return entity.IsHookAttached();
        }

        public static bool IsIntersectsWithRope(Entity entity)
        {
            return entity.IntersectsWithRope();
        }

        public static void GetAttachingGrapples(Entity entity, IList<Entity> results)
        {
            var players = entity.Scene.Tracker.GetEntities<Player>();
            foreach (Player player in players)
            {
                var grapple = player.GetGrappleHook();
                if (grapple != null && grapple.Active && grapple.State == GrapplingHook.HookStates.Fixed && grapple.AttachedEntity == entity)
                {
                    results.Add(grapple);
                }
            }
        }

        public static void SetEntityAttachCallbacks(Entity entity, Action<Entity> onAttach, Action<Entity> onDetach)
        {
            entity.SetAttachCallbacks(new Action<GrapplingHook>(onAttach), new Action<GrapplingHook>(onDetach));
        }

        public static void RegisterGrapplingHookCollision(Entity entity, Action<Entity> action)
        {
            var handler = new Action<GrapplingHook>(action);
            HookCollider com = entity.Get<HookCollider>();
            if (com == null)
            {
                entity.Add(new HookCollider(handler));
            }
            else
            {
                com.Callback = handler;
            }
        }

        public static void RegisterGrapplingHookInteraction(Entity entity, Func<Entity, Vector2, bool> interaction)
        {
            var handler = new HookInteractable.InteractHookHandler(interaction);
            HookInteractable com = entity.Get<HookInteractable>();
            if (com == null)
            {
                entity.Add(new HookInteractable(handler));
            }
            else
            {
                com.Interaction = handler;
            }
        }
    }
}
