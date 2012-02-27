using System;

namespace SpermGame.Engine.Component {
    class CustomComponent<TAction> : Core.Component {
        private readonly TAction action;
        protected TAction Action {
            get { return this.action; }
        }

        public CustomComponent(TAction action) {
            if (action == null) {
                throw new ArgumentNullException("action");
            }

            this.action = action;
        }
    }
}