using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
//
//
namespace IntecoAG.ERM.Trw.Subject {

//
    public partial class TrwSubjectVC : ViewController {
        public TrwSubjectVC() {
            InitializeComponent();
            RegisterActions(components);
        }

        private TrwSubjectImportDealActionParameters ImportActionParameters;

        private void ImportSaleDealsAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {
            TrwSubject trw_subj = e.CurrentObject as TrwSubject;
            if (trw_subj == null) return;
            
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                trw_subj = os.GetObject<TrwSubject>(trw_subj);

                TrwSubjectLogic.FillSaleDeals(os, trw_subj, trw_subj.Period.Year, 
                    ImportActionParameters.MaxCount, ImportActionParameters.VolumePercent);
                os.CommitChanges();
            }
            ImportActionParameters = null;
        }


        private void ImportSaleDealsAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            IObjectSpace os = Application.CreateObjectSpace();
            ImportActionParameters = new TrwSubjectImportDealActionParameters();
            ImportActionParameters.MaxCount = 0;
            ImportActionParameters.VolumePercent = 0;
            e.View = Application.CreateDetailView(os, ImportActionParameters);
//                new CollectionSource(objectSpace, typeof(Note)), true);

        }

        private void ImportBayDealsAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {
            TrwSubject trw_subj = e.CurrentObject as TrwSubject;
            if (trw_subj == null) return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                trw_subj = os.GetObject<TrwSubject>(trw_subj);
                TrwSubjectLogic.FillBayDeals(os, trw_subj, trw_subj.Period.Year,
                    ImportActionParameters.MaxCount, ImportActionParameters.VolumePercent);
                os.CommitChanges();
            }
            ImportActionParameters = null;
        }

        private void ImportBayDealsAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            IObjectSpace os = Application.CreateObjectSpace();
            ImportActionParameters = new TrwSubjectImportDealActionParameters();
            ImportActionParameters.MaxCount = 5;
            ImportActionParameters.VolumePercent = 0.6M;
            e.View = Application.CreateDetailView(os, ImportActionParameters);
        }
    }

    [NonPersistent]
    public class TrwSubjectImportDealActionParameters{
        public TrwSubjectImportDealActionParameters() : base() { }

        private Int32 _MaxCount;
        public Int32 MaxCount {
            get { return _MaxCount; }
//            set { SetPropertyValue<Int32>("MaxCount", ref _MaxCount, value); }
            set { _MaxCount = value; }
        }

        private Decimal _VolumePercent;
//        [ModelDefault]
        public Decimal VolumePercent {
            get { return _VolumePercent; }
            //            set { SetPropertyValue<Decimal>("VolumePercent", ref _VolumePercent, value); }
            set { _VolumePercent = value; }
        }
    }
}
