using System.Reflection;
using WPR;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using System.Runtime.Loader;
using System.Runtime;
using Microsoft.Xna.Framework.Graphics;
using WPR.MonoGameCompability;

namespace WPR
{
    class WPR
    {
        static void Main()
        {
            AppResolver resolver = new AppResolver();
            AssemblyDefinition newAsm = AssemblyDefinition.ReadAssembly("C:\\temp\\FNWP72.dll");
            Assembly assemMono = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("MonoGame.Framework"));


            AssemblyNameReference reference = AssemblyNameReference.Parse("WPR.MonoGameCompability");
            AssemblyNameReference referenceRuntime = AssemblyNameReference.Parse("System.Runtime");
            DefaultAssemblyResolver resolver22 = new DefaultAssemblyResolver();
            AssemblyDefinition patchMono = resolver22.Resolve(reference);

            if (newAsm == null)
            {
                return;
            }

            TypeDefinition typedef = patchMono.MainModule.GetType("WPF.MonoGameCompabilityPatch", "SpriteBatchPatch");
            foreach (var refer in newAsm.MainModule.GetTypeReferences()) {
                if (refer.Module.Name == "Microsoft.Xna.Framework.Graphics")
                {
                    var t = refer.MetadataToken;
                }
            };

            var module = newAsm.Modules[0];
            foreach (var refer in module.AssemblyReferences) {
                if (refer.Name.Contains("Microsoft.Xna") && (!refer.Name.Contains("GamerServices")))
                {
                    refer.Name = assemMono.GetName().Name;
                    refer.Version = assemMono.GetName().Version;
                    refer.PublicKey = assemMono.GetName().GetPublicKey();
                }
            }
            
            var typess = patchMono.MainModule.Types;

            module.AssemblyReferences.Add(reference);
            module.AssemblyReferences.Add(referenceRuntime);

            TypeReference typeRef = null;
            foreach (var existingRef in module.GetTypeReferences())
            {
                if (existingRef.Name == "SpriteBatch")
                {
                    existingRef.Name = "SpriteBatch2";
                    existingRef.Namespace = "WPR.MonoGameCompability.Graphics";
                    existingRef.Scope = reference;
                } else if (existingRef.FullName == "System.Diagnostics.Stopwatch")
                {
                    existingRef.Scope = referenceRuntime;
                } else if (existingRef.Name == "GraphicsDeviceManager")
                {
                    existingRef.Name = "GraphicsDeviceManager2";
                    existingRef.Namespace = "WPR.MonoGameCompability";
                    existingRef.Scope = reference;
                }
            }

            MemoryStream stream = new MemoryStream();
            newAsm.Write(stream);
            stream.Position = 0;

            Directory.SetCurrentDirectory("C:\\temp\\");

            var type = typeof(TitleContainer);
            var prop = type.GetProperty("Location", BindingFlags.NonPublic | BindingFlags.Static);
            prop.GetSetMethod(true).Invoke(null, new object[] { "C:\\temp\\" });

            Assembly assem = AssemblyLoadContext.Default.LoadFromStream(stream);
            Type tt = assem.GetType("Mortar.TheGame");
            Game obj = (Game)Activator.CreateInstance(tt);

            try
            {
                obj.IsMouseVisible = true;
                obj.Run();
                //game.Run();
            } catch (Exception ex)
            {
                Console.Write("HHH " + ex.ToString());
                Console.WriteLine(ex.StackTrace);
            }

            Console.Write("IH");
        }
    }
}