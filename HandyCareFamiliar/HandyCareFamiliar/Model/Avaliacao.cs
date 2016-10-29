using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using PropertyChanged;

namespace HandyCareFamiliar.Model
{
    [Table("Avaliacao")]
    [ImplementPropertyChanged]
    public class Avaliacao
    {
        public string Id { get; set; }
        public string AvaCuidador { get; set; }
        public string AvaFamiliar { get; set; }

        public double AvaPontuacao { get; set; }

        public string AvaDescricao { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        [JsonIgnore]
        public virtual Cuidador Cuidador { get; set; }
        [JsonIgnore]
        public virtual Familiar Familiar { get; set; }
    }
}