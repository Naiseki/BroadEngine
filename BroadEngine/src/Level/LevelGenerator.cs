using System.ComponentModel;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Broad
{
    struct LevelGenerator
    {
        Assembly assembly;


        void CreateLevel(Level levelData, Scene scene, Actor levelActor)
        {
            assembly = scene.GetType().Assembly;
            foreach (var actorData in levelData.Actors) {
                var actor = CreateActor(actorData);
                foreach (var c in actorData.Components)
                {
                    SetComponent(actor, c);
                }
                scene.AddActor(actor);
            }
        }

        Actor CreateActor(ActorData data)
        {
            return assembly.CreateInstance(data.Name) as Actor;
        }

        void SetComponent(Actor actor, ComponentData c)
        {
            var instance = assembly.CreateInstance(c.Name) as Component;
            if (instance is null)
                return;

            var type = assembly.GetType(c.Name);
            foreach (var prop in c.Properties) {
                if (type.GetField(prop.Key, BindingFlags.NonPublic) is FieldInfo info) {
                    info.SetValue(instance, prop.Value);
                }
            }
            actor.AddComponent(instance);
        }
    }
}
