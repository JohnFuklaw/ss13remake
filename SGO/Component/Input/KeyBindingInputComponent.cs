﻿using System.Collections.Generic;
using GameObject;
using Lidgren.Network;
using SGO.Events;
using SS13_Shared;
using SS13_Shared.GO;

namespace SGO
{
    /// <summary>
    /// This class recieves keypresses from the attached client and forwards them to other components.
    /// </summary>
    public class KeyBindingInputComponent : Component
    {
        public override void HandleNetworkMessage(IncomingEntityComponentMessage message, NetConnection client)
        {
            var keyFunction = (BoundKeyFunctions) message.MessageParameters[0];
            var keyState = (BoundKeyState) message.MessageParameters[1];

            Owner.SendMessage(this, ComponentMessageType.BoundKeyChange, keyFunction, keyState);
            Owner.RaiseEvent(new BoundKeyChangeEventArgs{KeyFunction = keyFunction, KeyState = keyState, Actor = Owner});
            var boolState = keyState == BoundKeyState.Down;

            SetKeyState(keyFunction, boolState);
        }
        
        private readonly Dictionary<BoundKeyFunctions, bool> _keyStates = new Dictionary<BoundKeyFunctions, bool>();
        
        public KeyBindingInputComponent()
        {
            Family = ComponentFamily.Input;
            _keyStates = new Dictionary<BoundKeyFunctions, bool>();
        }

        protected void SetKeyState(BoundKeyFunctions k, bool state)
        {
            // Check to see if we have a keyhandler for the key that's been pressed. Discard invalid keys.
            _keyStates[k] = state;
        }

        public bool GetKeyState(BoundKeyFunctions k)
        {
            if (_keyStates.ContainsKey(k))
                return _keyStates[k];
            return false;
        }
    }
}