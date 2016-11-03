using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareFamiliar.Data;
using HandyCareFamiliar.Helper;
using HandyCareFamiliar.Model;
using Newtonsoft.Json;
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
        private Parentesco _parentesco;
        private TipoContato _tipoContato;
        private App app;
        private Geoname _selectedEstado;
        public Familiar Familiar { get; set; }
        public ContatoEmergencia ContatoEmergencia { get; set; }
        public ConCelular ConCelular { get; set; }
        public ConEmail ConEmail { get; set; }
        public ConTelefone ConTelefone { get; set; }
        public bool NovoFamiliar { get; set; } = true;

        public ObservableCollection<TipoContato> TipoContatos { get; set; }
        public ObservableCollection<Parentesco> TiposFamiliares { get; set; }
        public TipoContato TipoContato { get; set; }
        public ObservableCollection<Geoname> ListaEstados { get; set; }
        public ObservableCollection<Geoname> ListaCidades { get; set; }
        public Geoname Cidade { get; set; }

        //public ValidacaoFamiliar ValidacaoFamiliar { get; set; }
        public PageModelHelper oHorario { get; set; }
        public ImageSource FamiliarFoto { get; set; }
        public ImageSource Documento { get; set; }

        public Parentesco SelectedParentesco
        {
            get { return _parentesco; }
            set
            {
                _parentesco = value;
                if (value != null)
                {
                    //ShowMedicamentos.Execute(value);
                    //SelectedPaciente = null;
                }
            }
        }
        public TipoContato SelectedTipoContato
        {
            get { return _tipoContato; }
            set
            {
                _tipoContato = value;
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
                    if (Cidade.name != null)
                    {
                        Familiar.FamCidade = Cidade.name;
                        Familiar.FamEstado = SelectedEstado.name;
                    }
                    if (oHorario.NovoFamiliar)
                    {
                        Familiar.FamParentesco = SelectedParentesco.Id;
                        Familiar.FamContatoEmergencia = ContatoEmergencia.Id;
                        ContatoEmergencia.ConEmail = ConEmail.Id;
                        ContatoEmergencia.ConCelular = ConCelular.Id;
                        ContatoEmergencia.ConTelefone = ConTelefone.Id;
                        ContatoEmergencia.ConTipo = TipoContato.Id;
                    }
                    await Task.Run(async () =>
                    {
                        await FamiliarRestService.DefaultManager.SaveConCelularAsync(ConCelular, oHorario.NovoFamiliar);
                        await FamiliarRestService.DefaultManager.SaveConEmailAsync(ConEmail, oHorario.NovoFamiliar);
                        await FamiliarRestService.DefaultManager.SaveConTelefoneAsync(ConTelefone, oHorario.NovoFamiliar);
                        await FamiliarRestService.DefaultManager.SaveContatoEmergenciaAsync(ContatoEmergencia, oHorario.NovoFamiliar);
                        await FamiliarRestService.DefaultManager.SaveFamiliarAsync(Familiar, oHorario.NovoFamiliar);
                    });
                    if (oHorario.NovoFamiliar)
                        app.AbrirMainMenu(Familiar);
                    else
                        await CoreMethods.PopPageModel(Familiar);
                    //                        await CoreMethods.PushPageModelWithNewNavigation<MainMenuPageModel>(Familiar);
                });
            }
        }
        public Command AlterarCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (NovoFamiliar)
                        NovoFamiliar = false;
                    else
                        NovoFamiliar = true;
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
            TipoContato=new TipoContato();
            oHorario.NovoFamiliar = Familiar?.Id == null;
            if (oHorario.NovoFamiliar)
            {
                oHorario.BoasVindas = "Tirar foto";
                NovoFamiliar = true;
                ConTelefone = new ConTelefone { Id = Guid.NewGuid().ToString() };
                ConCelular = new ConCelular { Id = Guid.NewGuid().ToString() };
                ConEmail = new ConEmail { Id = Guid.NewGuid().ToString() };
                ContatoEmergencia = new ContatoEmergencia { Id = Guid.NewGuid().ToString() };
            }
            else
            {
                NovoFamiliar = false;
                oHorario.BoasVindas = "Alterar foto";
            }
            oHorario.NovoCadastro = Familiar?.Id == null;
            oHorario.FamiliarExibicao = Familiar?.Id != null;
            await GetData();
            if (Familiar?.FamFoto != null)
            {
                FamiliarFoto = ImageSource.FromStream(() => new MemoryStream(Familiar.FamFoto));
            }
            var n = new HttpClient();
            var b = await n.GetStringAsync("http://www.geonames.org/childrenJSON?geonameId=3469034");
            var o = JsonConvert.DeserializeObject<RootObject>(b);
            ListaEstados = new ObservableCollection<Geoname>(o.geonames);

            oHorario.ActivityRunning = false;
            oHorario.Visualizar = true;
        }

        public Geoname SelectedEstado
        {
            get { return _selectedEstado; }
            set
            {
                _selectedEstado = value;
                if (value != null)
                {
                    EstadoSelected.Execute(value);
                }
            }
        }

        public Command<Geoname> EstadoSelected
        {
            get
            {
                return new Command<Geoname>(async estado =>
                {
                    var x = new HttpClient();
                    var b = await x.GetStringAsync("http://www.geonames.org/childrenJSON?geonameId=" + estado.geonameId);
                    var o = JsonConvert.DeserializeObject<RootObject>(b);
                    ListaCidades = new ObservableCollection<Geoname>(o.geonames);
                });
            }
        }

        private async Task GetData()
        {
            try
            {
                await Task.Run(async () =>
                {
                    TiposFamiliares = new ObservableCollection<Parentesco>(
                        await FamiliarRestService.DefaultManager.RefreshParentescoAsync());
                    TipoContato = new ObservableCollection<TipoContato>(
                        await FamiliarRestService.DefaultManager.RefreshTipoContatoAsync())
                        .FirstOrDefault(e=>e.TipDescricao=="Familiar");

                    //var x =
                    //    new ObservableCollection<Parentesco>(
                    //        await FamiliarRestService.DefaultManager.RefreshParentescoAsync());
                    if (!oHorario.NovoFamiliar)
                        SelectedParentesco = TiposFamiliares.FirstOrDefault(e => e.Id == Familiar.FamParentesco);
                });
                if (oHorario.NovoFamiliar == false)
                {
                    await Task.Run(async () =>
                    {
                        ContatoEmergencia = new ObservableCollection<ContatoEmergencia>(
                            await FamiliarRestService.DefaultManager.RefreshContatoEmergenciaAsync()).FirstOrDefault(
                            e => e.Id == Familiar.FamContatoEmergencia);
                        ConCelular = new ObservableCollection<ConCelular>(
                            await FamiliarRestService.DefaultManager.RefreshConCelularAsync()).FirstOrDefault(e=>e.Id==ContatoEmergencia.ConCelular);
                        ConEmail = new ObservableCollection<ConEmail>(
    await FamiliarRestService.DefaultManager.RefreshConEmailAsync()).FirstOrDefault(e => e.Id == ContatoEmergencia.ConEmail);
                        ConTelefone = new ObservableCollection<ConTelefone>(
    await FamiliarRestService.DefaultManager.RefreshConTelefoneAsync()).FirstOrDefault(e => e.Id == ContatoEmergencia.ConTelefone);
                    });
                }
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}