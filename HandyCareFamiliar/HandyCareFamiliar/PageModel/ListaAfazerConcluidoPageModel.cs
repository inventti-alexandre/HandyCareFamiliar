﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class ListaAfazerConcluidoPageModel : FreshBasePageModel
    {
        //private readonly IConclusaoAfazerRestService _conclusaoRestService;
        //private readonly IAfazerRestService _restService;
        //private readonly ICuidadorPacienteRestService _cuidadorPacienteRestService;
        private Afazer _selectedAfazer;
        public bool AfazerConcluido;

        public bool DeleteVisible;

        public PageModelHelper oHorario { get; set; }
        public Afazer AfazerSelecionado { get; set; }
        public PacienteFamiliar PacienteFamiliar { get; set; }
        public ObservableCollection<Afazer> Afazeres { get; set; }
        public ObservableCollection<ConclusaoAfazer> AfazeresConcluidos { get; set; }

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

        public Command<Afazer> AfazerSelected
        {
            get
            {
                return new Command<Afazer>(async afazer =>
                {
                    var afazerConcluido = AfazeresConcluidos.FirstOrDefault(m => m.ConAfazer == afazer.Id);
                    var afazeres = new Tuple<Afazer, ConclusaoAfazer, PacienteFamiliar>(afazer, afazerConcluido, PacienteFamiliar);
                    await CoreMethods.PushPageModel<ConclusaoAfazerPageModel>(afazeres);
                });
            }
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            PacienteFamiliar = new PacienteFamiliar();
            PacienteFamiliar = initData as PacienteFamiliar;
            oHorario = new PageModelHelper {ActivityRunning = true, Visualizar = false};
            AfazerSelecionado = new Afazer();
            await GetAfazeres();
        }

        public override void ReverseInit(object returndData)
        {
            base.ReverseInit(returndData);
            var newAfazer = returndData as Afazer;
            if (!Afazeres.Contains(newAfazer))
                Afazeres.Add(newAfazer);
            else
                Task.Run(async () => await GetAfazeres());
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
                    AfazeresConcluidos =
                        new ObservableCollection<ConclusaoAfazer>(
                            await FamiliarRestService.DefaultManager.RefreshConclusaoAfazerAsync());
                    var x =
                        new ObservableCollection<ValidacaoAfazer>(
                            await FamiliarRestService.DefaultManager.RefreshValidacaoAfazerAsync()).Where(
                            e => e.ValValidado == false).AsEnumerable();

                    var selection =
                        new ObservableCollection<Afazer>(await FamiliarRestService.DefaultManager.RefreshAfazerAsync())
                        .Where(e=>!x.Select(y=>y.ValAfazer).Contains(e.Id));
                    var result = selection.Where(e => AfazeresConcluidos.Select(m => m.ConAfazer)
                        .Contains(e.Id)).Where(e => pacresult.Select(m => m.Id).Contains(e.AfaPaciente)).AsEnumerable();
                    Afazeres = new ObservableCollection<Afazer>(result);
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