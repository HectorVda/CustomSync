using System.Collections.Generic;

namespace Model
{
    public class Config
    {
        public IList<string> SourcePaths { get; set; }
        public IList<string> DestinationPath { get; set; }
        public bool BackupOriginalFolder { get; set; }
        public bool MoveFiles { get; set; }


    }
}
