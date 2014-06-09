using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.ERM {
    public static class CommonConstants {

        /// <summary>
        /// Разделитель элементов в списках пользователей, ролей и групп
        /// </summary>
        public const string UserListSeparator = ";";

        /// <summary>
        /// Разделитель заголовка (типа) от идентификатора
        /// </summary>
        public const string ElemSeparator = ":";

        public static DateTime DateUnlimitedValue = new DateTime(2999, 12, 31);
        public static DateTime DateMaxValue = new DateTime(2199, 12, 31);
        public static DateTime DateMinValue = new DateTime(1950, 1, 1);

    }
}
