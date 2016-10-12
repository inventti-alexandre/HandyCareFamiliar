using System.IO;
using FreshMvvm;
using HandyCareFamiliar.Model;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareFamiliar.PageModel
{
    [ImplementPropertyChanged]
    public class FotoPageModel : FreshBasePageModel
    {
        public Foto Foto { get; set; }
        public ImageSource FotoPaciente { get; set; }

        public override void Init(object initData)
        {
            base.Init(initData);
            Foto = new Foto();
            Foto = initData as Foto;
            FotoPaciente = ImageSource.FromStream(() => new MemoryStream(Foto.FotoDados));
        }
    }
}