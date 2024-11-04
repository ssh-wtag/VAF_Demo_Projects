using MFilesAPI;

namespace VAF_ObjectCreation.Interfaces
{
    public interface IMFObjectFileOperation
    {
        /// <summary>
        /// Retrieves the object file associated with the specified ObjVer.
        /// </summary>
        /// <param name="objVer">The ObjVer of the object file to retrieve.</param>
        /// <returns>An ObjectFile containing information about the requested object file.</returns>
        ObjectFile GetFile(ObjVer objVer);



        /// <summary>
        /// Retrieves all object files associated with the specified object.
        /// </summary>
        /// <param name="objVer">The ObjVer of the object for which to retrieve files.</param>
        /// <returns>An array of ObjectFile objects representing all files related to the specified object.</returns>
        ObjectFile[] GetObjectFiles(ObjVer objVer);



        /// <summary>
        /// Downloads an M-Files object as a file to the specified file path.
        /// </summary>
        /// <param name="fileId">The identifier of the file to download.</param>
        /// <param name="fileVersion">The version of the file to download.</param>
        /// <param name="filePath">The path where the file will be saved.</param>
        void DownloadFile(int fileId, int fileVersion, string filePath);



        /// <summary>
        /// Retrieves the source object files for a specified object.
        /// </summary>
        /// <param name="objVer">The ObjVer of the object for which to retrieve source files.</param>
        /// <param name="filePath">The path where the source object files will be stored.</param>
        /// <returns>A collection of SourceObjectFile objects associated with the specified object.</returns>
        SourceObjectFiles GetSourceObjectFile(ObjVer objVer, string filePath);
    }
}
