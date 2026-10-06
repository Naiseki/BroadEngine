using System.Reflection;
using System.Data;
using System;
using System.Collections.Generic;
using MathKit;


namespace Broad
{
    struct TiledLevelGenerator
    {
        Root root;
        Scene scene;

        public TiledLevelGenerator(Root root, Scene scene)
        {
            this.root = root;
            this.scene = scene;
        }


        public void Run(string nameSpace)
        {
            foreach (var layer in root.layers) {
                GenObjects(layer, scene, nameSpace);
            }
        }


        void GenObjects(Layer layer, Scene scene, string nameSpace)
        {
            if (layer.objects is null) 
                return;
            foreach (var obj in layer.objects) {
                var actor = CreateActor(obj, nameSpace);
                actor.Form = GetTransform(obj);
                scene.AddActor(actor);
            }
        }


        Actor CreateActor(Object obj, string nameSpace)
        {
            var assembly = System.Reflection.Assembly.GetEntryAssembly();
            var props = GetProperties(obj.properties);
            var type = assembly.GetType($"{nameSpace}.{obj.type}");
            return Activator.CreateInstance(type, props) as Actor; 
        }


        object[] GetProperties(List<Property> props)
        {
            if (props is null) {
                return null;
            }

            var res = new object[props.Count];
            for (int i = 0; i < props.Count; i++) {
                var val = props[i].value;
                res[i] = props[i].type switch { 
                    "float" => val.GetSingle(),
                    "bool" => val.GetBoolean(),
                    "int" => val.GetInt32(),
                    "string" => val.GetString(),
                    _ => throw new Exception("Invalid property type")
                };
            }
            return res;
        }


        Transform GetTransform(Object obj)
        {
            var sca = new Float2(obj.width / root.tilewidth, obj.height / root.tileheight);
            var rot = MathUtil.ToRadians(obj.rotation);
            var pos = new Float2(obj.x/ root.tilewidth, obj.y / root.tileheight) + Float2.Rotate(sca * 0.5f, -rot);
            return new Transform(pos, sca, rot);
        }
    }
}