using MFiles.VAF.Common;
using MFiles.VAF.Core;
using MFilesAPI;
using System;
using System.Diagnostics;
using VAF_ConfigPropertyMapping.Configurations;
using VAF_ConfigPropertyMapping.Services;

namespace VAF_ConfigPropertyMapping
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

        [EventHandler(MFEventHandlerType.MFEventHandlerBeforeCheckInChanges)]
        public void OnBeforeCheckInChanges(EventHandlerEnvironment env)
        {
            Vault vault = env.Vault;
            ObjVer objVer = env.ObjVer;

            RenameWithAcceptance(vault, objVer);
            
            return;
        }

        private void RenameWithAcceptance(Vault vault, ObjVer objVer)
        {
            var isAccepted = this.Configuration.Accepted;

            if (objVer.Type == (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument)
            {
                try
                {
                    var properties = vault.ObjectPropertyOperations.GetProperties(objVer);
                    string currentName = string.Empty;

                    foreach (IPropertyValue propertyValue in properties)
                    {
                        if (propertyValue.PropertyDef == (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefNameOrTitle)
                        {
                            currentName = propertyValue.GetValueAsText(false, false, false, false, false, false);

                            string newName = string.Empty;

                            if (isAccepted)
                                newName = "Accepted_" + currentName;
                            else
                                newName = "Rejected_" + currentName;

                            var newPropertyValue = new PropertyValue
                            {
                                PropertyDef = (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefNameOrTitle,
                                TypedValue = new TypedValue { Value = newName }
                            };

                            vault.ObjectPropertyOperations.SetProperty(objVer, newPropertyValue);

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

        #endregion
    }
}