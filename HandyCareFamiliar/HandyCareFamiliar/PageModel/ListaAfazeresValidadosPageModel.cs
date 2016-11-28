using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FreshMvvm;
using HandyCareFamiliar.Data;
using HandyCareFamiliar.Helper;
using HandyCareFamiliar.Model;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareFamiliar.PageModel
{
    [ImplementPropertyChanged]
    public class ListaAfazeresValidadosPageModel:FreshBasePageModel
    {
        private Afazer _selectedAfazer;
        public bool AfazerConcluido;

        public bool DeleteVisible;

        public PageModelHelper oHorario { get; set; }
        public ValidacaoAfazer ValidacaoAfazer { get; set; }
        public MotivoNaoValidacaoConclusaoAfazer MotivoNaoValidacaoConclusaoAfazer { get; set; }
        public Afazer AfazerSelecionado { get; set; }
        public PacienteFamiliar PacienteFamiliar { get; set; }
        public ObservableCollection<Afazer> Afazeres { get; set; }
        public ObservableCollection<MotivoNaoValidacaoConclusaoAfazer> MotivoNaoValidacaoConclusaoAfazeres { get; set; }
        public ObservableCollection<ValidacaoAfazer> AfazeresValidados { get; set; }

        public Afazer SelectedAfazer
        {
            get { return _selectedAfazer; }
            set
            {
                _selectedAfazer = value;
                if (value == null) return;
                AfazerSelected.Execute(value);
                SelectedAfazer = null;
            }
        }

        public Command VisualizarNaoValidados
        {
            get
            {
                return new Command( async() =>
                {
                    await CoreMethods.PushPageModel<ListaAfazeresNaoValidadosPageModel>(PacienteFamiliar);
                });
            }
        }
        public Command<Afazer> AfazerSelected
        {
            get
            {
                return new Command<Afazer>(async afazer =>
                {
                var result = await CoreMethods.DisplayActionSheet("O afazer foi finalizado corretamente?",
"Cancelar", null, "Sim", "Não");
                    switch (result)
                    {
                        case "Sim":
                        {
                            ValidacaoAfazer.Id = Guid.NewGuid().ToString();
                            ValidacaoAfazer.ValAfazer = afazer.Id;
                            ValidacaoAfazer.ValFamId = PacienteFamiliar.FamId;
                            ValidacaoAfazer.ValValidado = true;
                            await FamiliarRestService.DefaultManager.SaveValidacaoAfazerAsync(ValidacaoAfazer, true);
                            await CoreMethods.PopPageModel();
                            UserDialogs.Instance.ShowSuccess("Afazer validado com sucesso", 4000);

                        }
                            break;
                        case "Não":
                        {
                            var resulto = await UserDialogs.Instance.PromptAsync(new PromptConfig()

                                .SetTitle("Informe o motivo da não validação do afazer")
                                .SetPlaceholder("Descreva o que aconteceu")
                                .SetMaxLength(255));
                            MotivoNaoValidacaoConclusaoAfazer.Id = Guid.NewGuid().ToString();
                            oHorario.Visualizar = false;
                            oHorario.ActivityRunning = true;
                            ValidacaoAfazer.Id = Guid.NewGuid().ToString();
                            ValidacaoAfazer.ValAfazer = afazer.Id;
                            ValidacaoAfazer.ValFamId = PacienteFamiliar.FamId;
                            ValidacaoAfazer.ValValidado = false;
                            MotivoNaoValidacaoConclusaoAfazer.MoValidacao = ValidacaoAfazer.Id;
                            MotivoNaoValidacaoConclusaoAfazer.MoDescricao = resulto.Text;
                            await FamiliarRestService.DefaultManager.SaveValidacaoAfazerAsync(ValidacaoAfazer, true);
                            await
                                FamiliarRestService.DefaultManager.SaveMotivoNaoValidacaoConclusaoAfazer(
                                    MotivoNaoValidacaoConclusaoAfazer, true);

                            await CoreMethods.PopPageModel();
                            UserDialogs.Instance.ShowSuccess("Não validação de afazer registrada com sucesso", 4000);

                        }
                            break;
                    }
                });
            }
        }
        public override async void Init(object initData)
        {
            base.Init(initData);
            ValidacaoAfazer=new ValidacaoAfazer();
            PacienteFamiliar = new PacienteFamiliar();
            MotivoNaoValidacaoConclusaoAfazer=new MotivoNaoValidacaoConclusaoAfazer();
            PacienteFamiliar = initData as PacienteFamiliar;
            oHorario = new PageModelHelper { ActivityRunning = true, Visualizar = false };
            AfazerSelecionado = new Afazer();
            await GetAfazeres();
        }
        public async Task GetAfazeres()
        {
            try
            {
                await Task.Run(async () =>
                {
                    var pacresult =
                        new ObservableCollection<CuidadorPaciente>(
                                await FamiliarRestService.DefaultManager.RefreshCuidadorPacienteAsync())
                            .Where(e => e.PacId == PacienteFamiliar.PacId)
                            .AsEnumerable();
                    MotivoNaoValidacaoConclusaoAfazeres = new ObservableCollection<MotivoNaoValidacaoConclusaoAfazer>(
                        await FamiliarRestService.DefaultManager.RefreshMotivoNaoValidacaoConclusaoAfazerAsync());
                    if (MotivoNaoValidacaoConclusaoAfazeres.Count > 0)
                    {
                        var x =
    new ObservableCollection<ValidacaoAfazer>(
        await FamiliarRestService.DefaultManager.RefreshValidacaoAfazerAsync()).Where(e => e.ValValidado).
        Where(e => !MotivoNaoValidacaoConclusaoAfazeres.Select(y => y.MoValidacao).Contains(e.Id)).AsEnumerable();
                        AfazeresValidados = new ObservableCollection<ValidacaoAfazer>(x);

                    }
                    else
                    {
                        AfazeresValidados= new ObservableCollection<ValidacaoAfazer>(
        await FamiliarRestService.DefaultManager.RefreshValidacaoAfazerAsync());
                    }

                    if (AfazeresValidados.Count > 0)
                    {
                        var py = new ObservableCollection<ConclusaoAfazer>(
    await FamiliarRestService.DefaultManager.RefreshConclusaoAfazerAsync()).Where(e => AfazeresValidados.Select(y => y.ValAfazer).Contains(e.Id));
                        var result =
        new ObservableCollection<Afazer>(await FamiliarRestService.DefaultManager.RefreshAfazerAsync()).Where(e => py.Select(y => y.ConAfazer).Contains(e.Id)).AsEnumerable();

                        //    var result = py.Where(e => AfazeresValidados.Select(m => m.ValAfazer)
                        //            .Contains(e.Id))
                        //        .Where(e => pacresult.Select(m => m.Id).Contains(e.AfaPaciente))
                        //        .AsEnumerable();
                            Afazeres = new ObservableCollection<Afazer>(result);
                    }
                    else
                    {
                        Afazeres = new ObservableCollection<Afazer>(await FamiliarRestService.DefaultManager.RefreshAfazerAsync());

                    }
                });
                oHorario.ActivityRunning = false;
                oHorario.Visualizar = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
