using System;
using Broad;
using Broad.Graphics;
using Broad.Physics;
using Broad.Audio;
using MathKit;

namespace EngineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new GameSettings
            {
                WindowSize = new Int2(1600, 900),
                Title = "Broad",
                GraphicsLayerCount = 2,
                RenderFrequency = 60,
                UpdateFrequency = 60
            };
            Assets.RegisterAssets(
                new SpriteInfo("EngineTest.Resources.forest.png", "map", new Int2(16)),
                new SpriteInfo("EngineTest.Resources.ball.png", "circle", new Int2(64)),
                new SpriteInfo("EngineTest.Resources.background.jpg", "bg", new Int2(1600, 900))
            //  new AudioInfo("EngineTest.Resources.music.ogg", "music")
            );

            AudioUtil.SetDistanceModel(AudioDistanceModel.LinearDistanceClamped);
            Camera.ViewScale = 10f;
            PhysicsUtil.Gravity = new Float2(0, 5);
            Game.Run(settings, typeof(MainScene));
        }
    }

    struct Sample
    {
        public string Name { get; set; }
        public int HP { get; set; }

        public override string ToString()
        {
            return $"name: {Name}, hp: {HP}";
        }
    }

    class MainScene : Scene
    {
        protected override void OnStart()
        {
            AddActor(new Player(new Float2(3, 0)));
            AddActor(new Player2(0, 0, true));
            AddActor(new Map());
            AddActor(new FrameCounter());
            //AddActor(new BG());
            //AddActor(new BG());
            //AddActor(new Player(new Float2(5f, 5f)));
            //TiledUtil.CreateLevel(this, "example.json", "EngineTest");
        }


        protected override void OnUpdate()
        {
            //AddActor(new Player2(4.5f, 0f));
        }

    }

    class NewScene : Scene
    {
        protected override void OnStart()
        {
            AddActor(new Player2(0f, 0f, true));
            // Assets.LoadAndSliceImage("forest.png", 16, 16, "map");

        }

        protected override void OnUpdate()
        {
        }

    }


    class Player : Actor
    {

        public Player(Float2 pos)
        {
            // sprite = Assets.GetSprite("circle");
            Form.Position = pos;
            Form.Scale = new Float2(1, 1);
            //AddComponent(body);
        }


        protected override void OnStart()
        {
            AddComponent(new SpriteRenderer("circle"));
        }
    }

    struct CollisionCategory
    {
        public static readonly int Player = 0b0001;
        public static readonly int Map = 0b0010;
    }
    struct CollisionMask
    {
        public static readonly int Player = CollisionCategory.Map;
        public static readonly int Map = CollisionCategory.Map | CollisionCategory.Player;
    }

    class Player2 : Actor
    {
        Sprite sprite;
        SpriteRenderer spriteRenderer;
        RigidBody body;
        Speaker speaker;
        float t = 0;

        public Player2(float x, float y, bool isDynamic)
        {
            Form.Position = new Float2(x, y);
            Form.Scale = new Float2(1f, 1f);
            body = RigidBody.CreateCircle(isDynamic, Form.Scale.x * 0.5f);
            body.Restitution = 0.3f;
            body.Friction = 0.5f;
            body.CategoryBits = CollisionCategory.Player;
            body.MaskBits = CollisionMask.Player;
        }


        protected override void OnStart()
        {
            spriteRenderer = new SpriteRenderer("circle");
            //AddComponent(spriteRenderer);
            var texRen = new TextRenderer("Hello", 100);
            texRen.SetLayer(1);
            AddComponent(texRen);
            //HolderScene.AddActor(new Player2(0, 0, false));
        }


        protected override void OnUpdate()
        {
            if (Input.IsPressed(Keys.Space))
            {
                body.ApplyImpulse(Float2.UnitY * -5f);
            }

            if (Input.IsKeyDown(Keys.Right))
            {
                body.Velocity.x = 1f;
                t += 0.01f;
                //Graphics.TakeScreenShot().Save("test.png");
            }
            if (Input.IsKeyDown(Keys.Left))
            {
                body.Velocity.x = -1f;
                t -= 0.01f;
            }
            Position = Float2.SmoothAccel(Float2.Zero, new Float2(5f, -3f), Float2.UnitY * 3f, t);
        }


        protected override void OnRender()
        {
        }


        protected override void OnCollide(CollisionEventArgs args)
        {
            //SceneUtil.ChangeScene(new NewScene());
        }


        protected override void OnDestroy()
        {
            speaker.Dispose();
        }
    }


    class Map : Actor
    {
        TilemapRenderer tilemapRenderer;
        SpriteRenderer bgRenderer;
        Tilemap map;
        RigidBody body;
        Animator animator;


        public Map()
        {
            //sprite = Assets.GetSprite("map");
            Form.Position = new Float2(-5f, 0f);
            Form.Scale = new Float2(1f);
            animator = new Animator();
            animator.AddAnimation(new int[] { 0, 1 }, "sample");
            animator.Play("sample", 3);
            var data = new int[] { -1, -1, -1,-1,-1,-1,-1,-1,-1,-1,1,
                                   -1, 1, -1,-1,-1,-1,-1,-1,-1,-1,1,
                                   8,  3,  7, 3, 3, 7, 7, 7, 7, 7,1,
                                   8,  -1,  1, 1, -1, -1, -1, -1, -1, 1,1};
            map = new Tilemap(data, Form.Scale.x, new Int2(11, 4));
            body = RigidBody.CreateTilemap(map, new Int2(0, 10));
            body.Friction = 0.9f;
            body.Restitution = 0f;
            body.CategoryBits = CollisionCategory.Map;
            body.MaskBits = CollisionMask.Map;
        }


        protected override void OnStart()
        {
            var sprite = Assets.Load<Sprite>("map");
            tilemapRenderer = new TilemapRenderer(map, sprite);
            bgRenderer = Assets.Load<Sprite>("bg").CreateRenderer();
            bgRenderer.LocalCoord = Float2.UnitX * 3f;
            AddComponent(body);
            AddComponent(tilemapRenderer);
            // HolderScene.TryGetActors<Player>(out var output);
        }


        protected override void OnUpdate()
        {
            if (Input.IsKeyDown(Keys.Up))
            {
                Camera.ViewScale -= 0.02f;
            }
            if (Input.IsKeyDown(Keys.Down))
            {
                Camera.ViewScale += 0.02f;
            }
            if (Input.IsKeyDown(Keys.W))
            {
                Camera.Position.y -= 0.02f;
            }
            if (Input.IsKeyDown(Keys.S))
            {
                Camera.Position.y += 0.02f;
            }
            if (Input.IsKeyDown(Keys.A))
            {
                Camera.Position.x -= 0.02f;
            }
            if (Input.IsKeyDown(Keys.D))
            {
                Camera.Position.x += 0.02f;
            }
            //Camera.ViewScale += 0.05f;
        }


        protected override void OnRender()
        {
            //Form.Position.x -= 0.01f;
            var form = new Transform(Float2.Zero, new Float2(10), 0);
            //Graphics.DrawSprite(bgSprite, 0, form);
            //Graphics.DrawTilemap(tilemap: map, sprite: sprite, form: Form);
        }


        protected override void OnDestroy()
        {
            // sprite.Release();
        }
    }


    class BG : Actor
    {
        SpriteRenderer bgRenderer;


        public BG()
        {
            Form.Scale = new Float2(20f, 10f);
        }


        protected override void OnStart()
        {
            bgRenderer = Assets.Load<Sprite>("bg").CreateRenderer();
            AddComponent(bgRenderer);
            // HolderScene.TryGetActors<Player>(out var output);
        }


        protected override void OnUpdate()
        {
            if (Input.IsPressed(Keys.Space))
            {
                //SceneUtil.ChangeScene(new NewScene());
            }
            //Camera.ViewScale += 0.05f;
        }


        protected override void OnRender()
        {
            //Graphics.DrawSprite(bgSprite, 0, form);
            //Graphics.DrawTilemap(tilemap: map, sprite: sprite, form: Form);
        }


        protected override void OnDestroy()
        {
            // sprite.Release();
        }
    }


    public class Controller : Component
    {
        protected override void OnUpdate(Actor actor)
        {
            MathUtil.Increase(ref actor.Form.Position.x, -3f, 3f, 100f);
        }
    }



    public class Tree : Actor
    {
        float rotSpeed { get; set; }

        public Tree(float rotSpeed)
        {
            this.rotSpeed = rotSpeed;
        }
        public Tree(float rotSpeed, bool x)
        {
            this.rotSpeed = rotSpeed;
        }


        protected override void OnStart()
        {
            var ren = Assets.Load<Sprite>("map").CreateRenderer();
            ren.UnitIndex = 5;
            AddComponent(ren);
            var rb = RigidBody.CreateCircle(true, 1f);
            rb.Velocity = Float2.One;
            rb.AngularVelocity = 10f;
            Console.WriteLine(rb.VelocityAt(Float2.UnitX));
            AddComponent(rb);
        }


        protected override void OnUpdate()
        {
            Form.Rotation += rotSpeed * Time.DeltaTime;
        }
    }



    public class FrameCounter : Actor
    {
        TextRenderer textRenderer;

        public FrameCounter()
        {
            textRenderer = new TextRenderer("0", 100);
            AddComponent(textRenderer);
            Position = new Float2(-1);
        }

        protected override void OnUpdate()
        {
            textRenderer.SetText(Time.FrameCount.ToString());
        }
    }
}
