using MFilesAPI;
using System;
using System.IO;
using System.Linq;
using VAF_ConfigPropertyMapping.Helper;
using VAF_ConfigPropertyMapping.Interfaces;

namespace VAF_ConfigPropertyMapping.Services
{
    public class MFObjectFileOperation : IMFObjectFileOperation
    {
        #region Initialization

        private readonly Vault _vault;
        private const int OBJECT_FILES_LENGTH = 0;
        private const int OBJECT_FILE_INDEX = 0;
        private const int SOURCE_OBJECT_FILE_INDEX = -1;

        public MFObjectFileOperation(Vault vault)
        {
            _vault = vault;
        }

        #endregion



        #region Implementation

        public ObjectFile GetFile(ObjVer objVer)
        {
            var objectFiles = GetObjectFiles(objVer);

            return objectFiles?.Length > OBJECT_FILES_LENGTH ? objectFiles[OBJECT_FILE_INDEX] : null;
        }



        public ObjectFile[] GetObjectFiles(ObjVer objVer)
        {
            var objectFiles = _vault.ObjectFileOperations
                .GetFiles(objVer)?
                .Cast<ObjectFile>()?
                .ToArray();

            return objectFiles;
        }



        public void DownloadFile(int fileId, int fileVersion, string filePath)
        {
            _vault.ObjectFileOperations.DownloadFile(fileId, fileVersion, filePath);
        }



        public SourceObjectFiles GetSourceObjectFile(ObjVer objVer, string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception($"No file found in the provided filepath. Filepath = [{filePath}]");
            }

            var file = GetFile(objVer);

            if (file != null)
            {
                var fileName = file.Title;
                var fileExtension = file.Extension;
                Assert.IsNotNullOrEmpty(fileName);
                Assert.IsNotNullOrEmpty(fileExtension);

                var sourceObjectFiles = new SourceObjectFiles
                {
                    {
                        SOURCE_OBJECT_FILE_INDEX,
                        new SourceObjectFile()
                        {
                            Extension = fileExtension,
                            Title = fileName,
                            SourceFilePath = filePath
                        }
                    }
                };

                return sourceObjectFiles;
            }

            return null;
        }

        #endregion
    }
}
