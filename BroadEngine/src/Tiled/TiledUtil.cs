using System;
using System.Collections.Generic;
using MathKit;

namespace Broad
{
    public static class TiledUtil
    {
        public static void CreateLevel(Scene scene, string sceneFile, string nameSpace)
        {
            var root = JsonUtil.Load<Root>(sceneFile);
            var generator = new TiledLevelGenerator(root, scene);
            generator.Run(nameSpace);
        }
    }
}