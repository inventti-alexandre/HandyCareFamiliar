using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareFamiliar.Data;
using HandyCareFamiliar.Helper;
using HandyCareFamiliar.Model;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareFamiliar.PageModel
{
    [ImplementPropertyChanged]
    public class DetalheBuscaPageModel:FreshBasePageModel
    {
        public PageModelHelper PageModelHelper { get; set; }
        public Familiar Familiar { get; set; }
        public Cuidador Cuidador { get; set; }
        public Avaliacao Avaliacao { get; set; }
        public ImageSource CuidadorFoto { get; set; }
        public ImageSource DocumentoFoto { get; set; }
        public ContatoEmergencia ContatoEmergencia { get; set; }
        public ConCelular ConCelular { get; set; }
        public ConEmail ConEmail { get; set; }
        public ConTelefone ConTelefone { get; set; }

        public TipoCuidador TipoCuidador { get; set; }
        public ValidacaoCuidador ValidacaoCuidador { get; set; }
        public ObservableCollection<Avaliacao> Avaliacoes { get; set; }

        public override async void Init(object initData)
        {
            base.Init(initData);
            PageModelHelper = new PageModelHelper
            {
                ActivityRunning = true,
                Visualizar = false
            };
            TipoCuidador=new TipoCuidador();
            ValidacaoCuidador=new ValidacaoCuidador();
            Familiar = new Familiar();
            Cuidador = new Cuidador();
            var x = initData as Tuple<Cuidador,Familiar>;
            if (x == null) return;
            Familiar = x.Item2;
            Cuidador = x.Item1;
            if (Cuidador.CuiFoto != null)
            {
                CuidadorFoto = ImageSource.FromStream(() => new MemoryStream(Cuidador.CuiFoto));
            }
            ContatoEmergencia=new ContatoEmergencia();
            ConEmail=new ConEmail();
            ConCelular=new ConCelular();
            ConTelefone=new ConTelefone();
            await GetAvaliacoes();
            PageModelHelper.ActivityRunning = false;
            PageModelHelper.Visualizar = true;
        }
        public Command AvaliacaoCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<VisualizarAvaliacoesPageModel>(Avaliacoes);
                });
            }
        }
        public Command CelularCommand
        {
            get
            {
                return new Command(()=>
                {
                    Device.OpenUri(new Uri("tel:"+ConCelular.ConNumCelular));
                });
            }
        }
        public Command TelefoneCommand
        {
            get
            {
                return new Command(() =>
                {
                    Device.OpenUri(new Uri("tel:"+ConTelefone.ConNumTelefone));
                });
            }
        }
        public Command EmailCommand
        {
            get
            {
                return new Command(() =>
                {
                    Device.OpenUri(new Uri("mailto:"+ConEmail.ConEnderecoEmail));
            });
            }
        }

        private async Task GetAvaliacoes()
        {
            try
            {
                await Task.Run(async () =>
                {
                    var selection =
                        new ObservableCollection<Avaliacao>(
                            await FamiliarRestService.DefaultManager.RefreshAvaliacaoAsync(true))
                            .Where(e => e.AvaCuidador ==Cuidador.Id).AsEnumerable();
                    PageModelHelper.Media = 0;
                    var avaliacaos = selection as Avaliacao[] ?? selection.ToArray();
                    foreach (var x in avaliacaos)
                    {
                        PageModelHelper.Media += x.AvaPontuacao;
                    }
                    PageModelHelper.Media /= avaliacaos.Count();
                    Avaliacoes = new ObservableCollection<Avaliacao>(avaliacaos);
                });
                await Task.Run(async () =>
                {
                    ValidacaoCuidador = new ObservableCollection<ValidacaoCuidador>(
                            await FamiliarRestService.DefaultManager.RefreshValidacaoCuidadorAsync(true))
                        .FirstOrDefault(e => e.Id == Cuidador.CuiValidacaoCuidador);
                    TipoCuidador = new ObservableCollection<TipoCuidador>(
                            await FamiliarRestService.DefaultManager.RefreshTipoCuidadorAsync(true))
                        .FirstOrDefault(e => e.Id == Cuidador.CuiTipoCuidador);
                    if (ValidacaoCuidador.ValDocumento != null)
                    {
                        DocumentoFoto = ImageSource.FromStream(() => new MemoryStream(ValidacaoCuidador.ValDocumento));
                    }
                });
                await Task.Run(async () =>
                {
                    ContatoEmergencia = new ObservableCollection<ContatoEmergencia>(
                        await FamiliarRestService.DefaultManager.RefreshContatoEmergenciaAsync()).FirstOrDefault(
                        e => e.Id == Cuidador.CuiContatoEmergencia);
                    ConCelular = new ObservableCollection<ConCelular>(
                        await FamiliarRestService.DefaultManager.RefreshConCelularAsync()).FirstOrDefault(e => e.Id == ContatoEmergencia.ConCelular);
                    ConEmail = new ObservableCollection<ConEmail>(
await FamiliarRestService.DefaultManager.RefreshConEmailAsync()).FirstOrDefault(e => e.Id == ContatoEmergencia.ConEmail);
                    ConTelefone = new ObservableCollection<ConTelefone>(
await FamiliarRestService.DefaultManager.RefreshConTelefoneAsync()).FirstOrDefault(e => e.Id == ContatoEmergencia.ConTelefone);
                });

            }
            catch (ArgumentNullException e)
            {
                Debug.WriteLine(e.Message);
            }

            catch (NullReferenceException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

    }
}
