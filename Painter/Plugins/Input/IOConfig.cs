using System;
using System.Collections.Generic;

namespace Painter
{

    /// <summary>
    /// IOConfig is a configuration object containing other objects that describe an entire application wide configuration of detction settings, input configurations and other pieces.
    /// </summary>
    public class IOConfig
    {

        /// <summary>
        /// A collection of Input objects that collectively describes an IO config for the application
        /// </summary>
        [Serializable]
        public class InputConfig
            
        {
            public string Name { get; set; }
            public string Description { get; set; }

            public int InputCount { get; set; }
            public List<Input> Inputs { get; set; }
            public DetectionSettings Settings { get; set; }
        }

    }


    /// <summary>
    /// An object that describes a configured input region.
    /// </summary>
    [Serializable]
    public class Input
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int StartX { get; set; }
        public int EndX { get; set; }
        public int StartY { get; set; }
        public int EndY { get; set; }
        public int TargetID { get; set; }
    }

    /// <summary>
    /// An object containing setting that configure Detection activities.
    /// </summary>
    [Serializable]
    public class DetectionSettings
    {
        public bool ShowPreview { get; set; }
        public bool DisableFormDrawing { get; set; }
        public int SampleWidth { get; set; }
        public int SampleAccuracy { get; set; }
        public int SampleInterval { get; set; }
        public bool ConserveMemory { get; set; }
    }

}
