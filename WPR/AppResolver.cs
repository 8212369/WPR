using System.Reflection;
using System.Runtime.Loader;
using Mono.Cecil;

namespace WPR
{
    internal class AppResolver : AssemblyLoadContext, IDisposable
    {
        public AppResolver() => this.Resolving += this.ResolveMissingDependencies;

        public void Dispose() => this.Resolving -= this.ResolveMissingDependencies;

        Assembly LoadXnaAssembly(AssemblyLoadContext context, AssemblyName requiredName)
        {
            AssemblyNameDefinition def =new AssemblyNameDefinition(requiredName.Name, requiredName.Version);
            def.PublicKeyToken = requiredName.GetPublicKeyToken();

            return AssemblyUtils.SaveExistingAssemblyAsAndReload(context, "MonoGame.Framework", def);
        }

        Assembly ResolveMissingDependencies(AssemblyLoadContext assemblyLoadContext, AssemblyName assemblyName)
        {
            String assemblyNameInString = assemblyName.Name ?? "";
            if ((assemblyNameInString.Equals("Microsoft.Xna.Framework", StringComparison.OrdinalIgnoreCase)) ||
                (assemblyNameInString.Equals("Microsoft.Xna.Framework.Game", StringComparison.OrdinalIgnoreCase)) ||
                (assemblyNameInString.Equals("Microsoft.Xna.Framework.Graphics", StringComparison.OrdinalIgnoreCase)))
            {
                Assembly asm = LoadXnaAssembly(assemblyLoadContext, assemblyName);
                return asm;
            }
            return null;
        }
    }
}
