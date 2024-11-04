using MFiles.VAF;
using MFiles.VAF.AppTasks;
using MFiles.VAF.Common;
using MFiles.VAF.Configuration;
using MFiles.VAF.Core;
using MFilesAPI;
using System;
using System.Diagnostics;

namespace VAF_Renamer
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
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
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

                    foreach (IPropertyValue propertyValue in properties)
                    {
                        if (propertyValue.PropertyDef == (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefNameOrTitle)
                        {
                            currentName = propertyValue.GetValueAsText(false, false, false, false, false, false);

                            if (currentName.StartsWith("demo_", StringComparison.OrdinalIgnoreCase))
                            {
                                string newName = "Rename_" + currentName;

                                var newPropertyValue = new PropertyValue
                                {
                                    PropertyDef = (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefNameOrTitle,
                                    TypedValue = new TypedValue { Value = newName }
                                };
                                vault.ObjectPropertyOperations.SetProperty(env.ObjVer, newPropertyValue);

                                vault.ObjectOperations.CheckIn(env.ObjVer);
                            }

                            break;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error renaming file: {ex.Message}");
                }
            }
        }

    }
}