using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

using DevExpress.Xpo;
using DevExpress.ExpressApp.Security;
using System.Security.Permissions;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.Security;
using DevExpress.ExpressApp.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Objects;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;

namespace IntecoAG.ERM.Security {

    // http://community.devexpress.com/forums/t/91461.aspx

        [NonPersistent]
        public class ConditionalObjectAccessPermission : ObjectAccessPermission {
            public ConditionalObjectAccessPermission() { }
            public ConditionalObjectAccessPermission(PermissionState permissionState) : base(permissionState) { }
            public ConditionalObjectAccessPermission(Type objectType, ObjectAccess access) : base(objectType, access) { }
            public ConditionalObjectAccessPermission(Type objectType, ObjectAccess access, SecurityContextList contexts) : base(objectType, access, contexts) { }
            public ConditionalObjectAccessPermission(Type objectType, ObjectAccess objectAccess, ObjectAccessModifier modifier) : base(objectType, objectAccess, modifier) { }
            public ConditionalObjectAccessPermission(Type objectType, ObjectAccess access, params DevExpress.ExpressApp.Security.SecurityContext[] securityContexts) : base(objectType, access, securityContexts) { }
    
            const string conditionAttr = "condition";

            public override SecurityElement ToXml() {
                SecurityElement result = base.ToXml();
                if (result != null && condition != null)
                    foreach(SecurityElement particularAccessItemElement in result.Children) 
                        particularAccessItemElement.AddAttribute(conditionAttr, SecurityElement.Escape(condition));
                return result;
            }

            public override void FromXml(SecurityElement element) {
                base.FromXml(element);
                condition = null;
                if (element != null)
                    foreach(SecurityElement particularAccessItemElement in element.Children) {
                        string tempCondition = particularAccessItemElement.Attribute(conditionAttr);
                        if (condition != null && condition != tempCondition)
                            throw new ArgumentException("Stored particular access item conditions do not match!");
                        condition = tempCondition ?? condition;
                    }
                var newAccessList = AccessItemList.Select(item => new ConditionalParticularAccessItem(item.ObjectType, item.Access, item.Modifier, condition)).ToList();
                AccessItemList.Clear();
                foreach (ParticularAccessItem item in newAccessList)
                    AccessItemList.Add(item);
            }
    
            string condition = string.Empty;
            [CriteriaObjectTypeMember("ObjectType"), Size(-1), ImmediatePostData]
            public string Condition {
                get { return condition; }
                set { condition = value; }
            }

            public override string ToString() {
                return string.IsNullOrEmpty(condition) ? base.ToString() : string.Format("{0} ({1})", base.ToString(), condition);
            }

            public override IPermission Copy() {
                ConditionalObjectAccessPermission result = new ConditionalObjectAccessPermission { ObjectType = ObjectType };
                foreach(ConditionalParticularAccessItem item in AccessItemList)
                    result.AccessItemList.Add(item);
                return result;
            }

            public override IPermission Union(IPermission target) {
                return Union<ConditionalObjectAccessPermission>(target);
            }

            public virtual IPermission Union<TActualResultType>(IPermission target) where TActualResultType : ObjectAccessPermission {
                try {
                    if (!(target is ObjectAccessPermission))
                        throw new ArgumentException("Can't unite anything other than an ObjectAccessPermission or one of its descendants!");
                    if (!typeof(ObjectAccessPermission).IsAssignableFrom(typeof(TActualResultType)))
                        throw new ArgumentException("Resultant object Type must be an ObjectAccessPermission or one of its descendants!");
                    List<ParticularAccessItem> resultItems = new List<ParticularAccessItem>();
                    IEnumerable<ParticularAccessItem> allItems = AccessItemList.Union(((ObjectAccessPermission)target).AccessItemList);
                    if (target is ConditionalObjectAccessPermission)
                        resultItems.AddRange(allItems.Distinct());
                    else
                        foreach (ParticularAccessItem item in allItems) {
                            // only process items not already stored in the result set
                            if (!resultItems.Exists(i => i.ObjectType == item.ObjectType && i.Access == item.Access)) {
                                // a conditional item (with an actual condition) has precedence over unconditional items...
                                // NOTE: multiple non mutually-exclusive conditional items will be ignored!
                                ConditionalParticularAccessItem conditionalItem = item as ConditionalParticularAccessItem;
                                if (conditionalItem == null || conditionalItem != null && string.IsNullOrEmpty(conditionalItem.Condition)) {
                                    var duplicateItems = allItems.Where(i => i.ObjectType == item.ObjectType && i.Access == item.Access && !object.ReferenceEquals(i, item));
                                    conditionalItem =
                                        (ConditionalParticularAccessItem)duplicateItems.FirstOrDefault(i => i is ConditionalParticularAccessItem && !string.IsNullOrEmpty(((ConditionalParticularAccessItem)i).Condition));
                                }
                                if (conditionalItem != null)
                                    resultItems.Add(new ConditionalParticularAccessItem(conditionalItem.ObjectType, conditionalItem.Access, conditionalItem.Modifier, conditionalItem.Condition));
                                else
                                    resultItems.Add(new ParticularAccessItem(item.ObjectType, item.Access, item.Modifier));
                            }
                        }
                    ObjectAccessPermission result = (ObjectAccessPermission)Activator.CreateInstance(typeof(TActualResultType));
                    resultItems.ForEach(item => result.AccessItemList.Add(item));
                    return result;
                } catch (Exception ex) {
                    throw new Exception(ex.ToString());
                }
           }

           public virtual ConditionalObjectAccessPermission FilterUnfitItems(object contextObject) {
                try {
                    Type objectType = contextObject.GetType();
                    //IObjectSpace objectSpace = ObjectSpace.FindObjectSpace(contextObject);
                    IObjectSpace objectSpace = ObjectSpace.FindObjectSpaceByObject(contextObject);
                    EvaluatorContextDescriptor descriptor = objectSpace != null ? objectSpace.GetEvaluatorContextDescriptor(objectType) : new EvaluatorContextDescriptorDefault(objectType);
                    ConditionalObjectAccessPermission result = new ConditionalObjectAccessPermission();
                    foreach (ConditionalParticularAccessItem item in AccessItemList) {
                        bool itemFits = string.IsNullOrEmpty(item.Condition);
                        if (!itemFits && item.ObjectType == objectType) {
                            LocalizedCriteriaWrapper wrapper = new LocalizedCriteriaWrapper(objectType, item.Condition);
                            wrapper.UpdateParametersValues(contextObject);
                            ExpressionEvaluator evaluator = new ExpressionEvaluator(descriptor, wrapper.CriteriaOperator);
                            itemFits = evaluator.Fit(contextObject);
                        }
                        if (itemFits)
                            result.AccessItemList.Add(item);
                    }
                    return result;
                } catch (Exception ex) {
                    throw new Exception(ex.ToString());
                }
           }
       }
   
       public class ConditionalParticularAccessItem : ParticularAccessItem, IEquatable<ConditionalParticularAccessItem> {
           public ConditionalParticularAccessItem(Type objectType, ObjectAccess particularAccess, ObjectAccessModifier modifier)
               : this(objectType, particularAccess, modifier, string.Empty) { }
           public ConditionalParticularAccessItem(Type objectType, ObjectAccess particularAccess, ObjectAccessModifier modifier, string condition)
               : base(objectType, particularAccess, modifier) {
               Condition = condition ?? string.Empty;
           }

           public string Condition { get; private set; }

           public override bool Equals(object obj) {
               ConditionalParticularAccessItem item = obj as ConditionalParticularAccessItem;
               if (ReferenceEquals(item, null))
                   return false;
               return Equals(item);
           }

           public bool Equals(ConditionalParticularAccessItem item) {
               if (ReferenceEquals(item, null))
                   return false;
               return ObjectType == item.ObjectType && Access == item.Access && Modifier == item.Modifier && Condition == item.Condition;
           }

           public static bool operator ==(ConditionalParticularAccessItem i1, ConditionalParticularAccessItem i2) {
               if (ReferenceEquals(i1, null))
                   if (ReferenceEquals(i2, null))
                       return true;
                   else
                       return false;
               return i1.Equals(i2);
           }

           public static bool operator !=(ConditionalParticularAccessItem i1, ConditionalParticularAccessItem i2) {
               return !(i1 == i2);
           }

           public override int GetHashCode() {
               return ObjectType.GetHashCode() ^ Access.GetHashCode() ^ Modifier.GetHashCode() ^ Condition.GetHashCode();
           }
       }
   
       public class ConditionalObjectAccessComparer: ObjectAccessComparer {
           public ConditionalObjectAccessComparer() { }
           public ConditionalObjectAccessComparer(ObjectAccessCompareMode objectAccessCompareMode) : base(objectAccessCompareMode) { }
   
           public override bool IsSubsetOf(ObjectAccessPermission sourcePermission, ObjectAccessPermission targetPermission) {
                try {
                    ObjectAccessPermission mergedTargetPermission = MergeTargetWithConditionalPermission(targetPermission, sourcePermission.Contexts);
                    return base.IsSubsetOf(sourcePermission, mergedTargetPermission);
                } catch (Exception ex) {
                    throw new Exception(ex.ToString());
                }
           }

           static ObjectAccessPermission MergeTargetWithConditionalPermission(ObjectAccessPermission targetPermission, SecurityContextList contexts) {
               try {
                   if (contexts.TargetObjectContext != null && contexts.TargetObjectContext.TargetObject != null) {
                       object targetObject = contexts.TargetObjectContext.TargetObject;
                       ConditionalObjectAccessPermission validatedConditionalPermission = ConditionalPermission.FilterUnfitItems(targetObject);
                       return (ObjectAccessPermission)validatedConditionalPermission.Union<ObjectAccessPermission>(targetPermission);
                   }
                   return targetPermission;
               } catch (Exception ex) {
                   throw new Exception(ex.ToString());
               }
           }

           static ConditionalObjectAccessPermission ConditionalPermission {
               get {
                   try {
                       IUser user = (IUser)SecuritySystem.Instance.User;
                       if (user != null)
                           return user.GetUserPermission<ConditionalObjectAccessPermission>() ?? new ConditionalObjectAccessPermission();
                       return new ConditionalObjectAccessPermission();
                   } catch (Exception ex) {
                       throw new Exception(ex.ToString());
                   }
               }
           }




           // Фрагмент для MemberAccessPermissionItem
           public override bool IsMemberReadGranted(Type requestedType, string propertyName, SecurityContextList securityContexts) {
               try {
                   ITypeInfo typeInfo = XafTypesInfo.Instance.FindTypeInfo(requestedType);
                   IMemberInfo memberInfo = typeInfo.FindMember(propertyName);
                   foreach (IMemberInfo currentMemberInfo in memberInfo.GetPath()) {
                           if (!SecuritySystem.IsGranted(new MemberAccessPermission(currentMemberInfo.Owner.Type, currentMemberInfo.Name, MemberOperation.Read))) {
                               return false;
                           }
                   }
                   return base.IsMemberReadGranted(requestedType, propertyName, securityContexts);
               } catch (Exception ex) {
                    throw new Exception(ex.ToString());
                }
           }
           public override bool IsMemberModificationDenied(object targetObject, IMemberInfo memberInfo) {
               try {
                   foreach (IMemberInfo currentMemberInfo in memberInfo.GetPath()) {
                       if (!SecuritySystem.IsGranted(new MemberAccessPermission(currentMemberInfo.Owner.Type, currentMemberInfo.Name, MemberOperation.Write))) {
                           return true;
                       }
                   }
                   return base.IsMemberModificationDenied(targetObject, memberInfo);
               } catch (Exception ex) {
                   throw new Exception(ex.ToString());
               }
           }



       }
   
       public static class IUserHelper {
           static public TPermissionType GetUserPermission<TPermissionType>(this IUser user) where TPermissionType : class, IPermission {
               try {
                   PermissionSet permissions = new PermissionSet(PermissionState.None);
                   foreach (IPermission currentPermission in user.Permissions)
                       permissions.AddPermission(currentPermission);
                   TPermissionType result = permissions.GetPermission(typeof(TPermissionType)) as TPermissionType;
                   return result;
               } catch (Exception ex) {
                   throw new Exception(ex.ToString());
               }
           }
       }

}