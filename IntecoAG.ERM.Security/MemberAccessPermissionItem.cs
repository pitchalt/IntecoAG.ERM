using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp.Security;
using System.ComponentModel;
using DevExpress.Xpo;
using System.Security;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.Security {
    public enum MemberOperation { NotAssigned, Read, Write }

    public class MemberAccessPermissionItem {
        private string memberName;
        private Type objectType;
        private MemberOperation operation;
        private ObjectAccessModifier modifier;
        public MemberAccessPermissionItem() { }
        public MemberAccessPermissionItem(MemberAccessPermissionItem source) {
            this.memberName = source.memberName;
            this.objectType = source.objectType;
            this.operation = source.operation;
            this.modifier = source.modifier;
        }
        public Type ObjectType {
            get { return objectType; }
            set { objectType = value; }
        }
        public string MemberName {
            get { return memberName; }
            set { memberName = value; }
        }
        public MemberOperation Operation {
            get { return operation; }
            set { operation = value; }
        }
        public ObjectAccessModifier Modifier {
            get { return modifier; }
            set { modifier = value; }
        }
    }

    [NonPersistent, DefaultProperty("DisplayName")]
    public class MemberAccessPermission : PermissionBase {
        public string DisplayName { get { return this.ToString(); } }
        private List<MemberAccessPermissionItem> items = new List<MemberAccessPermissionItem>();
        private MemberAccessPermissionItem GetDesignModeItem() {
            if (items.Count > 1) {
                throw new InvalidOperationException();
            }
            if (items.Count == 0) {
                items.Add(new MemberAccessPermissionItem());
            }
            return items[0];
        }
        private List<MemberAccessPermissionItem> CloneItems() {
            List<MemberAccessPermissionItem> clonedItems = new List<MemberAccessPermissionItem>();
            foreach (MemberAccessPermissionItem item in items) {
                clonedItems.Add(new MemberAccessPermissionItem(item));
            }
            return clonedItems;
        }
        public MemberAccessPermission() { }
        public MemberAccessPermission(Type objectType, string memberName, MemberOperation operation)
            : this(objectType, memberName, operation, ObjectAccessModifier.Allow) {
        }
        public MemberAccessPermission(Type objectType, string memberName, MemberOperation operation, ObjectAccessModifier modifier) {
            this.ObjectType = objectType;
            this.MemberName = memberName;
            this.Operation = operation;
            this.Modifier = modifier;
        }
        public override System.Security.IPermission Union(System.Security.IPermission target) {
            MemberAccessPermission result = (MemberAccessPermission)Copy();
            result.items.AddRange(((MemberAccessPermission)target).CloneItems());
            return result;
        }
        public override bool IsSubsetOf(System.Security.IPermission target) {
            if (base.IsSubsetOf(target)) {
                foreach (MemberAccessPermissionItem targetItem in ((MemberAccessPermission)target).items) {
                    if (targetItem.ObjectType == ObjectType
                        && targetItem.MemberName == MemberName
                        && targetItem.Operation == Operation) {
                        return targetItem.Modifier == Modifier;
                    }
                }
                return true;
            }
            return false;
        }
        [TypeConverter(typeof(PermissionTargetBusinessClassListConverter))]
        public Type ObjectType {
            get { return GetDesignModeItem().ObjectType; }
            set { GetDesignModeItem().ObjectType = value; }
        }
        public string MemberName {
            get { return GetDesignModeItem().MemberName; }
            set { GetDesignModeItem().MemberName = value; }
        }
        public MemberOperation Operation {
            get { return GetDesignModeItem().Operation; }
            set { GetDesignModeItem().Operation = value; }
        }
        public ObjectAccessModifier Modifier {
            get { return GetDesignModeItem().Modifier; }
            set { GetDesignModeItem().Modifier = value; }
        }
        public override System.Security.SecurityElement ToXml() {
            SecurityElement result = base.ToXml();
            SecurityElement itemElement = new SecurityElement("MemberAccessPermissionItem");
            itemElement.AddAttribute("Operation", Operation.ToString());
            itemElement.AddAttribute("ObjectType", (ObjectType != null) ? ObjectType.ToString() : "");
            itemElement.AddAttribute("Modifier", Modifier.ToString());
            itemElement.AddAttribute("MemberName", MemberName.ToString());
            result.AddChild(itemElement);
            return result;
        }
        public override void FromXml(System.Security.SecurityElement element) {
            items.Clear();
            if (element.Children != null) {
                if (element.Children.Count != 1) {
                    throw new InvalidOperationException();
                }
                SecurityElement childElement = (SecurityElement)element.Children[0];
                ObjectType = ReflectionHelper.FindType(childElement.Attributes["ObjectType"].ToString());
                Operation = (MemberOperation)Enum.Parse(typeof(MemberOperation), childElement.Attributes["Operation"].ToString());
                Modifier = (ObjectAccessModifier)Enum.Parse(typeof(ObjectAccessModifier), childElement.Attributes["Modifier"].ToString());
                MemberName = childElement.Attributes["MemberName"].ToString();
            }
        }
        public override string ToString() {
            return ((ObjectType != null) ? ObjectType.Name : "N/A") + "." + MemberName + " - " + Modifier + " " + Operation;
            //return base.ToString();
        }
        public override System.Security.IPermission Copy() {
            MemberAccessPermission result = new MemberAccessPermission();
            result.items.AddRange(CloneItems());
            return result;
        }
    }
}