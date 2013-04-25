using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.FinIndex;
using IntecoAG.ERM.Sync;

namespace IntecoAG.ERM.FM.Order {

    [MapInheritance(MapInheritanceType.ParentTable)]
    //    [Appearance("", AppearanceItemType.ViewItem, "Status != 'Project'", TargetItems="*", Enabled=false)]
    [NavigationItem("Finance")]
//    [DefaultProperty("Name")]
    [VisibleInReports]
    public class fmCOrderExt : fmCOrder, fmIOrderExt, SyncISyncObject {

        public fmCOrderExt(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            this.ComponentType = typeof(fmCOrderExt);
            this.CID = Guid.NewGuid();
            base.AfterConstruction();
        }

        #region fmCOrderFinIndexStructureItem

        [MapInheritance(MapInheritanceType.ParentTable)]
        public class fmCOrderFinIndexStructureItem : fmCFinIndexStructureItem {
            public fmCOrderFinIndexStructureItem(Session session)
                : base(session) {
            }

            private fmCOrderExt _Order;

            [Association("fmOrder-fmOrderFinIndexes")]
            public fmCOrderExt Order {
                get { return _Order; }
                set { SetPropertyValue<fmCOrderExt>("Order", ref _Order, value); }
            }

        }

        //public class fmCOrderManageDocCollection : XPCollection<fmCOrderManageDoc> {
        //    private fmCOrderExt _Order;

        //    public fmCOrderManageDocCollection(Session session, fmCOrderExt theOwner, XPMemberInfo refProperty) :
        //        base(session, theOwner, refProperty) {
        //        _Order = theOwner;
        //    }

        //    public fmCOrderExt Order {
        //        get { return _Order; }
        //    }

        //    public override int BaseAdd(object newObject) {
        //        if (!IsLoaded) {
        //        }
        //        return base.BaseAdd(newObject);
        //    }

        //    public override bool BaseRemove(object theObject) {
        //        return base.BaseRemove(theObject);
        //    }
        //}

        #endregion


        #region ПОЛЯ КЛАССА
        private Boolean _IsSyncRequired;
        private DateTime _ProjectOrderPayDate;

        [Persistent("ManageDocCurrent")]
        private fmCOrderManageDoc _ManageDocCurrent;
        [Persistent("ManageDocProject")]
        private fmCOrderManageDoc _ManageDocProject;
        //private fmCOrderManageDocCollection _ManageDocs;

        #endregion

        #region СВОЙСТВА КЛАССА
        [Action(Caption="Утвердить", AutoCommit = true, TargetObjectsCriteria="Status == 'Loaded'" )]
        public void Confirm() {
            if (Status == fmIOrderStatus.Loaded) {
                if (IsClosed)
                    Status = fmIOrderStatus.FinClosed;
//                else
//                    Status = fmIOrderStatus.Accepted;
            }
        }
//        public override Boolean ReadOnlyGet() {
//            return Status != fmIOrderStatus.Project && Status != fmIOrderStatus.Loaded;
//        }

        [PersistentAlias("_ManageDocCurrent")]
        public fmCOrderManageDoc ManageDocCurrent {
            get { return _ManageDocCurrent; }
        }

        [Appearance("", AppearanceItemType.ViewItem, "ManageDocProject == null", Visibility = ViewItemVisibility.Hide)]
        [PersistentAlias("_ManageDocProject")]
        public fmCOrderManageDoc ManageDocProject {
            get { return _ManageDocProject; }
        }

        public void ManageDocNew(fmCOrderManageDoc doc) {
//            if (Status == fmIOrderStatus.Project && _ManageDocProject == null)
//                Status = fmIOrderStatus.FinOpened;
//            else {
////                if (Status == fmIOrderStatus.FinOpened && _ManageDocProject == null)
////                    Status = fmIOrderStatus.Changes;
////                else
////                    throw new InvalidOperationException("Invalid ManageDocProject and status: " + Status.ToString());
//            } 
//            _ManageDocProject = doc;
//            OnChanged("ManageDocProject", null, doc);
        }

        public void ManageDocCancel(fmCOrderManageDoc doc) {
            //if (_ManageDocProject == doc) {
            //    if (Status == fmIOrderStatus.Opening || Status == fmIOrderStatus.Changes) {
            //        if (Status == fmIOrderStatus.Opening)
            //            Status = fmIOrderStatus.Project;
            //        if (Status == fmIOrderStatus.Changes)
            //            Status = fmIOrderStatus.Accepted;
            //        _ManageDocProject = null;
            //        OnChanged("ManageDocProject", doc, null);
            //    } else
            //        throw new InvalidOperationException("Invalid status: " + Status.ToString());
            //} else
            //    throw new InvalidOperationException("Invalid Doc ManageDocCurrent");
        }

        public void ManageDocComplete(fmCOrderManageDoc doc) {
            //if (_ManageDocProject == doc) {
            //    if (Status == fmIOrderStatus.Opening || Status == fmIOrderStatus.Changes) {
            //         Status = fmIOrderStatus.Accepted;
            //         CopyFrom(doc);
            //         fmCOrderManageDoc old = _ManageDocCurrent;
            //         _ManageDocCurrent = doc;
            //         _ManageDocProject = null;
            //         OnChanged("ManageDocCurrent", old, doc);
            //         OnChanged("ManageDocProject", doc, null);
            //    } else
            //        throw new InvalidOperationException("Invalid status: " + Status.ToString());
            //} else
            //    throw new InvalidOperationException("Invalid Doc ManageDocCurrent");
        }

        [Appearance("", AppearanceItemType.Action, "", TargetItems = "Delete", Enabled = false)]
//        [Appearance("", AppearanceItemType.Action, "ManageDocProject != null", TargetItems = "New", Enabled = false)]
        [Appearance("", AppearanceItemType.Action, "", TargetItems = "New", Enabled = false)]
        [Aggregated]
        [Association("fmOrder-OrderManageDoc", typeof(fmCOrderManageDoc))]
        public XPCollection<fmCOrderManageDoc> ManageDocs {
            get {
                return GetCollection<fmCOrderManageDoc>("ManageDocs");
            }
        }
        //public fmCOrderManageDocCollection ManageDocs {
        //    get {
        //        if (_ManageDocs == null)
        //            _ManageDocs = new fmCOrderManageDocCollection(Session, this,
        //                ClassInfo.GetMember("ManageDocs"));
        //        return _ManageDocs;
        //    }
        //}

        IList<fmIOrderManageDoc> fmIOrderExt.ManageDocs {
            get { return new ListConverter<fmIOrderManageDoc, fmCOrderManageDoc>(this.ManageDocs); }
        }

        [Aggregated]
        [Association("fmOrder-fmOrderFinIndexes", typeof(fmCOrderFinIndexStructureItem))]
        public XPCollection<fmCOrderFinIndexStructureItem> FinIndexes {
            get {
                XPCollection<fmCOrderFinIndexStructureItem> col = GetCollection<fmCOrderFinIndexStructureItem>("FinIndexes");
                //                col.Sorting.Add(new SortProperty("SortOrder", DevExpress.Xpo.DB.SortingDirection.Ascending));
                return col;
            }
        }

        public DateTime ProjectOrderPayDate {
            get {
                return _ProjectOrderPayDate;
            }
            set {
                SetPropertyValue<DateTime>("ProjectOrderPayDate", ref _ProjectOrderPayDate, value);
            }
        }
        #endregion


        public void CopyFrom(fmCOrderManageDoc doc) {
            doc.CopyTo(this);
            ((fmIFinIndexStructure)this).Copy(doc);
        }

        #region fmIFinStructure

        IList<fmIFinIndexStructureItem> fmIFinIndexStructure.FinIndexes {
            get { return new ListConverter<fmIFinIndexStructureItem, fmCOrderFinIndexStructureItem>(FinIndexes); }
        }

        /// <summary>
        /// Паша!!! Пока работаем с сессией потом нужен будет ObjectSpace
        /// </summary>
        /// <param name="fin_index"></param>
        /// <returns></returns>
        public fmIFinIndexStructureItem FinIndexesCreateItem(fmCFinIndex fin_index) {
            fmCOrderFinIndexStructureItem item = new fmCOrderFinIndexStructureItem(this.Session) {
                FinIndex = fin_index
            };
            FinIndexes.Add(item);
            return item;
        }

        public void UpdateIndexStructure(IList<fmCFinIndex> index_col) {
            fmIFinIndexStructureLogic.UpdateIndexStructure(this, index_col);
        }

        void fmIFinIndexStructure.Copy(fmIFinIndexStructure from) {
            fmIFinIndexStructureLogic.Copy(this, from);
        }
        
        #endregion

        public Boolean IsSyncRequired {
            get {
                return _IsSyncRequired;
            }
            set {
                SetPropertyValue<Boolean>("IsSyncRequired", ref _IsSyncRequired, value);
            }
        }
    }

}
