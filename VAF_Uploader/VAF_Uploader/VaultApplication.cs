using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MFiles.VAF;
using MFiles.VAF.AppTasks;
using MFiles.VAF.Common;
using MFiles.VAF.Configuration;
using MFiles.VAF.Core;
using MFilesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace VAF_Uploader
{
    /// <summary>
    /// The entry point for this Vault Application Framework application.
    /// </summary>
    /// <remarks>Examples and further information available on the developer portal: http://developer.m-files.com/. </remarks>
    public class VaultApplication : ConfigurableVaultApplicationBase<Configuration>
    {
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

        [EventHandler(MFEventHandlerType.MFEventHandlerBeforeCheckInChanges)]
        public void OnCheckIn(EventHandlerEnvironment env)
        {
            Vault vault = env.Vault;

            if (env.ObjVer.Type == (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument)
            {
                try
                {
                    var properties = vault.ObjectPropertyOperations.GetProperties(env.ObjVer);
                    string currentName = string.Empty;
                    string filePath = string.Empty;

                    foreach (IPropertyValue propertyValue in properties)
                    {
                        if (propertyValue.PropertyDef == (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefNameOrTitle)
                        {
                            currentName = propertyValue.GetValueAsText(false, false, false, false, false, false);
                            var fileVersion = vault.ObjectFileOperations.GetLatestFileVersion(env.ObjVer.ID, true);

                            Upload();

                            break;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error renaming file: {ex.Message}");
                }
            }
        }




        private void Upload()
        {
            GoogleCredential credential;

            using (var stream = new FileStream("UploadKey.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(DriveService.Scope.DriveFile);
            }

            // Create Drive API service
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Drive API Service Account Example",
            });

            // File metadata
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = "myfile.txt",
                Parents = new List<string> { "PLACEHOLDER" }
            };


            // Upload the file
            using (var stream = new FileStream("UploadThisFile.txt", FileMode.Open))
            {
                var request = service.Files.Create(fileMetadata, stream, "text/plain");
                request.Fields = "id";
                request.Upload();
                var file = request.ResponseBody;
                Debug.WriteLine("File ID: " + file.Id);
            }

        }
    }
}