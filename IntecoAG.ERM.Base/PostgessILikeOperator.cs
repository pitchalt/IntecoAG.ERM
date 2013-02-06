using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;

using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB;

namespace IntecoAG.ERM.Module {

    // SHU 2011-12-08 Самодельный оператор критерия ILike
    // Работает этот ILike с ошибкой, устранить которую пока не удалось,
    // 

    class PostgessILikeOperator : ICustomFunctionOperatorFormattable {

        // Первый агрумент - искомая строка, а во втором аргументе находится строка, в которой ищется первая.
        #region ICustomFunctionOperatorFormattable Members
        // The function's expression to be evaluated on the server. 
        //string ICustomFunctionOperatorFormattable.Format(Type providerType, params string[] operands) {
        //    // This example implements the function for PostgreSQL databases only. 
        //    if (providerType == typeof(DevExpress.Xpo.DB.PostgreSqlConnectionProvider))
        //        return string.Format(CultureInfo.InvariantCulture, "{0} ILike {1}", operands[0], operands[1]);
        //    throw new NotSupportedException(string.Concat("This provider is not supported: ", providerType.Name));
        //}

        // По невыясненным причинам система выдаёт ошибку на ILike такого вида:
        //An error occurs while applying the 'DevExpress.ExpressApp.Utils.LightDictionary`2[System.String,DevExpress.Data.Filtering.CriteriaOperator] 
        //@41fa22e5-8302-4fc5-9b9d-52e4415e60fb' criteria: 'DefaultFormatFunction for custom(IList)' 
        // Появилась идея заменить ILike на Strpos в сочетании с 
        string ICustomFunctionOperatorFormattable.Format(Type providerType, params string[] operands) {
            // This example implements the function for PostgreSQL databases only. 
            if (providerType == typeof(DevExpress.Xpo.DB.PostgreSqlConnectionProvider))
                return string.Format(CultureInfo.InvariantCulture, "{0} ILike {1}", operands[0], operands[1]);
            throw new NotSupportedException(string.Concat("This provider is not supported: ", providerType.Name));
        }


        public static bool ILike(string Str1, string Str2) {
            return Str2.IndexOf(Str1) > -1;
        }
        #endregion

        #region ICustomFunctionOperator Members
        // Evaluates the function on the client. 
        object ICustomFunctionOperator.Evaluate(params object[] operands) {
            return ILike(operands[0].ToString(), operands[1].ToString());
        }

        string ICustomFunctionOperator.Name {
            get { return "ILike"; }
        }

        Type ICustomFunctionOperator.ResultType(params Type[] operands) {
            return typeof(bool);
        }
        #endregion
    }
}