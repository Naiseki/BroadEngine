using System.Collections.Generic;
using System;

namespace Broad
{
    static class ActorCommands
    {
        static Queue<Actor> destroyCommands = new Queue<Actor>();
        static Queue<Actor> spawnCommands = new Queue<Actor>();


        public static void Spawn(Actor actor) => spawnCommands.Enqueue(actor);

        public static void Destroy(Actor actor) => destroyCommands.Enqueue(actor);


        public static void Execute()
        {
            while (spawnCommands.Count > 0) {
                var actor = spawnCommands.Dequeue();
                actor.HolderScene.PushActor(actor);
            }

            while (destroyCommands.Count > 0) {
                var actor = destroyCommands.Dequeue();
                actor.OnDestroy();
                actor.HolderScene.RemoveActor(actor);
            }
        }
    }
}
