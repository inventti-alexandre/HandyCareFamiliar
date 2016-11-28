using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;

namespace HandyCareFamiliar.Model
{

    [Table("Camera")]
    [ImplementPropertyChanged]
    public partial class Camera
    {
        public string Id { get; set; }
        [Required]
        [StringLength(36)]
        public string CamFamiliar { get; set; }

        public string CamIPv4 { get; set; }

        [StringLength(50)]
        public string CamIPv6 { get; set; }
        public string CamDescricao { get; set; }

        public virtual Familiar Familiar { get; set; }

    }
}
