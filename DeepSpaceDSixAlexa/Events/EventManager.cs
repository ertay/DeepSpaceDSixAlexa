using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSpaceDSixAlexa.Events
{
    public class EventManager
    {

        /// <summary>
        /// Dictionary that stores all events and the methods that fire.
        /// </summary>
        private Dictionary<string, List<Action<IEvent>>> _register;

        public EventManager()
        {
            _register = new Dictionary<string, List<Action<IEvent>>>();
        }
        


        /// <summary>
        /// Registers an event with a given name and method to fire when triggered.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        public void On(string name, Action<IEvent> callback)
        {
            if (!_register.ContainsKey(name))
            {
                _register[name] = new List<Action<IEvent>>();
            }
            _register[name].Add(callback);
        }

        /// <summary>
        /// Triggers the event, by it's name, default event object will be passed to callback
        /// </summary>
        /// <param name="name"></param>
        public void Trigger(string name)
        {
            Trigger(name, new DefaultEvent());
        }

        /// <summary>
        /// Triggers the event by it's name, "data" event object will be passed to callback
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public void Trigger(string name, IEvent data)
        {
            if (_register.ContainsKey(name))
            {
                foreach (Action<IEvent> callback in _register[name])
                {
                    callback(data);
                }
            }
        }

    }

    public interface IEvent { }
    public class DefaultEvent : IEvent { 
        public string Message { get; set; }
        public DefaultEvent() { }

        public DefaultEvent(string message)
        {
            Message = message;
        }
    }
}
