using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareFamiliar.Model
{
    [Table("ValidacaoAfazer")]
    [ImplementPropertyChanged]
    public class ValidacaoAfazer
    {

        public string Id { get; set; }
        public string ValFamId { get; set; }
        public string ValAfazer { get; set; }

        public bool ValValidado { get; set; }

    }
}