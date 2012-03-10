using SpermGame.Engine.Core;

namespace SpermGame.Scripts {
    class Properties {
        public static readonly Property<bool> DestroysEnemies = new Property<bool>(false);
        public static readonly Property<bool> IsPowerup = new Property<bool>(false);
        public static readonly Property<float> Damage = new Property<float>(1.0f);
        public static readonly Property<uint> Score = new Property<uint>(0);
        public static readonly Property<uint> Points = new Property<uint>(0);
        public static readonly Property<Entity> Owner = new Property<Entity>();
        public static readonly Property<uint> Milliseconds = new Property<uint>();
    }
}
