using System;
using System.Collections.ObjectModel;
using FreshMvvm;
using HandyCareFamiliar.Data;
using HandyCareFamiliar.Helper;
using HandyCareFamiliar.Model;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareFamiliar.PageModel
{
    [ImplementPropertyChanged]
    public class AfazerPageModel : FreshBasePageModel
    {
        public Afazer Afazer { get; set; }
        public bool NewItem { get; set; }
        public Material oMaterial { get; set; }
        public Medicamento oMedicamento { get; set; }
        public Paciente Paciente { get; set; }
        public PageModelHelper oHorario { get; set; }
        public MaterialUtilizado oMaterialUtilizado { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }
        public MedicamentoAdministrado oMedicamentoAdministrado { get; set; }
        public ConclusaoAfazer AfazerConcluido { get; set; }
        public ObservableCollection<Material> Materiais { get; set; }
        public ObservableCollection<Material> MateriaisEscolhidos { get; set; }
        public ObservableCollection<Medicamento> Medicamentos { get; set; }
        public ObservableCollection<MaterialUtilizado> MateriaisUtilizados { get; set; }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (NewItem)
                        Afazer.Id = Guid.NewGuid().ToString();
                    oHorario.Visualizar = false;
                    oHorario.ActivityRunning = true;
                    Afazer.AfaPaciente = CuidadorPaciente.Id;
                    Afazer.AfaHorarioPrevisto = oHorario.Data.Date + oHorario.Horario;
                    await FamiliarRestService.DefaultManager.SaveAfazerAsync(Afazer, true);
                    if (oMaterial != null)
                    {
                        oMaterialUtilizado = new MaterialUtilizado
                        {
                            MatAfazer = Afazer.Id,
                            MatUtilizado = oMaterial.Id,
                            MatQuantidadeUtilizada = Convert.ToInt32(oHorario.Quantidade),
                            Id = Guid.NewGuid().ToString()
                        };
                        oMaterial.MatQuantidade -= oMaterialUtilizado.MatQuantidadeUtilizada;
                        oMaterial.MaterialUtilizado.Add(oMaterialUtilizado);
                        await FamiliarRestService.DefaultManager.SaveMaterialAsync(oMaterial, false);
                        await FamiliarRestService.DefaultManager.SaveMaterialUtilizadoAsync(oMaterialUtilizado, true);
                    }
                    if (oMedicamento != null)
                    {
                        oMedicamentoAdministrado = new MedicamentoAdministrado
                        {
                            MedAfazer = Afazer.Id,
                            MedAdministrado = oMedicamento.Id,
                            MemQuantidadeAdministrada = Convert.ToInt32(oHorario.Quantidade),
                            Id = Guid.NewGuid().ToString()
                        };
                        oMedicamento.MedQuantidade -= oMedicamentoAdministrado.MemQuantidadeAdministrada;
                        await FamiliarRestService.DefaultManager.SaveMedicamentoAsync(oMedicamento, false);
                        await
                            FamiliarRestService.DefaultManager.SaveMedicamentoAdministradoAsync(
                                oMedicamentoAdministrado, true);
                    }
                    await CoreMethods.PopPageModel(Afazer);
                });
            }
        }

        public Command ConcluirCommand
        {
            get
            {
                return new Command(async () =>
                {
                    oHorario.Visualizar = false;
                    oHorario.ActivityRunning = true;
                    AfazerConcluido.ConConcluido = false;
                    AfazerConcluido.ConHorarioConcluido = DateTime.Now;
                    AfazerConcluido.ConAfazer = Afazer.Id;
                    await FamiliarRestService.DefaultManager.SaveConclusaoAfazerAsync(AfazerConcluido, true);
                    await CoreMethods.PopPageModel(Afazer);
                });
            }
        }

        public Command DeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    oHorario.Visualizar = false;
                    oHorario.ActivityRunning = true;
                    await FamiliarRestService.DefaultManager.DeleteAfazerAsync(Afazer);
                    await CoreMethods.PopPageModel(Afazer);
                });
            }
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            oHorario = new PageModelHelper
            {
                HabilitarMaterial = false,
                HabilitarMedicamento = false,
                deleteVisible = false
            };

            var x = initData as Tuple<Afazer, Paciente, CuidadorPaciente>;
            Afazer = new Afazer();
            Paciente = new Paciente();
            CuidadorPaciente = new CuidadorPaciente();
            if (x != null)
            {
                if (x.Item1 != null)
                {
                    Afazer = x.Item1;
                    NewItem = false;
                }
                else
                {
                    NewItem = true;
                }
                Paciente = x.Item2;
                CuidadorPaciente = x.Item3;
            }
            Materiais =
                new ObservableCollection<Material>(await FamiliarRestService.DefaultManager.RefreshMaterialAsync());
            Medicamentos =
                new ObservableCollection<Medicamento>(await FamiliarRestService.DefaultManager.RefreshMedicamentoAsync());
            MateriaisUtilizados =
                new ObservableCollection<MaterialUtilizado>(
                    await FamiliarRestService.DefaultManager.RefreshMaterialUtilizadoAsync(Afazer?.Id));
            if (Afazer?.Id != null)
                oHorario.deleteVisible = true;
            AfazerConcluido = new ConclusaoAfazer();
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            if (Afazer?.Id == null)
            {
                //oHorario.deleteVisible = false;
                oHorario.Data = DateTime.Now;
                oHorario.Horario = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            }
            else
            {
                oHorario.Data = Afazer.AfaHorarioPrevisto;
                oHorario.Horario = Afazer.AfaHorarioPrevisto.TimeOfDay;
                //oHorario.deleteVisible = true;
            }
        }
    }
}