using Crud_operation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crud_operation.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentContext _Db;
        public StudentController(StudentContext Db)
        {
           this._Db = Db;
        }
        

        public IActionResult StudentList()
        {
            try
            {
                //var stdList = _Db.Students.ToList();
                var stdList = from a in _Db.Students
                              join b in _Db.Departments
                              on a.DeptID equals b.ID
                              into Dep
                              from b in Dep.DefaultIfEmpty()

                              select new Students
                              {
                                  ID = a.ID,
                                  Name = a.Name,
                                  Fname = a.Fname,
                                  Mobile = a.Mobile,
                                  Email = a.Email,
                                  Description = a.Description,
                                  DeptID = a.DeptID,
                                  Department = b == null? "" : b.Department  
                              };

                return View(stdList);
            }
            catch (Exception)
            {
                return View();

            }
        }


        public IActionResult Create(Students obj)
        {
            LoadDDL();
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> AddStd(Students obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(obj.ID == 0)
                    {
                        Random rd = new Random();
                        obj.ID = rd.Next(1010, 10000);
                        _Db.Students.Add(obj);
                        await _Db.SaveChangesAsync();

                    }
                    else
                    {
                        _Db.Entry(obj).State = EntityState.Modified;
                        await _Db.SaveChangesAsync();
                    }

                    return RedirectToAction("StudentList");

                }
                return View(obj);

            }

            catch(Exception)
            {
                return RedirectToAction("StudentList");
                
            }
        }

        public async Task<IActionResult> DeleteStd(int id)
        {
            try
            {
                var std =await _Db.Students.FindAsync(id);
                if (std != null)
                {
                    _Db.Students.Remove(std);
                    await _Db.SaveChangesAsync();
                }
                return RedirectToAction("StudentList");

            }
            catch(Exception)
            {
                return RedirectToAction("StudentList");

            }
        }

        private void LoadDDL()
        {
            try
            {
                List<Departments> depList = new List<Departments>();
                depList = _Db.Departments.ToList();

                depList.Insert(0, new Departments { ID = 0, Department = "Please Select" });
                ViewBag.DeptList = depList;

            }

            catch (Exception)
            {
                

            }
        }


        
    }
}
