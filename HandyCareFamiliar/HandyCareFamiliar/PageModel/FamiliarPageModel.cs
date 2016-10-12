using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareFamiliar.Data;
using HandyCareFamiliar.Helper;
using HandyCareFamiliar.Model;
using Plugin.Media;
using Plugin.Media.Abstractions;
using PropertyChanged;
using Xamarin.Forms;
//using Plugin.Media;
//using Plugin.Media.Abstractions;

namespace HandyCareFamiliar.PageModel
{
    [ImplementPropertyChanged]
    public class FamiliarPageModel : FreshBasePageModel
    {
        private Parentesco _Parentesco;
        private App app;
        public Familiar Familiar { get; set; }
        public ObservableCollection<Parentesco> TiposFamiliares { get; set; }
        //public ValidacaoFamiliar ValidacaoFamiliar { get; set; }
        public PageModelHelper oHorario { get; set; }
        public ImageSource FamiliarFoto { get; set; }
        public ImageSource Documento { get; set; }

        public Parentesco SelectedParentesco
        {
            get { return _Parentesco; }
            set
            {
                _Parentesco = value;
                if (value != null)
                {
                    //ShowMedicamentos.Execute(value);
                    //SelectedPaciente = null;
                }
            }
        }

        public Command FotoFam
        {
            get
            {
                return new Command(async () =>
                {
                    var result =
                        await CoreMethods.DisplayActionSheet("Forma de fotografia", "Cancelar", null, "Galeria",
                            "Tirar foto");

                    switch (result)
                    {
                        case "Tirar foto":
                        {
                            var image = new Image();
                            await CrossMedia.Current.Initialize();

                            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                            {
                                await CoreMethods.DisplayAlert("No Camera", ":( No camera available.", "OK");
                                return;
                            }

                            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                            {
                                Directory = "Handy Care Fotos",
                                Name = DateTime.Now + "HandyCareFoto.jpg",
                                CompressionQuality = 10,
                                PhotoSize = PhotoSize.Medium,
                                SaveToAlbum = true
                            });
                            if (file == null)
                                return;
                            await CoreMethods.DisplayAlert("File Location", file.Path, "OK");
                            Familiar.FamFoto = HelperClass.ReadFully(file.GetStream());
                            FamiliarFoto = ImageSource.FromStream(() =>
                            {
                                var stream = file.GetStream();
                                file.Dispose();
                                return stream;
                            });
                        }
                            break;
                        case "Galeria":
                        {
                            var image = new Image();

                            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                            {
                                CompressionQuality = 10,
                                PhotoSize = PhotoSize.Medium
                            });
                            if (file == null)
                                return;
                            await CoreMethods.DisplayAlert("File Location", file.Path, "OK");
                            Familiar.FamFoto = HelperClass.ReadFully(file.GetStream());
                            FamiliarFoto = ImageSource.FromStream(() =>
                            {
                                var stream = file.GetStream();
                                file.Dispose();
                                return stream;
                            });
                        }
                            break;
                    }
                });
            }
        }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    oHorario.Visualizar = false;
                    oHorario.ActivityRunning = true;
                    Familiar.FamParentesco = SelectedParentesco.Id;
                    await FamiliarRestService.DefaultManager.SaveFamiliarAsync(Familiar, oHorario.NovoFamiliar);
                    if (oHorario.NovoFamiliar)
                        app.AbrirMainMenu(Familiar);
                    else
                        await CoreMethods.PopPageModel(Familiar);

                    //                        await CoreMethods.PushPageModelWithNewNavigation<MainMenuPageModel>(Familiar);
                });
            }
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            Familiar = new Familiar();
            SelectedParentesco = new Parentesco();
            oHorario = new PageModelHelper
            {
                ActivityRunning = true,
                Visualizar = false,
                VisualizarTermino = false,
                NovoFamiliar = false,
                NovoCadastro = false,
                FamiliarExibicao = true
            };
            var x = initData as Tuple<Familiar, App>;
            if (x != null)
            {
                Familiar = x.Item1;
                if (x.Item2 != null)
                    app = x.Item2;
            }
            //Familiar = initData as Familiar;
            oHorario.NovoFamiliar = Familiar?.Id == null;
            oHorario.NovoCadastro = Familiar?.Id == null;
            oHorario.FamiliarExibicao = Familiar?.Id != null;
            await GetData();
            oHorario.ActivityRunning = false;
            oHorario.Visualizar = true;
        }


        private async Task GetData()
        {
            try
            {
                await Task.Run(async () =>
                {
                    TiposFamiliares =
                        new ObservableCollection<Parentesco>(
                            await FamiliarRestService.DefaultManager.RefreshParentescoAsync());
                    var x = TiposFamiliares.Count;
                    //var x =
                    //    new ObservableCollection<Parentesco>(
                    //        await FamiliarRestService.DefaultManager.RefreshParentescoAsync());
                    if (!oHorario.NovoFamiliar)
                        SelectedParentesco = TiposFamiliares.FirstOrDefault(e => e.Id == Familiar.FamParentesco);
                });
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}