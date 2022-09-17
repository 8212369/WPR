using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPR.Models
{
    [Serializable]
    public class Application
    {
        public const string DataStoreFolder = "AppData";

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string ProductId { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Assembly { get; set; }
        public string EntryPoint { get; set; }
        public string Version { get; set; }     // Can't find why this conflicted with the class if Version class is used
        public DateTime InstalledTime { get; set; }
        public int PatchedVersion { get; set; }
    }
}
