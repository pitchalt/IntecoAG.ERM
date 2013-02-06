using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Data.OleDb;
using System.Threading;

using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.Persistent.Base;

using Npgsql;

namespace IntecoAG.ERM.Win {

    // SHU 2011-12-08 Собственный провайдер для PostgeSql (чтобы Like заменить на ILike)
    // ПРОВАЙДЕР РАБОТАЕТ, НО ПРОБЛЕМУ НЕ РЕШАЕТ, т.к. вместо Like механизмы поиска составляют другое выражение 
    // навроде такого:
    // ((Strpos(N0."Code", @p0) > 0) or (Strpos(N0."Name", @p0) > 0) or (Strpos(N0."Description", @p0) > 0)))' with parameters {СтрокаКотораяИщется}
    // Для решения проблемы перекрыт метод FormatSelect


    // Этот клас позволяет подменить Like на ILike конкретно для PostgreSqlConnectionProvider
    // ILike работает без учёта регистров
    public class CustomPostgreSqlConnectionProvider : DevExpress.Xpo.DB.PostgreSqlConnectionProvider {
        public CustomPostgreSqlConnectionProvider(IDbConnection connection, AutoCreateOption autoCreateOption)
            : base(connection, autoCreateOption) {
        }

        public override string FormatBinary(DevExpress.Data.Filtering.BinaryOperatorType operatorType, string leftOperand, string rightOperand) {

            if (BinaryOperatorType.Like == operatorType) {
                return String.Format(CultureInfo.InvariantCulture, "{0} ilike {1}", leftOperand, rightOperand);

            } else {
                return base.FormatBinary(operatorType, leftOperand, rightOperand);
            }
        }

        //public new static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect) {
        //    IDbConnection connection = new OleDbConnection(connectionString);
        //    objectsToDisposeOnDisconnect = new IDisposable[] { connection };
        //    return CreateProviderFromConnection(connection, autoCreateOption);
        //}

        //public new static IDataStore CreateProviderFromConnection(IDbConnection connection, AutoCreateOption autoCreateOption) {
        //    if (((System.Data.OleDb.OleDbConnection)connection).Provider.StartsWith("Microsoft.Jet.OLEDB")
        //        || ((System.Data.OleDb.OleDbConnection)connection).Provider.StartsWith("Microsoft.ACE.OLEDB"))

        //        return new CustomPostgreSqlConnectionProvider(connection, autoCreateOption);
        //    else
        //        return null;
        //}


        public new static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect) {
            IDbConnection connection = new NpgsqlConnection(connectionString);
            objectsToDisposeOnDisconnect = new IDisposable[] { connection };
            return CreateProviderFromConnection(connection, autoCreateOption);
        }

        public new static IDataStore CreateProviderFromConnection(IDbConnection connection, AutoCreateOption autoCreateOption) {
            return new CustomPostgreSqlConnectionProvider(connection, autoCreateOption);
        }


        public new static string GetConnectionString(string server, string userid, string password, string database) {
            return String.Format("{4}={5};Server={0};User Id={1};Password={2};Database={3};Encoding=UNICODE;",
                server, userid, password, database, DataStoreBase.XpoProviderTypeParameterName, XpoProviderTypeString);
        } 

        public new static string GetConnectionString(string server, int port, string userid, string password, string database) {
            return String.Format("{5}={6};Server={0};User Id={1};Password={2};Database={3};Encoding=UNICODE;Port={4}",
                server, userid, password, database, port, DataStoreBase.XpoProviderTypeParameterName, XpoProviderTypeString);
        }

        public new static void Register() {
            try {
                DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, new  DataStoreCreationFromStringDelegate(CreateProviderFromString));
            } catch (ArgumentException e) {
                Tracing.Tracer.LogError(e);
                //Tracing.Tracer.LogText("A connection provider with the same name ( {0} ) has already been registered", XpoProviderTypeString);
            }

        }

        public new const string XpoProviderTypeString = "CustomPostgreSqlConnectionProvider";

        public override string FormatSelect(string selectedPropertiesSql, string fromSql, string whereSql, string orderBySql, string groupBySql, string havingSql, int skipSelectedRecords, int topSelectedRecords) {

            // ((Strpos(N0."Code", @p0) > 0) or (Strpos(N0."Name", @p0) > 0) or (Strpos(N0."Description", @p0) > 0)))' with parameters {СтрокаКотораяИщется}
            // Находим в условии whereSql все выражения вида (Strpos(N0."Name", @p0) > 0) - конечно, тут надо регулярные выражения применять - но 
            // некогда это делать, поэтому по-простому

            string whereSqlNew = "";

            if (!string.IsNullOrEmpty(whereSql)) {

                if (whereSql.Contains("(Strpos(")) {

                    string[] delimiter = { "(Strpos(" };
                    string[] mWhereSql = ("WHERE"+whereSql).Split(delimiter, StringSplitOptions.None);

                    whereSqlNew = mWhereSql[0].Substring(5);
                    // Теперь в начале каждого элемента массива находится выражение "(Strpos(" (а может и не находится)
                    for (int i = 1; i < mWhereSql.Length; i++) {
                        string elem = mWhereSql[i];

                        //if (elem.StartsWith("(Strpos(")) {
                            // Находим первое вхождение подстроки ") >" 
                            int bigPos = elem.IndexOf(">");
                            if (bigPos > -1) {

                                // Заменяем ">" на ") >"
                                //elem = String.Format("{0}) {1}", elem.Substring(0, bigPos), elem.Substring(bigPos));
                                elem = elem.Substring(0, bigPos) + ") " + elem.Substring(bigPos);

                                // Находим первое вхождение запятой
                                int comaPos = elem.IndexOf(",");

                                // Заменяем "," на "), strtoupper("
                                elem = elem.Substring(0, comaPos) + "), upper(" + elem.Substring(comaPos + 1);

                                // Заменяем (Strpos(" на "(Strpos(Strtoupper("
                                elem = "(Strpos(upper(" + elem;  //.Substring("(Strpos(".Length);
                                //elem.Replace("(Strpos(", "(Strpos(Strtoupper(");
                            }
                        //}

                        whereSqlNew += elem;
                    }
                } else {
                    whereSqlNew = whereSql;
                }

            } else {
                whereSqlNew = whereSql;
            }

            return base.FormatSelect(selectedPropertiesSql, fromSql, whereSqlNew, orderBySql, groupBySql, havingSql, skipSelectedRecords, topSelectedRecords);
        }

    }
    
}
