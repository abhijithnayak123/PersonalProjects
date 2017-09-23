using System;
using System.Collections.Generic;
using System.Data;
// to use EntityConnection
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core;

namespace EFBasicTutorials
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("*** EntityFramework Basic Tutorials Demo Start ***");
            Console.WriteLine("");

            EFBasics.DynamicProxyDemo();
            EFBasics.FindEntityDemo();
            EFBasics.LazyLoadingDemo();
            EFBasics.ExplicitLoadingDemo();
            EFBasics.RawSQLtoEntityTypeDemo();
            EFBasics.ReadDataUsingStoredProcedure();
            EFBasics.RawSQLCommandDemo();
            EFBasics.EntitySQLDemo();
            EFBasics.EntitySQLUsingEntityConnectionDemo();
            EFBasics.SpatialDataTypeDemo();
            EFBasics.GetPropertyValuesDemo();
            EFBasics.LocalDemo();
            EFBasics.ChangeTrackingDemo();
            EFBasics.SetValuesDemo();
            EFBasics.ValidationErrorDemo();
            EFBasics.CRUDOperationInConnectedScenarioDemo();
            EFBasics.OptimisticConcurrencyDemo();
            EFBasics.DBEntityEntryDemo();
            //Add and map sp_InsertStudentInfo, sp_UpdateStudent 
            //and sp_DeleteStudent into EDM to run following demo
            //EFBasics.CUDOperationUsingStoredProcedureDemo();

            AddDemo.AddSingleEntity();
            AddDemo.AddEntityGraph();

            UpdateDemo.UpdateSingleEntity();
            UpdateDemo.UpdateEntityGraphUsingId();


            //Implement IEntityObjectState in Standard & Teacher entity class to run following demo
            //UpdateDemo.UpdateEntityGraphUsingStateProperty();

            Console.WriteLine("*** EntityFramework Basic Tutorials Demo Finished ***");
            Console.ReadKey();

        }

      

        
    }
}
