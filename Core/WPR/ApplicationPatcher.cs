using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Xml.Serialization;
using Mono.Cecil.Rocks;

using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WPR
{
    public class ApplicationPatcher
    {
        public static int Version => 0;

        private AssemblyNameReference FNACompRef;
        private AssemblyNameReference FNARef;
        private AssemblyNameReference SystemRunTimeRef;
        private AssemblyNameReference WindowsCompRef;
        private AssemblyNameReference StandardCompRef;
        private AssemblyNameReference ServiceModelPrimitivesRef;
        private AssemblyNameReference ServiceModelHTTPRef;

        private class TypePatchInfo
        {
            public String? NewName;
            public String? NewNamespace;
            public AssemblyNameReference? Reference;
        }

        private Dictionary<string, TypePatchInfo> Patches;
        private Dictionary<string, Type> MemberPatches;

        public ApplicationPatcher()
        {
            FNARef = AssemblyNameReference.Parse("FNA");
            FNACompRef = AssemblyNameReference.Parse("WPR.XnaCompability");
            SystemRunTimeRef = AssemblyNameReference.Parse("System.Runtime");
            WindowsCompRef = AssemblyNameReference.Parse("WPR.WindowsCompability");
            ServiceModelPrimitivesRef = AssemblyNameReference.Parse("System.ServiceModel.Primitives");
            ServiceModelHTTPRef = AssemblyNameReference.Parse("System.ServiceModel.Http");
            StandardCompRef = AssemblyNameReference.Parse("WPR.StandardCompability");

            Patches = new Dictionary<string, TypePatchInfo>()
            {
                { "System.Diagnostics.Stopwatch", new TypePatchInfo()
                {
                    Reference = SystemRunTimeRef
                }
                },
                { "Microsoft.Xna.Framework.GraphicsDeviceManager", new TypePatchInfo()
                {
                    NewName = "GraphicsDeviceManager2",
                    NewNamespace = "WPR.XnaCompability",
                    Reference = FNACompRef
                }
                },
                { "System.Windows.Application", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewNamespace = "WPR.WindowsCompability"
                }
                },
                { "System.Windows.ApplicationUnhandledExceptionEventArgs", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewNamespace = "WPR.WindowsCompability"
                }
                },
                { "System.IO.IsolatedStorage.IsolatedStorageSettings", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewNamespace = "WPR.WindowsCompability"
                }
                },
                { "Microsoft.Xna.Framework.Media.MediaSource", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.MediaSourceType", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.SongCollection", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.Artist", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.ArtistCollection", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.Album", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.AlbumCollection", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.Genre", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "Microsoft.Xna.Framework.Media.MediaLibrary", new TypePatchInfo()
                {
                    Reference = FNACompRef,
                    NewNamespace = "WPR.XnaCompability.Media"
                }
                },
                { "System.Windows.Media.SolidColorBrush", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewNamespace = "WPR.WindowsCompability.Media"
                }
                },
                { "System.Windows.Media.Color", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewNamespace = "WPR.WindowsCompability.Media"
                }
                },
                { "System.Windows.Thickness", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewNamespace = "WPR.WindowsCompability.Media"
                }
                },
                { "System.Windows.ResourceDictionary", new TypePatchInfo()
                {
                    Reference = WindowsCompRef,
                    NewNamespace = "WPR.WindowsCompability"
                }
                },
                { "System.ServiceModel.XmlSerializerFormatAttribute", new TypePatchInfo()
                {
                    Reference = ServiceModelPrimitivesRef
                }
                },
                { "System.ServiceModel.BasicHttpBinding", new TypePatchInfo()
                {
                    Reference = ServiceModelHTTPRef
                }
                },
                { "System.ServiceModel.BasicHttpSecurity", new TypePatchInfo()
                {
                    Reference = ServiceModelHTTPRef
                }
                },
                { "System.ServiceModel.BasicHttpSecurityMode", new TypePatchInfo()
                {
                    Reference = ServiceModelHTTPRef
                }
                },
                { "System.Windows.MessageBox", new TypePatchInfo()
                {
                    Reference = WindowsCompRef
                }
                },
                { "System.Windows.MessageBoxResult", new TypePatchInfo()
                {
                    Reference = WindowsCompRef
                }
                },
                { "System.Windows.MessageBoxButton", new TypePatchInfo()
                {
                    Reference = WindowsCompRef
                }
                }
            };

            MemberPatches = new Dictionary<string, Type>
            {
                {
                    "System.Type System.Type::GetType(System.String,System.Boolean)",
                    typeof(WPR.WindowsCompability.Type2)
                },
                {
                    "Microsoft.Xna.Framework.Graphics.DisplayMode Microsoft.Xna.Framework.Graphics.GraphicsDevice::get_DisplayMode()",
                    typeof(WPR.XnaCompability.Graphics.GraphicsDevice2)
                },
                {
                    "Microsoft.Xna.Framework.Graphics.DisplayMode Microsoft.Xna.Framework.Graphics.GraphicsAdapter::get_CurrentDisplayMode()",
                    typeof(WPR.XnaCompability.Graphics.GraphicsAdapter2)
                },
                {
                    "System.String System.IO.Path::GetDirectoryName(System.String)",
                    typeof(WPR.WindowsCompability.Path2)
                },
                {
                    "System.String System.IO.Path::GetFileName(System.String)",
                    typeof(WPR.WindowsCompability.Path2)
                },
                {
                    "System.String System.IO.Path::GetFileNameWithoutExtension(System.String)",
                    typeof(WPR.WindowsCompability.Path2)
                },
                {
                    "System.Void System.GC::Collect()",
                    typeof(WPR.WindowsCompability.GC2)
                },
                {
                    "System.Xml.Linq.XElement System.Xml.Linq.XElement::Load(System.String)",
                    typeof(WPR.StandardCompability.Xml.Linq.XElement2)
                }
            };
        }

        private void PatchRelaxedXmlNullableAttribTextSerialize(ModuleDefinition? module)
        {
            Queue<TypeDefinition> typeScanQueue = new Queue<TypeDefinition>();
            foreach (var typeDef in module!.Types)
            {
                typeScanQueue.Enqueue(typeDef);
            }

            CustomAttribute? xmlIgnoreAttrib = null;

            // Patch type for resolve XML library incompability
            while (typeScanQueue.Count != 0)
            {
                TypeDefinition type = typeScanQueue.Dequeue();

                if (type.HasNestedTypes)
                {
                    foreach (var typeNested in type.NestedTypes)
                    {
                        typeScanQueue.Enqueue(typeNested);
                    }
                }

                foreach (var field in type.Fields)
                {
                    CustomAttribute? xmlNonNullableProp = null;

                    foreach (var attrib in field.CustomAttributes)
                    {
                        if (attrib.AttributeType.FullName == typeof(XmlAttributeAttribute).FullName)
                        {
                            xmlNonNullableProp = attrib;
                            break;
                        }
                    }

                    if (xmlNonNullableProp == null)
                    {
                        continue;
                    }

                    if (field.FieldType.FullName.Contains("System.Nullable"))
                    {
                        var actualFieldType = (field.FieldType as GenericInstanceType)!.GenericArguments[0];

                        // Generate holder getter/setter
                        var getterMethod = new MethodDefinition($"get_{field.Name}SerializableHolder", MethodAttributes.Public, actualFieldType);
                        var getterGen = getterMethod.Body.GetILProcessor();

                        var nullableRefTypeGeneric = module.ImportReference(Type.GetType("System.Nullable`1")!);
                        var nullableRefType = nullableRefTypeGeneric.MakeGenericInstanceType(new TypeReference[] { actualFieldType });

                        // Emit getter
                        getterGen.Emit(OpCodes.Ldarg_0);
                        getterGen.Emit(OpCodes.Ldflda, field);
                        getterGen.Emit(OpCodes.Call, new MethodReference("get_Value", nullableRefTypeGeneric.GenericParameters[0])
                        {
                            HasThis = true,
                            DeclaringType = nullableRefType
                        });
                        getterGen.Emit(OpCodes.Ret);

                        // Emit setter
                        var setterMethod = new MethodDefinition($"set_{field.Name}SerializableHolder", MethodAttributes.Public, module.TypeSystem.Void)
                        {
                            Parameters = { new ParameterDefinition(actualFieldType) },
                            HasThis = true
                        };
                        var setterGen = setterMethod.Body.GetILProcessor();

                        setterGen.Emit(OpCodes.Ldarg_0);
                        setterGen.Emit(OpCodes.Ldarg_1);
                        setterGen.Emit(OpCodes.Newobj, new MethodReference(".ctor", module.TypeSystem.Void, nullableRefType)
                        {
                            Parameters = { new ParameterDefinition(nullableRefTypeGeneric.GenericParameters[0]) },
                            HasThis = true
                        });

                        setterGen.Emit(OpCodes.Stfld, field);
                        setterGen.Emit(OpCodes.Ret);

                        // Emit skip serialize consideration
                        var shouldSerializeMethod = new MethodDefinition($"ShouldSerialize{field.Name}SerializableHolder", MethodAttributes.Public, module.TypeSystem.Boolean);
                        var shouldSerializeGen = shouldSerializeMethod.Body.GetILProcessor();

                        shouldSerializeGen.Emit(OpCodes.Ldarg_0);
                        shouldSerializeGen.Emit(OpCodes.Ldflda, field);
                        shouldSerializeGen.Emit(OpCodes.Call, new MethodReference("HasValue", module.TypeSystem.Boolean, nullableRefType)
                        {
                            HasThis = true
                        });
                        shouldSerializeGen.Emit(OpCodes.Ret);

                        type.Methods.Add(shouldSerializeMethod);
                        type.Methods.Add(getterMethod);
                        type.Methods.Add(setterMethod);

                        var propSeri = new PropertyDefinition($"{field.Name}SerializableHolder", PropertyAttributes.None, actualFieldType)
                        {
                            GetMethod = getterMethod,
                            SetMethod = setterMethod
                        };

                        type.Properties.Add(propSeri);

                        if (xmlIgnoreAttrib == null)
                        {
                            xmlIgnoreAttrib = new CustomAttribute(module.ImportReference(typeof(XmlIgnoreAttribute).
                                GetConstructor(Type.EmptyTypes)));
                        }

                        field.CustomAttributes.Remove(xmlNonNullableProp);
                        field.CustomAttributes.Add(xmlIgnoreAttrib);

                        // Add attribute if they already gave name, else we need to be creative
                        if (xmlNonNullableProp.HasConstructorArguments)
                        {
                            propSeri.CustomAttributes.Add(xmlNonNullableProp);
                        }
                        else
                        {
                            var attributeType = (xmlNonNullableProp.AttributeType.FullName == typeof(XmlAttributeAttribute).FullName)
                                    ? typeof(XmlAttributeAttribute) : typeof(XmlTextAttribute);

                            MethodReference methodConstructor = module.ImportReference(attributeType
                                .GetConstructor(new Type[] { typeof(String) }));
                            propSeri.CustomAttributes.Add(new CustomAttribute(methodConstructor)
                            {
                                ConstructorArguments = { new CustomAttributeArgument(module.TypeSystem.String, field.Name) }
                            });
                        }
                    }
                }
            }
        }

        public void PatchDll(string modulePath)
        {
            AssemblyDefinition assemblyData = AssemblyDefinition.ReadAssembly(modulePath);
            var module = assemblyData.MainModule;

            assemblyData.Name.Name = AssemblyNameStandardization.Process(assemblyData.Name.Name);

            string modulePathNameStandardized = Path.Combine(Path.GetDirectoryName(modulePath)!,
                AssemblyNameStandardization.Process(Path.GetFileNameWithoutExtension(modulePath)) +
                Path.GetExtension(modulePath));

            AssemblyNameReference? xnaGameServices = null;

            // Remove unneeded attribute (pretty sure!)
            foreach (var attrib in module.Assembly.CustomAttributes)
            {
                if (attrib.AttributeType.FullName == "System.Runtime.CompilerServices.CodeGenerationAttribute")
                {
                    module.Assembly.CustomAttributes.Remove(attrib);
                    break;
                }
            }

            foreach (var refer in module.AssemblyReferences)
            {
                if (refer.Name.Contains("Microsoft.Xna"))
                {
                    if (refer.Name.Contains("GamerServices"))
                    {
                        xnaGameServices = refer;
                    }
                    else
                    {
                        refer.Name = FNARef.Name;
                        refer.Version = FNARef.Version;
                        refer.PublicKey = FNARef.PublicKey;
                    }
                } else if (refer.Name.Equals("mscorlib.Extensions", StringComparison.OrdinalIgnoreCase))
                {
                    refer.Name = SystemRunTimeRef.Name;
                    refer.Version = SystemRunTimeRef.Version;
                    refer.PublicKey = SystemRunTimeRef.PublicKey;
                }
                else if (refer.Name.Equals("System.ServiceModel", StringComparison.OrdinalIgnoreCase))
                {
                    refer.Name = ServiceModelPrimitivesRef.Name;
                    refer.Version = ServiceModelPrimitivesRef.Version;
                    refer.PublicKey = ServiceModelPrimitivesRef.PublicKey;
                }
            }

            PatchRelaxedXmlNullableAttribTextSerialize(module);

            module.AssemblyReferences.Add(FNACompRef);
            module.AssemblyReferences.Add(WindowsCompRef);
            module.AssemblyReferences.Add(SystemRunTimeRef);
            module.AssemblyReferences.Add(ServiceModelPrimitivesRef);
            module.AssemblyReferences.Add(ServiceModelHTTPRef);
            module.AssemblyReferences.Add(StandardCompRef);

            Dictionary<string, TypeReference> typeRefPatchCache = new Dictionary<string, TypeReference>();

            foreach (var memberRef in module.GetMemberReferences())
            {
                foreach (var patch in MemberPatches)
                {
                    if (memberRef.FullName.Contains("Collect"))
                    {
                        Console.WriteLine("TeSTING!");
                    }
                    if (memberRef.FullName == patch.Key)
                    {
                        if (typeRefPatchCache.ContainsKey(patch.Value.FullName!))
                        {
                            memberRef.DeclaringType = typeRefPatchCache[patch.Value.FullName!];
                        } else
                        {
                            memberRef.DeclaringType = module.ImportReference(patch.Value);
                            typeRefPatchCache.Add(patch.Value.FullName!, memberRef.DeclaringType);
                        }
                    }
                }
            }

            foreach (var existingRef in module.GetTypeReferences())
            {
                existingRef.Name = AssemblyNameStandardization.Process(existingRef.Name);

                if (existingRef.FullName == "Microsoft.Xna.Framework.GamerServices.GamerServicesComponent")
                {
                    existingRef.Scope = xnaGameServices;
                }
                else
                {
                    if (Patches.ContainsKey(existingRef.FullName))
                    {
                        TypePatchInfo patch = Patches[existingRef.FullName];
                        if (patch != null)
                        {
                            if (patch.NewName != null)
                            {
                                existingRef.Name = patch.NewName;
                            }

                            if (patch.NewNamespace != null)
                            {
                                existingRef.Namespace = patch.NewNamespace;
                            }

                            if (patch.Reference != null)
                            {
                                existingRef.Scope = patch.Reference;
                            }
                        }
                    }
                }
            }

            assemblyData.Write(modulePath + ".new");
            assemblyData.Dispose();

            File.Move(modulePath, modulePathNameStandardized + ".original", true);
            File.Move(modulePath + ".new", modulePathNameStandardized, true);
        }

        public void Patch(string appRootPath, Action<int> progress, CancellationToken token)
        {
            List<string> filenameList = Directory.EnumerateFiles(appRootPath, "*.dll", SearchOption.AllDirectories).ToList();
            int totalCount = filenameList.Count;
            int current = 0;

            foreach (var filename in filenameList)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                try
                {
                    PatchDll(filename);
                } catch (Exception ex)
                {
                    Common.Log.Error(Common.LogCategory.AppPatcher, $"Fail to patch DLL with path: {filename}. Error:\n{ex}");
                    continue;
                }

                current++;
                progress((int)(current * 100.0 / totalCount));
            }
        }
    }
}
