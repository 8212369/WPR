using Mono.Cecil;
using System;
using System.IO.Compression;
using System.Reflection;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using WPR.Common;
using WPR.Models;

namespace WPR
{
    public static class ApplicationInstaller
    {
        private const string TempXmlFile = "temp.xml";

        private static async Task<(ApplicationInstallError, Application?, string)> CreateApplicationEntryAndExtract(string path, Action<int> progressSet, Func<Application, IObservable<bool>> deleteExistingApp, CancellationToken canceled)
        {
            Application? app = null;
            string dataFolderProduct = "";

            try
            {
                ZipArchive archive = ZipFile.OpenRead(path);
                ZipArchiveEntry? entry = archive.GetEntry("WMAppManifest.xml");
                if (entry == null)
                {
                    return ( ApplicationInstallError.MissingManifestFiles, app, dataFolderProduct );
                }

                if (canceled.IsCancellationRequested)
                {
                    return ( ApplicationInstallError.Canceled, app, dataFolderProduct );
                }

                progressSet(3);

                entry.ExtractToFile(TempXmlFile, true);

                XmlDocument wmManifestDoc = new XmlDocument();
                wmManifestDoc.Load(TempXmlFile);

                XmlElement? root = wmManifestDoc.DocumentElement;
                XmlNodeList? appNodeList = root!.SelectNodes("//App");

                if (appNodeList == null)
                {
                    return ( ApplicationInstallError.InvalidManifestFiles, app, dataFolderProduct );
                }

                XmlNode? appNode = appNodeList[0];

                XmlAttribute? titleAttrib = appNode!.Attributes!["Title"];
                XmlAttribute? runtimeTypeAttrb = appNode!.Attributes!["RuntimeType"];
                XmlAttribute? versionAttrib = appNode!.Attributes!["Version"];
                XmlAttribute? productAttrib = appNode!.Attributes!["ProductID"];

                if ((titleAttrib == null) || (runtimeTypeAttrb == null) || (versionAttrib == null) || (productAttrib == null))
                {
                    return ( ApplicationInstallError.InvalidManifestFiles, app, dataFolderProduct );
                }

                string productTrimmed = productAttrib!.Value.Trim('{').Trim('}');
                string storeFolder = Configuration.Current.DataPath(Application.DataStoreFolder);
                string productStoreFolder = Path.Combine(storeFolder, productTrimmed);

                List<Application> existingApp = await ApplicationContext.Current.Applications!
                    .Where(a => a.ProductId == productTrimmed)
                    .ToListAsync();

                if (existingApp.Count != 0)
                {
                    if (await deleteExistingApp(existingApp[0]))
                    {
                        Directory.Delete(productStoreFolder, true);
                        ApplicationContext.Current.Applications!.Remove(existingApp[0]);
                    } else
                    {
                        return ( ApplicationInstallError.Canceled, app, dataFolderProduct );
                    }
                }

                if (!Enum.TryParse(runtimeTypeAttrb.Value, true, out ApplicationType runtimeTypeParsed))
                {
                    return ( ApplicationInstallError.NotSupportedAppType, app, dataFolderProduct );
                }

                XmlAttribute? authorAttrib = appNode!.Attributes!["Author"];
                XmlAttribute? publisherAttrib = appNode!.Attributes!["Publisher"];
                XmlAttribute? descriptionAttrib = appNode!.Attributes!["Description"];

                XmlNodeList? iconPathNodes = appNode!.SelectNodes("//IconPath");
                String? iconPath = null;

                if (iconPathNodes != null)
                {
                    iconPath = iconPathNodes[0]!.InnerText;
                }

                entry = archive.GetEntry("AppManifest.xaml");
                if (entry == null)
                {
                    return ( ApplicationInstallError.MissingManifestFiles, app, dataFolderProduct) ;
                }

                if (canceled.IsCancellationRequested)
                {
                    return (ApplicationInstallError.Canceled, app, dataFolderProduct);
                }

                entry.ExtractToFile(TempXmlFile, true);

                wmManifestDoc = new XmlDocument();
                wmManifestDoc.Load(TempXmlFile);

                XmlNode? deploymentNode = wmManifestDoc.DocumentElement;

                XmlAttribute? entryPointAsmAttrib = deploymentNode!.Attributes!["EntryPointAssembly"];
                XmlAttribute? entryPointTypeAttrib = deploymentNode!.Attributes!["EntryPointType"];
                
                if ((entryPointAsmAttrib == null) || (entryPointTypeAttrib == null))
                {
                    return (ApplicationInstallError.InvalidManifestFiles, app, dataFolderProduct);
                }

                var nsmgr = new XmlNamespaceManager(wmManifestDoc.NameTable);
                nsmgr.AddNamespace("a", "http://schemas.microsoft.com/client/2007/deployment");

                XmlNodeList? assemblies = deploymentNode!.SelectNodes("//a:Deployment.Parts//a:AssemblyPart", nsmgr);
                if (assemblies == null)
                {
                    return (ApplicationInstallError.InvalidManifestFiles, app, dataFolderProduct);
                }

                XmlAttribute? entryPointAsmFileNameAttrib = null;

                foreach (XmlNode? assembly in assemblies)
                {
                    XmlAttribute? attrib = assembly!.Attributes!["x:Name"] ?? assembly!.Attributes!["Name"];
                    if ((attrib == null) || (attrib.Value != entryPointAsmAttrib.Value))
                    {
                        continue;
                    }
                    entryPointAsmFileNameAttrib = assembly!.Attributes!["Source"];
                }

                if (entryPointAsmFileNameAttrib == null)
                {
                    return (ApplicationInstallError.InvalidManifestFiles, app, dataFolderProduct);
                }

                progressSet(5);

                dataFolderProduct = productStoreFolder;

                Directory.CreateDirectory(productStoreFolder);
                int count = 0;

                foreach (ZipArchiveEntry iterateEntry in archive.Entries)
                {
                    if (canceled.IsCancellationRequested)
                    {
                        Directory.Delete(productStoreFolder, true);
                        return (ApplicationInstallError.Canceled, app, dataFolderProduct);
                    }
                    string fileDestinationPath = Path.Combine(productStoreFolder, iterateEntry.FullName);

                    if (Path.GetFileName(fileDestinationPath).Length == 0)
                    {
                        Directory.CreateDirectory(fileDestinationPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fileDestinationPath)!);
                        iterateEntry.ExtractToFile(fileDestinationPath, true);
                    }

                    count++;
                    progressSet(5 + (int)((float)count / archive.Entries.Count * 50.0f));
                }

                app = new Application()
                {
                    Name = titleAttrib.Value,
                    ApplicationType = runtimeTypeParsed,
                    Version = versionAttrib.Value,
                    IconPath = (iconPath == null) ? "" : Path.Combine(productStoreFolder, iconPath),
                    Publisher = (publisherAttrib == null) ? "Unknown" : publisherAttrib.Value,
                    Author = (authorAttrib == null) ? "Unknown" : authorAttrib.Value,
                    Description = (descriptionAttrib == null) ? "" : descriptionAttrib.Value,
                    ProductId = productTrimmed,
                    Assembly = entryPointAsmFileNameAttrib.Value,
                    EntryPoint = entryPointTypeAttrib.Value,
                    InstalledTime = DateTime.Now,
                    PatchedVersion = ApplicationPatcher.Version
                };

                ApplicationContext.Current.Add(app);
                ApplicationContext.Current.SaveChanges();

                progressSet(60);
            }
            catch (Exception ex)
            {
                Log.Error(LogCategory.AppInstall, $"An unexpected error happen during the installation: \n{ex}");
                return (ApplicationInstallError.UnexpectedError, app, dataFolderProduct);
            }

            return (ApplicationInstallError.None, app, dataFolderProduct);
        }

        public static async Task<ApplicationInstallError> Install(string path, Action<int> progressSet, Func<Application, IObservable<bool>> deleteExistingApp, CancellationToken cancelSource)
        {
            try
            {
                Application? app;
                string? appDataFolder;
                ApplicationInstallError error;

                // 60% spend for extracting files
                (error, app, appDataFolder) = await Task.Run(() => CreateApplicationEntryAndExtract(path, progressSet, deleteExistingApp, cancelSource));

                if (error != ApplicationInstallError.None)
                {
                    return error;
                }

                // 20% for patching DLLs
                try
                {
                    await Task.Run(() =>
                    {
                        ApplicationPatcher patcher = new ApplicationPatcher();
                        patcher.Patch(appDataFolder, progress => progressSet(60 + (int)((double)progress / 5)), cancelSource);
                    });
                }
                catch (Exception exception)
                {
                    Log.Error(LogCategory.AppInstall, $"Application DLL patching failed with exception:\n{exception}");
                    return ApplicationInstallError.PatchFailed;
                }

                if (cancelSource.IsCancellationRequested)
                {
                    Directory.Delete(appDataFolder, true);
                    ApplicationContext.Current.Remove(app);

                    return ApplicationInstallError.Canceled;
                }

                // 20% for converting audio to unified support
                if (app.ApplicationType == ApplicationType.XNA)
                {
                    try
                    {
                        await AudioCompabilityConverter.ScanWmaAndConvert(Path.Combine(appDataFolder, "Content"),
                            progress => progressSet(80 + (int)((double)progress / 5)),
                            cancelSource);
                    } catch (Exception exception)
                    {
                        Log.Error(LogCategory.AppInstall, $"Application WMA conversion failed with exception:\n{exception}");
                        return ApplicationInstallError.ConvertFailed;
                    }
                }

                if (cancelSource.IsCancellationRequested)
                {
                    Directory.Delete(appDataFolder, true);
                    ApplicationContext.Current.Remove(app);

                    return ApplicationInstallError.Canceled;
                }

                progressSet(100);
            } catch (Exception ex)
            {
                Log.Error(LogCategory.AppInstall, $"An unexpected error happen during the installation: \n{ex}");
                return ApplicationInstallError.UnexpectedError;
            }

            return ApplicationInstallError.None;
        }
    }
}
