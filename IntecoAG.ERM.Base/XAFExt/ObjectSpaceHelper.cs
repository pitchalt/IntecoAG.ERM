using System;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
//

namespace IntecoAG.ERM.Module {

    /// <summary>
    /// ������ ���������� ObjectSpace
    /// </summary>
    static public class ObjectSpaceHelper {

        /// <summary>
        /// �������������� ��������� Linq ��� XPO � ��������� ���������� �������� XPO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="os"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        static public CriteriaOperator LinqToXPOCriteria<T>(IObjectSpace os, System.Linq.Expressions.Expression<Func <T, bool>> expression) {
            // ����������� �� ��� T ���� �� ����������
            CriteriaOperator criteria = XPQuery<T>.TransformExpression(((ObjectSpace)os).Session, expression);
            return criteria;
        }
    }

}