using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Hardware
{
    /// <summary>
    /// Define a frame detector for data transfered with a <see cref="StreamResource"/>
    /// </summary>
    public class FrameDetector
    {
        /// <summary>
        /// The terminator sequence of the frame
        /// </summary>
        public byte[] TerminatorSequence { get; internal set; }

        private readonly ConcurrentQueue<byte[]> queue;
        private readonly List<byte> buffer;

        public FrameDetector(byte[] terminatorSequence)
        {
            queue = new ConcurrentQueue<byte[]>();
            buffer = new List<byte>();

            TerminatorSequence = terminatorSequence;
        }

        /// <summary>
        /// Try to get the last retrieved frame data
        /// </summary>
        /// <param name="data">The data retrieved</param>
        /// <returns>The operation result</returns>
        public bool TryGet(out byte[] data)
        {
            bool result = queue.TryDequeue(out data);
            return result;
        }

        /// <summary>
        /// Add new bytes to the frame
        /// </summary>
        /// <param name="data">The data to add</param>
        public void Add(byte[] data)
        {
            if (DetectFrame(data, out int position))
            {
                buffer.AddRange(data.Skip(TerminatorSequence.Length).Take(position)); // Enqueue the byte up until terminator sequence
                queue.Enqueue(buffer.ToArray());

                buffer.Clear(); // Then take the remaining bytes on data
                buffer.AddRange(data.Skip(position + TerminatorSequence.Length));
            }
            else
            {
                buffer.AddRange(data); // Otherwise simply add data
            }
        }

        /// <summary>
        /// Search for a <see cref="TerminatorSequence"/> inside a <see cref="byte"/> array <paramref name="data"/>
        /// </summary>
        /// <param name="data">The array in which find</param>
        /// <param name="position">The position of the <see cref="TerminatorSequence"/></param>
        /// <returns><see langword="true"/> if<see cref="TerminatorSequence"/> is found in <paramref name="data"/>, <see langword="false"/> otherwise</returns>
        protected virtual bool DetectFrame(byte[] data, out int position)
        {
            int maxFirstCharSlot = data.Length - TerminatorSequence.Length + 1;
            for (int i = 0; i < maxFirstCharSlot; i++)
            {
                if (data[i] != TerminatorSequence[0]) // Compare only first byte
                {
                    continue;
                }

                // Found a match on first byte, now try to match rest of the pattern
                for (int j = TerminatorSequence.Length - 1; j >= 1; j--)
                {
                    if (data[i + j] != TerminatorSequence[j])
                    {
                        break;
                    }

                    if (j == 1)
                    {
                        position = i;
                        return true;
                    }
                }
            }

            position = 0;
            return false;
        }
    }
}
