using System;

namespace Horror
{
    public struct AudioSignalKey
    {
        public static AudioSignalKey initialize = new AudioSignalKey(-1, null);

        public int Value;
        public AudioSignalSO AudioCue;

        internal AudioSignalKey(int value, AudioSignalSO audioCue)
        {
            Value = value;
            AudioCue = audioCue;
        }

        public override bool Equals(Object obj)
        {
            return obj is AudioSignalKey x && Value == x.Value && AudioCue == x.AudioCue;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ AudioCue.GetHashCode();
        }
        public static bool operator ==(AudioSignalKey x, AudioSignalKey y)
        {
            return x.Value == y.Value && x.AudioCue == y.AudioCue;
        }
        public static bool operator !=(AudioSignalKey x, AudioSignalKey y)
        {
            return !(x == y);
        }
    }
}
