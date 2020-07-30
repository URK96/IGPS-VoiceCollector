using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IGPS.Models
{
    public class VoiceDataItem
    {
        public int Section { get; set; }
        public int Number { get; set; }
        public string VoiceText { get; set; }
        public bool IsRecorded { get; set; }
        public bool IsUploaded { get; set; }

        public VoiceDataItem(DataRow dr)
        {
            Section = (int)dr["Section"];
            Number = (int)dr["No."];
            VoiceText = (string)dr["VoiceText"];
            IsRecorded = (bool)dr["IsRecorded"];
            IsUploaded = (bool)dr["IsUploaded"];
        }
    }
}
