using System;
using System.Net;
using System.Drawing;
using Newtonsoft.Json;

namespace Painter
{
    /// <summary>
    /// Provides functionality for connecting and interacting with a Philips Hue environment.
    /// </summary>
    class Hue
    {
        /// <summary>
        /// Prepares a hue bridge to allow the application to interact with it - needs to be run once per bridge.
        /// </summary>
        /// <param name="bridgeip">IP address of the bidge to connect to.</param>
        /// <param name="uname">Username for the paplication to use.</param>
        /// <returns>Boolean value depending on whether registration was a success.</returns>
        public bool hueconfigure(string bridgeip, string uname)
        {
            var client = new WebClient();
            Logging Log = new Logging();
            //our uri to perform registration
            var uri = new Uri(string.Format("http://{0}/api", bridgeip));

            //create our registration object, along with username and description
            var reg = new
            {
                username = uname,
                devicetype = Properties.Resources.APPLICATION_TITLE
            };

            var jsonObj = JsonConvert.SerializeObject(reg);


            //decide what to do with the response we get back from the bridge
            client.UploadStringCompleted += (o, args) =>
            {
                try
                {
                    Log.Write(Properties.Resources.LOG_HUE_BRIDGE_RESPONSE + " " + args.Result);
                }
                catch (Exception ex)
                {
                    Log.Write(Properties.Resources.LOG_HUE_BRIDGE_EXCEPTION + " " + ex.Message);
                }
            };

            //Invoke a POST to the bridge
            client.UploadStringAsync(uri, jsonObj);
            //WriteLog("Connection request sent (hope you pressed the button!");

            return true;
        }


        /// <summary>
        /// Sends a state command to a hue bridge.
        /// </summary>
        /// <param name="bridgeip">IP address of the hue bridge.</param>
        /// <param name="username">Username to authenticate with.</param>
        /// <param name="color">A Color object representing the color to set.</param>
        /// <param name="light">An int value representing the light ID to change.</param>
        /// <param name="transitionspeed">The speed at which the transition should occur.</param>
        public void hueset(string bridgeip, string username, Color color, int light, int transitionspeed)
        {
            //Get the HSV Value from the currently selected color
            //var hsv = LightColorSlider.Color.GetHSV();

            //build our State object
            Logging Log = new Logging();

            var state = new
            {
                on = true,
                hue = (int)(color.GetHue() * 182.04), //we convert the hue value into degrees by multiplying the value by 182.04
                sat = (int)(color.GetSaturation() * 254),
                transitiontime = (int)transitionspeed
            };

            //convert it to json:
            var jsonObj = JsonConvert.SerializeObject(state);

            //set the api url to set the state
            var uri = new Uri(string.Format("http://{0}/api/{1}/lights/{2}/state", bridgeip, username, light));

            var client = new WebClient();

            //decide what to do with the response we get back from the bridge
            client.UploadStringCompleted += (o, args) =>
            {
                try
                {
                    Log.Write(Properties.Resources.LOG_HUE_BRIDGE_RESPONSE + " " + args.Result);
                }
                catch (Exception ex)
                {
                    Log.Write(Properties.Resources.LOG_HUE_BRIDGE_EXCEPTION + " " + ex.Message);
                }

            };

            //Invoke the PUT method to set the state of the bulb
            client.UploadStringAsync(uri, "PUT", jsonObj);
        }

        private void SendCommand(string command)
        {

        }
    }
}
