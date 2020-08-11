using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace IGPS.Models
{
    public class SectionItem
    {
        public int Section { get; set; }
        public int Count { get; set; }
        public float Progress { get; set; }

        public SectionItem(int key)
        {
            Section = key;
            Count = AppEnvironment.dataService.voiceTextData[key].Length;
            Progress = 0;
        }
    }

    public class ChapterItem
    {
        public int Section { get; set; }
        public int Chapter { get; set; }
        public int Count { get; set; }
        public float Progress { get; set; }
        public (int start, int end) Range { get; set; }

        public ChapterItem(int section, int index, int start, int end)
        {
            Section = section;
            Range = (start, end);
            Chapter = index;
            Count = end - start + 1;
            Progress = 0;
        }
    }
}
