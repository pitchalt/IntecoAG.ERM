using System;
using System.Xml;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.Xpo.Metadata;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.Base.General;

namespace IntecoAG.ERM.Module {

    //[DefaultClassOptions]
    [Persistent]
    public class AppUserActivity : BasePersistentObject, IEvent, IRecurrentEvent {

        private bool _AllDay;
        private string _Description;
        private DateTime _StartOn;
        private DateTime _EndOn;
        private int _Label;
        private string _Location;
        private int _Status;
        private string _Subject;
        private int _Type;
        private string _RecurrenceInfoXml;
        [Persistent("ResourceIds"), Size(SizeAttribute.Unlimited)]
        private string _AppUserIds;
        [Persistent("RecurrencePattern")]
        private AppUserActivity _RecurrencePattern;

        public AppUserActivity(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            StartOn = DateTime.Now;
            EndOn = StartOn.AddHours(1);
            AppUsers.Add(Session.GetObjectByKey<AppUser>(SecuritySystem.CurrentUserId));
        }

        [Association("Activity-AppUsers", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<AppUser> AppUsers {
            get { return GetCollection<AppUser>("AppUsers"); }
        }

        protected override XPCollection<T> CreateCollection<T>(XPMemberInfo property) {
            XPCollection<T> result = base.CreateCollection<T>(property);
            if (property.Name == "AppUsers") {
                result.ListChanged += AppUsers_ListChanged;
            }
            return result;
        }

        public void UpdateAppUserIds() {
            _AppUserIds = string.Empty;
            foreach (AppUser activityUser in AppUsers) {
                _AppUserIds += String.Format(@"<ResourceId Type=""{0}"" Value=""{1}"" />", activityUser.Id.GetType().FullName, activityUser.Id);
            }
            _AppUserIds = String.Format("<ResourceIds>{0}</ResourceIds>", _AppUserIds);
        }

        private void UpdateAppUsers() {
            AppUsers.SuspendChangedEvents();
            try {
                while (AppUsers.Count > 0) AppUsers.Remove(AppUsers[0]);
                if (!String.IsNullOrEmpty(_AppUserIds)) {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(_AppUserIds);
                    foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes) {
                        AppUser activityUser = Session.GetObjectByKey<AppUser>(new Guid(xmlNode.Attributes["Value"].Value));
                        if (activityUser != null)
                            AppUsers.Add(activityUser);
                    }
                }
            } finally {
                AppUsers.ResumeChangedEvents();
            }
        }

        private void AppUsers_ListChanged(object sender, ListChangedEventArgs e) {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted) {
                UpdateAppUserIds();
                OnChanged("ResourceId");
            }
        }

        protected override void OnLoaded() {
            base.OnLoaded();
            if (AppUsers.IsLoaded && !Session.IsNewObject(this))
                AppUsers.Reload();
        }

        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("EventIntervalValid", DefaultContexts.Save, "The start date must be less than the end date", SkipNullOrEmptyValues = false, UsedProperties = "StartOn, EndOn")]
        public bool IsIntervalValid { get { return StartOn <= EndOn; } }


        #region IEvent Members

        public bool AllDay {
            get { return _AllDay; }
            set { SetPropertyValue("AllDay", ref _AllDay, value); }
        }

        [Browsable(false), NonPersistent]
        public object AppointmentId {
            get { return Oid; }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }

        public int Label {
            get { return _Label; }
            set { SetPropertyValue("Label", ref _Label, value); }
        }

        public string Location {
            get { return _Location; }
            set { SetPropertyValue("Location", ref _Location, value); }
        }

        [PersistentAlias("_AppUserIds"), Browsable(false)]
        public string ResourceId {
            get {
                if (_AppUserIds == null)
                    UpdateAppUserIds();
                return _AppUserIds;
            }
            set {
                if (_AppUserIds != value && value != null) {
                    _AppUserIds = value;
                    UpdateAppUsers();
                }
            }
        }

        [Indexed]
        [Custom("DisplayFormat", "{0:G}")]
        [Custom("EditMask", "G")]
        public DateTime StartOn {
            get { return _StartOn; }
            set { SetPropertyValue("StartOn", ref _StartOn, value); }
        }

        [Indexed]
        [Custom("DisplayFormat", "{0:G}")]
        [Custom("EditMask", "G")]
        public DateTime EndOn {
            get { return _EndOn; }
            set { SetPropertyValue("EndOn", ref _EndOn, value); }
        }

        public int Status {
            get { return _Status; }
            set { SetPropertyValue("Status", ref _Status, value); }
        }

        [Size(250)]
        public string Subject {
            get { return _Subject; }
            set { SetPropertyValue("Subject", ref _Subject, value); }
        }

        [Browsable(false)]
        public int Type {
            get { return _Type; }
            set { SetPropertyValue("Type", ref _Type, value); }
        }

        #endregion


        #region IRecurrentEvent Members

        [DevExpress.Xpo.DisplayName("Recurrence"), Size(SizeAttribute.Unlimited)]
        public string RecurrenceInfoXml {
            get { return _RecurrenceInfoXml; }
            set { SetPropertyValue("RecurrenceInfoXml", ref _RecurrenceInfoXml, value); }
        }

        [Browsable(false)]
        [PersistentAlias("_RecurrencePattern")]
        public IRecurrentEvent RecurrencePattern {
            get { return _RecurrencePattern; }
            set { SetPropertyValue("RecurrencePattern", ref _RecurrencePattern, value as AppUserActivity); }
        }

        #endregion
    }
} 
 
