using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System;

namespace Microsoft.Xna.Framework.GamerServices
{
    public class Achievement
    {
        public Achievement()
        {
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        public string _IconPath { get; set; }

        public string Description { get; set; }

        public bool DisplayBeforeEarned { get; set; }

        public DateTime EarnedDateTime { get; set; }

        public bool EarnedOnline { get; set; }

        public int GamerScore { get; set; }

        public string HowToEarn { get; set; }

        public bool IsEarned { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public string OwnProductId { get; set; }

        public Stream GetPicture()
        {
            Stream res = new FileStream(_IconPath, FileMode.Open);
            return res;
        }
    }
}