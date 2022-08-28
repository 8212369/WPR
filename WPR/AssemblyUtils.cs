using System.Reflection;
using System.Runtime.Loader;
using Mono.Cecil;

namespace WPR
{
    internal class AssemblyUtils
    {
        public static Assembly? SaveExistingAssemblyAsAndReload(AssemblyLoadContext context, String currentName, AssemblyNameDefinition newName)
        {
            AssemblyNameReference reference = AssemblyNameReference.Parse(currentName);
            DefaultAssemblyResolver resolver = new DefaultAssemblyResolver();
            AssemblyDefinition newAsm = resolver.Resolve(reference);
            if (newAsm == null)
            {
                return null;
            }

            ModuleDefinition mainModule = newAsm.MainModule;
            mainModule.Name = newName.Name + ".dll";

            newAsm.Name = newName;
            newAsm.Write(newName.Name + ".dll");

            return context.LoadFromAssemblyPath("C:\\development\\WPR\\WPR\\bin\\Debug\\net6.0\\" + $"{newName.Name}.dll");
        }
    }
}
