using System.Collections.Generic;
using System.Linq;

namespace IntecoAG.ERM.FM.StatementAccount {

    static class ArrayExtensions {

        /// <summary>
        /// Определение вхождения массива x в массив y. Расширение методов byte[].
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IEnumerable<int> StartIndex(this byte[] x, byte[] y) {
            IEnumerable<int> index = Enumerable.Range(0, x.Length - y.Length + 1);
            for (int i = 0; i < y.Length; i++) {
                index = index.Where(n => x[n + i] == y[i]).ToArray();
            }
            return index;
        }

    } 


}
