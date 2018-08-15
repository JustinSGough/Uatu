using DrwgTronics.Uatu.Models;

namespace DrwgTronics.Uatu.Components
{
    /// <summary>
    /// Report completed line count of a file.
    /// </summary>
    public class LineCountProgress
    {
        public FileEvent FileEvent { get; set; }
        public int Count { get; set; }
        public LineCountStatus Status { get; set; }
        public string Note { get; set; }

        public LineCountProgress(
            FileEvent fileEvent, 
            int count = -1, 
            LineCountStatus status = LineCountStatus.Success, 
            string note = null)
        {
            FileEvent = fileEvent;
            Count = count;
            Status = status;
            Note = note;
        }

        public LineCountProgress CopyTo(LineCountProgress copy)
        {
            copy.FileEvent = FileEvent;
            copy.Count     = Count;
            copy.Status    = Status;
            copy.Note      = Note;
            return copy;
        }
    }
}
