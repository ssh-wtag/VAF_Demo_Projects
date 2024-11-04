using MFilesAPI;

namespace VAF_ObjectCreation.Models
{
    public class NewObjectArgs
    {
        public int ObjectType { get; set; }
        public PropertyValues Properties { get; set; }
        public SourceObjectFiles SourceObjectFile { get; set; } = null;
        public bool IsSingleFileDocument { get; set; } = false;
        public bool CheckIn { get; set; } = true;
        public AccessControlList AccessControlList { get; set; } = null;
    }
}
