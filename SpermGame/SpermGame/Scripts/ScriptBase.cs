using System;
using Microsoft.Xna.Framework.Content;
using SpermGame.Engine.Core;

namespace SpermGame.Scripts {
    abstract class ScriptBase : IDisposable {
        private bool contentLoaded = false;

        public void LoadContent(ContentManager content) {
            if (this.contentLoaded) {
                throw new InvalidOperationException("Content already loaded");
            }

            this.LoadContentImpl(content);

            this.contentLoaded = true;
        }

        protected virtual void LoadContentImpl(ContentManager content) {
            // Override me
        }

        public void UnloadContent() {
            if (!this.contentLoaded) {
                throw new InvalidOperationException("Content not loaded");
            }

            this.UnloadContentImpl();

            this.contentLoaded = false;
        }

        protected virtual void UnloadContentImpl() {
            // Override me
        }

        public abstract void Execute(EntityCollection entities);

        public virtual void Dispose() {
            // Screw the disposable pattern
            if (!this.contentLoaded) {
                this.UnloadContent();
            }
        }
    }
}
