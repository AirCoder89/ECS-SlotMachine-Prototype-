
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public static class GameExtension
{
        public static bool isOnMove;
        private static Contexts _contexts;
        private static System.Random _random = new System.Random();
        public static float spinSpeed;
        
       
        
        public static void SetContexts(Contexts contexts)
        {
           _contexts = contexts;
        }

        public static void OnClickSpinButton()
        {
            if (_contexts == null)
            {
                Debug.Log("_contexts is null");
                return;
            }
            spinSpeed = Random.Range(_contexts.game.sharedVal.value.speedRange.x,
                _contexts.game.sharedVal.value.speedRange.y);
            isOnMove = true;
            foreach (var entity in _contexts.game.GetEntities())
            {
                entity.isOnMove = true;
                entity.isMoveComplete = false;
            }
            
            GameController.Instance.StartSpinning();
        }
        
        public static void OnStopSpin()
        {
            if (_contexts == null)
            {
                Debug.Log("_contexts is null");
                return;
            }
            isOnMove = false;
            foreach (var entity in _contexts.game.GetEntities())
            {
                entity.isOnMove = false;
                entity.isMoveComplete = true;
            }
            
            GameController.Instance.StopAllMoveSystem();
        }

        public static GameEntity CreateEntity(Contexts contexts, Dimension position,bool isVisible)
        {
            if (_contexts == null) _contexts = contexts;
            
                var entity = contexts.game.CreateEntity();
                entity.AddGridPosition(position);
                entity.isVisible = isVisible;
                entity.AddItemId(RandomString(16));
                entity.AddType(contexts.game.sharedVal.value.GetRandomSlotInfo());
                entity.isMoveComplete = true;
                return entity;
        }
        
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public static Vector2 GetCanvasPosition(Dimension gridPos)
        {
            var spacing = _contexts.game.sharedVal.value.spacing;
            var offset = _contexts.game.sharedVal.value.offset;
           
            var desiredPos = new Vector2(gridPos.x * spacing.x, gridPos.y * spacing.y);
            //offset
            desiredPos.x += offset.x;
            desiredPos.y += offset.y;

            return desiredPos;
        }
}