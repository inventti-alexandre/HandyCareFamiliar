using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class ConclusaoAfazerPageModel : FreshBasePageModel
    {
        //IAfazerRestService _restService;
        //IMaterialRestService _materialRestService;
        //IConclusaoAfazerRestService _concluirRestService;
        //IMaterialUtilizadoRestService _materialUtilizadoRestService;
        //IMedicamentoRestService _medicamentoRestService;
        //private IMedicamentoAdministradoRestService _medicamentoAdministradoRestService;
        public Afazer Afazer { get; set; }
        public bool newItem { get; set; }
        public Material oMaterial { get; set; }
        public Medicamento oMedicamento { get; set; }
        public PageModelHelper oHorario { get; set; }
        public ConclusaoAfazer ConclusaoAfazer { get; set; }
        public ValidacaoAfazer ValidacaoAfazer { get; set; }
        public MaterialUtilizado oMaterialUtilizado { get; set; }
        public ConclusaoAfazer AfazerConcluido { get; set; }
        public ObservableCollection<Material> Materiais { get; set; }
        public ObservableCollection<Material> MateriaisEscolhidos { get; set; }
        public MotivoNaoValidacaoConclusaoAfazer MotivoNaoValidacaoConclusaoAfazer { get; set; }
        public ObservableCollection<Medicamento> Medicamentos { get; set; }
        public PacienteFamiliar PacienteFamiliar { get; set; }
        public ObservableCollection<MedicamentoAdministrado> MedicamentosAdministrados { get; set; }
        public ObservableCollection<MaterialUtilizado> MateriaisUtilizados { get; set; }

        public override async void Init(object initData)
        {
            base.Init(initData);
            Afazer = new Afazer();
            var detalheAfazer = initData as Tuple<Afazer, ConclusaoAfazer, PacienteFamiliar>;
            if (detalheAfazer != null)
            {
                ConclusaoAfazer=new ConclusaoAfazer();
                ConclusaoAfazer = detalheAfazer.Item2;
                PacienteFamiliar=new PacienteFamiliar();
                PacienteFamiliar = detalheAfazer.Item3;
                Afazer = detalheAfazer.Item1;
                MateriaisUtilizados =
                    new ObservableCollection<MaterialUtilizado>(
                        await FamiliarRestService.DefaultManager.RefreshMaterialUtilizadoAsync(detalheAfazer?.Item1.Id));
                MedicamentosAdministrados =
                    new ObservableCollection<MedicamentoAdministrado>(
                        await
                            FamiliarRestService.DefaultManager.RefreshMedicamentoAdministradoAsync(
                                detalheAfazer?.Item1.Id));
            }
            oMaterial = new ObservableCollection<Material>(
                await FamiliarRestService.DefaultManager.RefreshMaterialExistenteAsync()).FirstOrDefault(
                m => MateriaisUtilizados.Select(n => n.MatUtilizado).Contains(m.Id));
            oMedicamento = new ObservableCollection<Medicamento>(
                await FamiliarRestService.DefaultManager.RefreshMedicamentoAsync()).FirstOrDefault(
                m => MedicamentosAdministrados.Select(n => n.MedAdministrado).Contains(m.Id));
        }
        public Command ValidarAfazer
        {
            get
            {
                return new Command(async () =>
                {
                    var result = await CoreMethods.DisplayActionSheet("O afazer foi finalizado corretamente?",
    "Cancelar", null, "Sim", "Não");
                    switch (result)
                    {
                        case "Sim":
                            {
                                ValidacaoAfazer = new ValidacaoAfazer
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ValAfazer = ConclusaoAfazer.Id,
                                    ValFamId = PacienteFamiliar.FamId,
                                    ValValidado = true
                                };
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
                                oHorario.Visualizar = false;
                                oHorario.ActivityRunning = true;
                                ValidacaoAfazer = new ValidacaoAfazer
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ValAfazer = ConclusaoAfazer.Id,
                                    ValFamId = PacienteFamiliar.FamId,
                                    ValValidado = false
                                };
                                MotivoNaoValidacaoConclusaoAfazer = new MotivoNaoValidacaoConclusaoAfazer
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    MoValidacao = ValidacaoAfazer.Id,
                                    MoDescricao = resulto.Text
                                };
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

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            oHorario = new PageModelHelper
            {
                HabilitarMaterial = false,
                HabilitarMedicamento = false
            };
            if (Afazer == null)
            {
                oHorario.deleteVisible = false;
                oHorario.Data = DateTime.Now;
                oHorario.Horario = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            }
            else
            {
                oHorario.Data = Afazer.AfaHorarioPrevisto;
                oHorario.Horario = Afazer.AfaHorarioPrevisto.TimeOfDay;
                oHorario.deleteVisible = true;
            }
        }
    }
}