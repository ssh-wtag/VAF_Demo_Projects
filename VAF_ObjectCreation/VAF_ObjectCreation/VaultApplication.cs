using MFiles.VAF.Common;
using MFiles.VAF.Core;
using MFilesAPI;
using System;
using System.Diagnostics;
using System.IO;
using VAF_ObjectCreation.Configurations;
using VAF_ObjectCreation.Services;

namespace VAF_ObjectCreation
{
    public class VaultApplication : ConfigurableVaultApplicationBase<Configuration>
    {
        #region Initialization

        private MFObjectOperation _objOp;
        private MFObjectFileOperation _objFileOp;

        protected override void InitializeApplication(Vault vault)
        {
            base.InitializeApplication(vault);

            _objOp = new MFObjectOperation(vault);
            _objFileOp = new MFObjectFileOperation(vault);
        }

        public override void StartOperations(Vault vaultPersistent)
        {
            try
            {
                base.StartOperations(vaultPersistent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion



        #region Implementation

        [EventHandler(MFEventHandlerType.MFEventHandlerAfterCheckOut)]
        public void OnAfterCheckInChanges(EventHandlerEnvironment env)
        {

            Vault vault = env.Vault;
            ObjVer objVer = env.ObjVer;

            CreateFile1(vault, objVer);
            //CreateFile2(vault, objVer);
            //CreateFile3(vault, objVer);

            return;
        }

        private void CreateFile1(Vault vault, ObjVer objVer)
        {
            // Step 1: Define the Type of Object that Is being Created -> int
            int objectType = (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument;

            // Step 2: Create a Collection of Properties
            // that the Created File Will Contain.
            var propertyValues = new PropertyValues();

            // Step 3: Create Properties with Values to Add to the Property Collection
            // Class -> Create Property with Definition then Set Values to that Definition
            var classProp = new PropertyValue { PropertyDef = (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefClass };

            classProp.Value.SetValue(
                MFDataType.MFDatatypeLookup,
                (int)MFBuiltInDocumentClass.MFBuiltInDocumentClassOtherDocument
                );

            // Name -> Create a New Property with a Definition then Set Data Type and Value to that Definition
            var nameProp = new PropertyValue { PropertyDef = (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefNameOrTitle };

            nameProp.Value.SetValue(
                MFDataType.MFDatatypeText,
                "My Created Document Using CreateNewObjectEx"
                );

            propertyValues.Add(-1, classProp);
            propertyValues.Add(-1, nameProp);

            // Step 4: Create a Source Object File from Either an Existing File
            // Or Create a New File Eniterly

            var sourceFiles = new SourceObjectFiles();

            //var file1 = new SourceObjectFile();
            //file1.SourceFilePath = MyFile1.txt";
            //file1.Title = "My Test Title";
            //file1.Extension = "txt";

            string textToWrite = "This is a custom made file made to be uploaded to M-Files";
            string filePath = "output.txt";
            File.WriteAllText(filePath, textToWrite);

            var file2 = new SourceObjectFile();
            file2.Title = "My Custom Test Title";
            file2.Extension = "txt";
            file2.SourceFilePath = filePath;

            //sourceFiles.Add(-1, file1);
            sourceFiles.Add(-1, file2);

            var isSingleFileDocument = objectType == (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument && sourceFiles.Count == 1;

            var createFile = vault.ObjectOperations.CreateNewObject(
                objectType,
                propertyValues,
                sourceFiles
                );

            createFile.Vault.ObjectOperations.CheckIn(createFile.ObjVer);

            return;
        }

        private void CreateFile2(Vault vault, ObjVer objVer)
        {
            int objectType = (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument;

            // Step 2: Create a Collection of Properties
            // that the Created File Will Contain.
            var propertyValues = new PropertyValues();

            var classProp = new PropertyValue { PropertyDef = (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefClass };

            classProp.Value.SetValue(
                MFDataType.MFDatatypeLookup,
                (int)MFBuiltInDocumentClass.MFBuiltInDocumentClassOtherDocument
                );

            var nameProp = new PropertyValue { PropertyDef = (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefNameOrTitle };

            nameProp.Value.SetValue(
                MFDataType.MFDatatypeText,
                "My Created Document Using CreateNewObjectEx"
                );

            propertyValues.Add(-1, classProp);
            propertyValues.Add(-1, nameProp);

            var sourceFiles = new SourceObjectFiles();

            //var file1 = new SourceObjectFile();
            //file1.SourceFilePath = "MyFile1.txt";
            //file1.Title = "My Test Title";
            //file1.Extension = "txt";

            string textToWrite = "This is a custom made file made to be uploaded to M-Files";
            string filePath = "output.txt";
            File.WriteAllText(filePath, textToWrite);

            var file2 = new SourceObjectFile();
            file2.Title = "My Custom Test Title";
            file2.Extension = "txt";
            file2.SourceFilePath = filePath;

            //sourceFiles.Add(-1, file1);
            sourceFiles.Add(-1, file2);

            var isSingleFileDocument = objectType == (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument && sourceFiles.Count == 1;

            var createFile = vault.ObjectOperations.CreateNewObjectEx(
                objectType,
                propertyValues,
                sourceFiles,
                SFD: isSingleFileDocument,
                CheckIn: true
                );

            return;
        }

        private void CreateFile3(Vault vault, ObjVer objVer)
        {
            return;
        }

        #endregion
    }
}