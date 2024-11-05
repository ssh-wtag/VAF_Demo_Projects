using MFiles.VAF.Common;
using MFilesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAF_ConfigPropertyMapping.Helper;
using VAF_ConfigPropertyMapping.Interfaces;
using VAF_ConfigPropertyMapping.Models;

namespace VAF_ConfigPropertyMapping.Services
{
    public class MFObjectOperation : IMFObjectOperation
    {
        #region Initialization

        private readonly Vault _vault;

        public MFObjectOperation(Vault vault)
        {
            _vault = vault;
        }

        #endregion



        #region Implementation

        public ObjVer CreateNewObject(NewObjectArgs objArgs)
        {
            int newObjVerId = _vault.ObjectOperations.CreateNewObjectExQuick
                            (
                                objArgs.ObjectType,
                                objArgs.Properties,
                                objArgs.SourceObjectFile,
                                objArgs.IsSingleFileDocument,
                                objArgs.CheckIn,
                                objArgs.AccessControlList
                            );

            var newObjId = new ObjID
            {
                Type = objArgs.ObjectType,
                ID = newObjVerId
            };

            var newObjVer = _vault.ObjectOperations.GetLatestObjVer(newObjId, AllowCheckedOut: true);

            return newObjVer;
        }



        public ObjVer CheckInObject(ObjVer objVer)
        {
            var latestObjectVer = GetLatestObjVer(objVer);
            var objVerEx = new ObjVerEx(_vault, latestObjectVer);
            objVerEx.CheckIn();

            return GetLatestObjVer(objVer);
        }



        public ObjVer GetLatestObjVer(ObjVer objVer)
        {
            if (objVer != null)
            {
                var objId = new ObjID
                {
                    Type = objVer.Type,
                    ID = objVer.ID
                };

                var latestObjVer = _vault.ObjectOperations.GetLatestObjVer(objId, AllowCheckedOut: true);
                return latestObjVer;
            }

            return objVer;
        }



        public ObjectClass GetMFilesObjectClass(ObjVer objVer)
        {
            Assert.ArgumentIsNotNull(objVer, nameof(objVer));

            var objPropValue = _vault.ObjectPropertyOperations.GetProperty(objVer, (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefClass);
            Assert.IsNotNull(objPropValue, $"{nameof(objPropValue)} cannot be null.");
            Assert.IsNotNull(objPropValue.Value, $"M-Files class property value cannot be null.");

            var classId = objPropValue.Value.GetLookupID();
            var objClass = _vault.ClassOperations.GetObjectClass(classId);

            return objClass;
        }



        public IList<string> GetPlaceholderValuesAsText(ObjVer objVer, string text)
        {
            var objVerEx = new ObjVerEx(_vault, objVer);
            var list = objVerEx.ExpandPlaceholders(text);

            return list?.GetExpandedValues<string>()?.ToList();
        }



        public IEnumerable<ObjectVersion> GetCollectionMembersFromDocumentCollection(ObjVer objVer)
        {
            return _vault.ObjectOperations.GetCollectionMembers(objVer)?.Cast<ObjectVersion>();
        }



        public ObjectVersionPermissions GetMFilesObjectPermissions(ObjVer objVer)
        {
            return _vault.ObjectOperations.GetObjectPermissions(objVer);
        }

        #endregion
    }
}
