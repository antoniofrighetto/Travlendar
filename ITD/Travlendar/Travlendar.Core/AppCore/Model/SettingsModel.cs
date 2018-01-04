using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Travlendar.Core.AppCore.Model
{
    [JsonObject(MemberSerialization.OptOut)]

    public class Settings
    {

        public int userId { get; set; }


        public bool car { get; set; }


        public bool bike { get; set; }


        public bool publicTransport { get; set; }


        public bool minimizeCarbonFootPrint { get; set; }


        public bool lunchBreak { get; set; }


        public TimeSpan timeBreak { get; set; }


        public TimeSpan timeInterval { get; set; }

    }
}
