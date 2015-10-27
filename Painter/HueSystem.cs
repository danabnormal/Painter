using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Painter
{
    class HueSystem
    {
        private string _hubip;
        private string _hubusername;
        private List<Light> _lights = new List<Light>();
        private Exception _result;

        /// <summary>
        /// The Light object represents an individual light in the system and can be used to perform basic functions, such as on/off.
        /// </summary>
        public class Light
        {
            int _number;
            string _name;
            bool _on;
            bool _off;

            public int Number {
                get
                {
                    return _number;
                }
                set
                {
                    _number = value;
                }
            }

            public string Name
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                }
            }

            public bool IsOn
            {
                get
                {
                    return _on;
                }
                set
                {
                    _on = value;
                }
            }

            public bool IsOff
            {
                get
                {
                    return _off;
                }
                set
                {
                    _off = value;
                }
            }

        }

        /// <summary>
        /// A collection of Light objects that exist in the system.
        /// </summary>
        public List<Light> Lights { get; set; }
        
        /// <summary>
        /// IP address of the hub with which to communicate.
        /// </summary>
        public string HubIPAddress
        {
            get
            {
                return _hubip;
            }

            set
            {
                _hubip = value;
            }
        }

        /// <summary>
        /// The Username used to talk with the Bridge.
        /// </summary>
        public string HubUserName
        {
            get
            {
                return _hubusername;
            }

            set
            {
                _hubusername = value;
            }
        }

        /// <summary>
        /// Public method for sending commands to the system
        /// </summary>
        /// <param name="Command">A Command to be sent to the system.</param>
        /// <param name="Light">A Light object to interact with.</param>
        /// <param name="Value">The value to be sent to the target, if appropriate.</param>
        /// <returns></returns>
        public void SendCommand(Commands Command, Light Light, int Value=0)
        {
            switch (Command)
            {
                case Commands.AllOff:
                    break;
                case Commands.AllOn:
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// Returns an Exception object detailing success/failure details of any commands issued.
        /// </summary>
        public Exception Result
        {
            get
            {
                return _result;
            }
            private set
            {
                _result = value;
            }
        }

        /// <summary>
        /// A predefined set of commands that can be sent to the system.
        /// </summary>
        public enum Commands
        {
            AllOff = 1,
            AllOn = 2,

            RegisterApplication = 100,

            SendColor =200,
            SendCommand=201,
            
            Custom=500
        }
    }
}
