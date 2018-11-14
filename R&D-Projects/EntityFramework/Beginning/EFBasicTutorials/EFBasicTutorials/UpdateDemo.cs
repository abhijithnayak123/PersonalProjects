using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFBasicTutorials
{
    public class UpdateDemo
    {
        public static void UpdateSingleEntity()
        {
            Console.WriteLine("****Starting UpdateSingleEntity****");

            Student disconnectedStudent = null;

            using (var context = new SchoolDatabaseEntities())
            {
                disconnectedStudent = context.Students.Where(s => s.StudentID == 1).FirstOrDefault<Student>();
            }

            disconnectedStudent.StudentName = "Edited Student Name";

            using (var context = new SchoolDatabaseEntities())
            {
                context.Entry(disconnectedStudent).State = EntityState.Modified;
                context.SaveChanges();

                Console.WriteLine("Student Updated Successfully.");
            }
            Console.WriteLine("****Finished UpdateSingleEntity****");
        }


        public static void UpdateEntityGraphUsingId()
        {
            Console.WriteLine("****Starting UpdateEntityGraphUsingId****");

            Standard disconnectedStandard = null;

            using (var context = new SchoolDatabaseEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;

                disconnectedStandard = context.Standards.Where(s => s.StandardId == 2).Include(s => s.Teachers).FirstOrDefault<Standard>();
            }
            //Update Standard in disconnected mode
            disconnectedStandard.StandardName = "Edited Standard Name";
            
            //Update teachers collection by editing first teacher and adding new teacher
            disconnectedStandard.Teachers.ElementAt(0).TeacherName = "Edited Teacher Name";
            disconnectedStandard.Teachers.Add(new Teacher() { TeacherName = "New Teacher", StandardId = disconnectedStandard.StandardId });

            using (var newContext = new SchoolDatabaseEntities())
            {
                //mark standard based on StandardId
                newContext.Entry(disconnectedStandard).State = disconnectedStandard.StandardId == 0 ? EntityState.Added : EntityState.Modified;

                //mark teacher based on StandardId
                foreach (Teacher tchr in disconnectedStandard.Teachers)
                    newContext.Entry(tchr).State = tchr.TeacherId == 0 ? EntityState.Added : EntityState.Modified;
                
                //check entity state
                foreach (var std in newContext.Standards.Local)
                {
                    Console.WriteLine(
                        "Found StudentId: {0} with state {1}",
                        std.StandardId,
                        newContext.Entry(std).State);
                }

                //check entity state
                foreach (var tchr in newContext.Teachers.Local)
                {
                    Console.WriteLine(
                        "Found CourseId: {0} with state {1}",
                        tchr.TeacherId,
                        newContext.Entry(tchr).State);
                }

                newContext.SaveChanges();
            }

            Console.WriteLine("**** Finshed UpdateEntityGraphUsingId****");
        }

        //make sure Standard & Teacher entity implement IObjectState before uncommenting below code
        public static void UpdateEntityGraphUsingStateProperty()
        {
            //Teacher existingTeacher = null;

            //using (var context = new SchoolDatabaseEntities())
            //{
            //    context.Configuration.ProxyCreationEnabled = false;
            //    existingTeacher = context.Teachers.FirstOrDefault<Teacher>();

            //}
            //Standard disconnectedStandard = new Standard() { StandardName = "New Standard", ObjectState = EntityObjectState.Added };
            //existingTeacher.ObjectState = EntityObjectState.Modified;
            ////add existing teacher(in db) to standard
            //disconnectedStandard.Teachers.Add(existingTeacher);
            ////add new standard
            //disconnectedStandard.Teachers.Add(new Teacher() { TeacherName = "New teacher", StandardId = disconnectedStandard.StandardId, ObjectState = EntityObjectState.Added });

            //using (var newContext = new SchoolDatabaseEntities())
            //{
            //    //check the ObjectState property and mark appropriate EntityState 
            //    if (disconnectedStandard.ObjectState == EntityObjectState.Added)
            //        newContext.Entry(disconnectedStandard).State = System.Data.Entity.EntityState.Added;
            //    else if (disconnectedStandard.ObjectState == EntityObjectState.Modified)
            //        newContext.Entry(disconnectedStandard).State = System.Data.Entity.EntityState.Modified;
            //    else if (disconnectedStandard.ObjectState == EntityObjectState.Deleted)
            //        newContext.Entry(disconnectedStandard).State = System.Data.Entity.EntityState.Deleted;
            //    else
            //        newContext.Entry(disconnectedStandard).State = System.Data.Entity.EntityState.Unchanged;

            //    //check the ObjectState property of each teacher and mark appropriate EntityState 
            //    foreach (Teacher tchr in disconnectedStandard.Teachers)
            //    {
            //        if (tchr.ObjectState == EntityObjectState.Added)
            //            newContext.Entry(tchr).State = System.Data.Entity.EntityState.Added;
            //        else if (tchr.ObjectState == EntityObjectState.Modified)
            //            newContext.Entry(tchr).State = System.Data.Entity.EntityState.Modified;
            //        else if (tchr.ObjectState == EntityObjectState.Deleted)
            //            newContext.Entry(tchr).State = System.Data.Entity.EntityState.Deleted;
            //        else
            //            newContext.Entry(tchr).State = System.Data.Entity.EntityState.Unchanged;
            //    }
            //    //save changes
            //    newContext.SaveChanges();

            //}
        }
    }
    
}
