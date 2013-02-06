using System;
using System.Collections.Generic;
using System.Text;

namespace IntecoAG.ERM.Module {

    /// <summary>
    /// см. WS-HumanTask_v1.pdf, p.11
    /// Соглашение о хранении информации о пользователях, ролях и группах в строках.
    /// Формат хранения: строка1; строка2; строка3; и т.д.
    /// строкаi имеет вид (без кавычек)  "тип объекта: строковый идентификатор объекта"
    /// тип объекта = user | role | group
    /// </summary>
    public struct TaskParameters {

        public uint Priority;

        public List<string> ExcludedOwners;
        public List<string> PotentialOwners;
        public List<string> BusinessAdministrators;
        public List<string> PossibleDelegates;
        public List<string> TaskStakeholders;
        public List<string> NotificationRecipients;



        private string _ExcludedOwnerString;
        public string ExcludedOwnerString {
            get { return _ExcludedOwnerString; }
            set { 
                _ExcludedOwnerString = value;
                if (ExcludedOwners == null) ExcludedOwners = new List<string>();
                ExcludedOwners.Clear();
                Array mExcludedOwners = ExcludedOwnerString.Split(new string[] { CommonConstants.UserListSeparator }, StringSplitOptions.RemoveEmptyEntries);
                ProcessElements(mExcludedOwners, ExcludedOwners);
            }
        }

        public string _PotentialOwnerString;
        public string PotentialOwnerString {
            get { return _PotentialOwnerString; }
            set { 
                _PotentialOwnerString = value;
                if (PotentialOwners == null) PotentialOwners = new List<string>();
                PotentialOwners.Clear();
                Array mPotentialOwners = PotentialOwnerString.Split(new string[] { CommonConstants.UserListSeparator }, StringSplitOptions.RemoveEmptyEntries);
                ProcessElements(mPotentialOwners, PotentialOwners);
            }
        }

        public string _BusinessAdministratorString;
        public string BusinessAdministratorString {
            get { return _BusinessAdministratorString; }
            set { 
                _BusinessAdministratorString = value;
                if (BusinessAdministrators == null) BusinessAdministrators = new List<string>();
                BusinessAdministrators.Clear();
                Array mBusinessAdministrators = BusinessAdministratorString.Split(new string[] { CommonConstants.UserListSeparator }, StringSplitOptions.RemoveEmptyEntries);
                ProcessElements(mBusinessAdministrators, BusinessAdministrators);
            }
        }

        public string _PossibleDelegateString;
        public string PossibleDelegateString {
            get { return _PossibleDelegateString; }
            set { 
                _PossibleDelegateString = value;
                if (PossibleDelegates == null) PossibleDelegates = new List<string>();
                PossibleDelegates.Clear();
                Array mPossibleDelegates = PossibleDelegateString.Split(new string[] { CommonConstants.UserListSeparator }, StringSplitOptions.RemoveEmptyEntries);
                ProcessElements(mPossibleDelegates, PossibleDelegates);
            }
        }

        public string _TaskStakeholderString;
        public string TaskStakeholderString {
            get { return _TaskStakeholderString; }
            set { 
                _TaskStakeholderString = value;
                if (TaskStakeholders == null) TaskStakeholders = new List<string>();
                TaskStakeholders.Clear();
                Array mTaskStakeholders = TaskStakeholderString.Split(new string[] { CommonConstants.UserListSeparator }, StringSplitOptions.RemoveEmptyEntries);
                ProcessElements(mTaskStakeholders, TaskStakeholders);
            }
        }

        public string _NotificationRecipientString;
        public string NotificationRecipientString {
            get { return _NotificationRecipientString; }
            set { 
                _NotificationRecipientString = value;
                if (NotificationRecipients == null) NotificationRecipients = new List<string>();
                NotificationRecipients.Clear();
                Array mNotificationRecipients = NotificationRecipientString.Split(new string[] { CommonConstants.UserListSeparator }, StringSplitOptions.RemoveEmptyEntries);
                ProcessElements(mNotificationRecipients, NotificationRecipients);
            }
        }



        public string TaskInitiator;
        public string ActualOwner;

        public Guid CallBackObject;
        

        /// <summary>
        /// Заполнение коллекции пользователей из массива
        /// </summary>
        /// <param name="mArray"></param>
        /// <param name="UserAssociationTypeName"></param>
        /// <param name="ual"></param>
        public static void ProcessElements(Array mArray, List<string> userList) {
            foreach (string elem in mArray) {
                Array mElems = elem.Split(new string[] { CommonConstants.ElemSeparator }, StringSplitOptions.None);

                if (mElems.Length != 2) break;

                if (((string)(mElems.GetValue(0))).Trim() == UserAssociationType.user.ToString()) {
                    string uname = (string)(mElems.GetValue(1));
                    if (!string.IsNullOrEmpty(uname)) {
                        if (userList.IndexOf(uname) == -1) userList.Add(uname);
                    }
                }

                // Роли пока не обрабатываем
                if (((string)(mElems.GetValue(0))).Trim() == UserAssociationType.role.ToString()) {
                }

                // Группы пока не обрабатываем
                if (((string)(mElems.GetValue(0))).Trim() == UserAssociationType.group.ToString()) {
                }
            }
        }

    }
}
