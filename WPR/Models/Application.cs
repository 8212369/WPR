using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPR.Models
{
    public class Application
    {
        public const string DataStoreFolder = "AppData";

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String IconPath { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public String ProductId { get; set; }
        public String Author { get; set; }
        public String Publisher { get; set; }
        public String Assembly { get; set; }
        public String EntryPoint { get; set; }
        public String Version { get; set; }     // Can't find why this conflicted with the class if Version class is used
        public DateTime InstalledTime { get; set; }
        public int PatchedVersion { get; set; }
    }
}
