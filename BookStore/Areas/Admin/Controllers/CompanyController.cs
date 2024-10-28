using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAcess.Data;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]

    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            
            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {
            
            if (id == null || id == 0)
            {
                return View(new Company());
            }
            else
            {
                Company companyObj = _unitOfWork.Company.Get(u=>u.Id==id);
                return View(companyObj);
            }
            
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {
           
            if (ModelState.IsValid)
            {

                if (company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                
                return View(company);
            }
            

        }

        

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Company obj = _unitOfWork.Company.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return RedirectToAction("Index");

        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(int id)
        {
            List<Company> objProductList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objProductList });

        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error :(" });

            }

            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Success :)" });

        }

        #endregion

    }
}
