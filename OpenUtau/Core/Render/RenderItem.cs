﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenUtau.Core.USTx;

namespace OpenUtau.Core.Render
{
    class RenderItem
    {
        // For resampler
        public string RawFile;
        public int NoteNum;
        public int Velocity;
        public int Volume;
        public string StrFlags;
        public List<int> PitchData;
        public int RequiredLength;
		public double Tempo;
        public UOto Oto;

        // For connector
        public double SkipOver;
        public double PosMs;
        public double DurMs;
        public List<ExpPoint> Envelope;

        // Sound data
        public CachedSound Sound = null;

        public RenderItem() { }

        public uint HashParameters()
        {
            return Lib.xxHash.CalcStringHash(RawFile + " " + GetResamplerExeArgs());
        }

        public string GetResamplerExeArgs()
        {
            // fresamp.exe <infile> <outfile> <tone> <velocity> <flags> <offset> <length_req>
            // <fixed_length> <endblank> <volume> <modulation> <pitch>
            return string.Format(
                "{0} {1:D} {2} {3} {4:D} {5} {6} {7:D} {8:D} {9} {10}",
                MusicMath.GetNoteString(NoteNum),
                Velocity,
                StrFlags,
                Oto.Offset,
                RequiredLength,
                Oto.Consonant,
                Oto.Cutoff,
                Volume,
                0,
				Tempo,
                String.Join(",",PitchData));
        }
    }
}
