using System.Collections.Generic;
using System.Text.Json;


namespace Broad
{
    struct Export
    {
        public string format { get; set; }
        public string target { get; set; }
    }

    struct Editorsettings
    {
        public Export export { get; set; }
    }

    struct Property
    {
        public string name { get; set; }
        public string type { get; set; }
        public JsonElement value { get; set; }
    }

    struct Object
    {
        public int gid { get; set; }
        public float height { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public List<Property> properties { get; set; }
        public float rotation { get; set; }
        public string type { get; set; }
        public bool visible { get; set; }
        public float width { get; set; }
        public float x { get; set; }
        public float y { get; set; }
    }

    struct Layer
    {
        public List<int> data { get; set; }
        public int height { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int opacity { get; set; }
        public string type { get; set; }
        public bool visible { get; set; }
        public int width { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public string draworder { get; set; }
        public List<Object> objects { get; set; }
    }

    struct Tileset
    {
        public int firstgid { get; set; }
        public string source { get; set; }
    }

    class Root
    {
        public int compressionlevel { get; set; }
        public Editorsettings editorsettings { get; set; }
        public int height { get; set; }
        public bool infinite { get; set; }
        public List<Layer> layers { get; set; }
        public int nextlayerid { get; set; }
        public int nextobjectid { get; set; }
        public string orientation { get; set; }
        public string renderorder { get; set; }
        public string tiledversion { get; set; }
        public int tileheight { get; set; }
        public List<Tileset> tilesets { get; set; }
        public int tilewidth { get; set; }
        public string type { get; set; }
        public string version { get; set; }
        public int width { get; set; }
    }
}