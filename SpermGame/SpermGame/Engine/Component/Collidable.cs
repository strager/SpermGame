﻿using SpermGame.Engine.Core;
using SpermGame.Engine.Physics;

namespace SpermGame.Engine.Component {
    static class Collidable {
        public static readonly Property<Body> Body = new Property<Body>();

        public static Body GetEffectiveBody(Entity e) {
            var body = e.Get(Body);
            if (body == null) {
                return null;
            }

            var position = e.Get(Located.Position);
            return body.Offset(position);
        }
    }
}
