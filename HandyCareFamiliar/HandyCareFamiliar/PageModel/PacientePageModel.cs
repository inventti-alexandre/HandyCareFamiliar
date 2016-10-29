using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class PacientePageModel : FreshBasePageModel
    {
        public Paciente Paciente { get; set; }
        public PacienteFamiliar PacienteFamiliar { get; set; }
        public MotivoCuidado MotivoCuidado { get; set; }
        public Familiar Familiar { get; set; }
        public TipoTratamento TipoTratamento { get; set; }
        public PeriodoTratamento PeriodoTratamento { get; set; }
        public PageModelHelper oHorario { get; set; }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (oHorario.VisualizarTermino == false)
                            PeriodoTratamento.PerTermino = null;
                        else
                            PeriodoTratamento.PerTermino = oHorario.Data;
                        oHorario.Visualizar = false;
                        oHorario.ActivityRunning = true;
                        Paciente.Id = Guid.NewGuid().ToString();
                        PacienteFamiliar = new PacienteFamiliar {Id = Guid.NewGuid().ToString()};
                        MotivoCuidado.Id = Guid.NewGuid().ToString();
                        PacienteFamiliar.PacId = Paciente.Id;
                        PacienteFamiliar.FamId = Familiar.Id;
                        TipoTratamento.Id = Guid.NewGuid().ToString();
                        TipoTratamento.TipCuidado = MotivoCuidado.Id;
                        PeriodoTratamento.Id = Guid.NewGuid().ToString();
                        //PacienteFamiliar.CuiPeriodoTratamento = PeriodoTratamento.Id;
                        Paciente.PacMotivoCuidado = MotivoCuidado.Id;
                        await Task.Run(async () =>
                        {
                            await
                                FamiliarRestService.DefaultManager.SaveMotivoCuidadoAsync(MotivoCuidado,
                                    oHorario.NovoPaciente);
                            await
                                FamiliarRestService.DefaultManager.SavePeriodoTratamentoAsync(PeriodoTratamento,
                                    oHorario.NovoPaciente);
                            await FamiliarRestService.DefaultManager.SavePacienteAsync(Paciente, oHorario.NovoPaciente);
                            await
                                FamiliarRestService.DefaultManager.SaveTipoTratamentoAsync(TipoTratamento,
                                    oHorario.NovoPaciente);
                            await
                                FamiliarRestService.DefaultManager.SavePacienteFamiliarAsync(PacienteFamiliar,
                                    oHorario.NovoPaciente);
                        });
                        await CoreMethods.PopPageModel(Paciente);
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.WriteLine(e.ToString());
                        throw;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                        throw;
                    }
                });
            }
        }

        public Command DeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await FamiliarRestService.DefaultManager.DeletePacienteAsync(Paciente);
                    await CoreMethods.PopPageModel(Paciente);
                });
            }
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            Familiar = new Familiar();
            Paciente = new Paciente();
            PeriodoTratamento = new PeriodoTratamento
            {
                PerInicio = DateTime.Now,
                PerTermino = DateTime.Now
            };
            TipoTratamento = new TipoTratamento();
            MotivoCuidado = new MotivoCuidado();
            oHorario = new PageModelHelper {ActivityRunning = true, Visualizar = false};
            var x = initData as Tuple<Paciente, bool, Familiar>;
            if (x != null)
            {
                oHorario.NovoPaciente = x.Item2;
                oHorario.DadoPaciente = false;
                Familiar = x.Item3;
            }
            if (oHorario.NovoPaciente == false)
            {
                if (x?.Item1 != null)
                {
                    Paciente = x.Item1;
                    await GetInfo();
                }
                oHorario.DadoPaciente = true;
            }
            oHorario.ActivityRunning = false;
            oHorario.Visualizar = true;
        }

        private async Task GetInfo()
        {
            try
            {
                await Task.Run(async () =>
                {
                    var pacresult = new ObservableCollection<PacienteFamiliar>(
                            await FamiliarRestService.DefaultManager.RefreshPacienteFamiliarAsync())
                        .FirstOrDefault(e => e.PacId == Paciente.Id);
                    //PeriodoTratamento = new ObservableCollection<PeriodoTratamento>(
                    //        await FamiliarRestService.DefaultManager.RefreshPeriodoTratamentoAsync())
                    //    .FirstOrDefault(e => e.Id == pacresult.CuiPeriodoTratamento);
                    if (PeriodoTratamento.PerTermino != null)
                    {
                        oHorario.Data = PeriodoTratamento.PerTermino.Value;
                        oHorario.VisualizarTermino = true;
                    }
                    var selection =
                        new ObservableCollection<MotivoCuidado>(
                            await FamiliarRestService.DefaultManager.RefreshMotivoCuidadoAsync());
                    MotivoCuidado = selection.FirstOrDefault(e => e.Id.Contains(Paciente.PacMotivoCuidado));
                    TipoTratamento = new ObservableCollection<TipoTratamento>(
                            await FamiliarRestService.DefaultManager.RefreshTipoTratamentoAsync())
                        .FirstOrDefault(e => e.TipCuidado.Contains(MotivoCuidado.Id));
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            //oHorario = new PageModelHelper();
        }
    }
}