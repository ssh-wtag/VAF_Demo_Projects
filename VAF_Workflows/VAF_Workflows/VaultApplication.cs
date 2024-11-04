using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using MFiles.VAF.Common;
using MFiles.VAF.Core;
using MFilesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace VAF_Workflows
{
    public class VaultApplication : ConfigurableVaultApplicationBase<Configuration>
    {
        #region Initialization

        protected override void InitializeApplication(Vault vault)
        {
            base.InitializeApplication(vault);
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

        [EventHandler(MFEventHandlerType.MFEventHandlerAfterCheckInChanges)]
        public void OnCheckIn(EventHandlerEnvironment env)
        {
            Vault vault = env.Vault;
            ObjVer objVer = env.ObjVer;

            ObjectVersionWorkflowState workflowState = vault.ObjectPropertyOperations.GetWorkflowState(objVer);

            if(workflowState.State.TypedValue.DisplayValue.ToString() == "Rejected")
            {
                ObjectFile file = GetFile(vault, objVer);

                StoreFileContent(vault, objVer, file);

                Upload(file);
            }

            return;
        }


        private void Upload(ObjectFile file)
        {
            GoogleCredential credential;
            using (var credStream = new FileStream("UploadKey.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(credStream).CreateScoped(DriveService.Scope.DriveFile);
            }

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Drive API Service Account Example",
            });

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = "Rejected_" + file.Title,
                Parents = new List<string> { "PLACEHOLDER" }
            };


            // Upload the file
            using (var stream = new FileStream("output.txt", FileMode.Open))
            {
                var request = service.Files.Create(fileMetadata, stream, "text/plain");
                request.Fields = "id";
                request.Upload();
                var createdFile = request.ResponseBody;
                Debug.WriteLine("File ID: " + createdFile.Id);
            }
        }

        private void StoreFileContent(Vault vault, ObjVer objVer, ObjectFile file)
        {
            vault.ObjectFileOperations.DownloadFile(file.ID, file.Version, "output.txt");

            return;
        }

        private ObjectFile GetFile(Vault vault, ObjVer objVer)
        {
            var objectFiles = GetObjectFiles(vault, objVer);

            return objectFiles?.Length > 0 ? objectFiles[0] : null;
        }

        private ObjectFile[] GetObjectFiles(Vault vault, ObjVer objVer)
        {
            var objectFiles = vault.ObjectFileOperations
                .GetFiles(objVer)?
                .Cast<ObjectFile>()?
                .ToArray();

            return objectFiles;
        }

        #endregion
    }
}