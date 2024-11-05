using MFilesAPI;
using System.Collections.Generic;
using VAF_ConfigPropertyMapping.Models;

namespace VAF_ConfigPropertyMapping.Interfaces
{
    public interface IMFObjectOperation
    {
        /// <summary>
        /// Creates a new object based on the specified source object.
        /// </summary>
        /// <param name="objectArgs">The arguments required for creating a new object.</param>
        /// <returns>The latest version of the newly created object.</returns>
        ObjVer CreateNewObject(NewObjectArgs objectArgs);



        /// <summary>
        /// Checks in the specified object and returns the latest version.
        /// </summary>
        /// <param name="objVer">The version of the object to check in.</param>
        /// <returns>The latest object version of the checked-in object.</returns>
        ObjVer CheckInObject(ObjVer objVer);



        /// <summary>
        /// Retrieves the latest version of the specified object.
        /// </summary>
        /// <param name="objVer">The version of the object for which to retrieve the latest version.</param>
        /// <returns>The latest object version.</returns>
        ObjVer GetLatestObjVer(ObjVer objVer);



        /// <summary>
        /// Retrieves the class of the specified M-Files object. Object classes are used for categorizing objects.
        /// </summary>
        /// <param name="objVer">The version of the object for which to retrieve the class.</param>
        /// <returns>The class of the specified object.</returns>
        ObjectClass GetMFilesObjectClass(ObjVer objVer);



        /// <summary>
        /// Retrieves all placeholder texts associated with the specified object.
        /// </summary>
        /// <param name="objVer">The version of the object from which to retrieve placeholder texts.</param>
        /// <param name="text">The text used to identify the placeholders.</param>
        /// <returns>A list of all placeholder texts within the specified object.</returns>
        IList<string> GetPlaceholderValuesAsText(ObjVer objVer, string text);



        /// <summary>
        /// Retrieves all collection members within a specified document collection.
        /// </summary>
        /// <param name="objVer">The version of the document collection to query.</param>
        /// <returns>An enumeration of object versions for all collection members.</returns>
        IEnumerable<ObjectVersion> GetCollectionMembersFromDocumentCollection(ObjVer objVer);



        /// <summary>
        /// Retrieves the permissions associated with the specified object.
        /// </summary>
        /// <param name="objVer">The version of the object for which to retrieve permissions.</param>
        /// <returns>The permissions associated with the specified object.</returns>
        ObjectVersionPermissions GetMFilesObjectPermissions(ObjVer objVer);
    }
}
