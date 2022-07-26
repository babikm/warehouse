using DAL;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public IActionResult Index() => View(_unitOfWork.CompanyRepository.GetAll());

        public IActionResult Create() => View();

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Company company)
        {
            if (!ModelState.IsValid)
                return View(company);
            _unitOfWork.CompanyRepository.Add(company);
            return View();
        }

        public IActionResult Delete(string id)
        {
            _unitOfWork.CompanyRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(string id) => View(_unitOfWork.CompanyRepository.Get(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Company company)
        {
            try
            {
                _unitOfWork.CompanyRepository.Update(x => x.Id == company.Id, company);
                return RedirectToAction("Index", "Company");
            }
            catch
            {
                return View();
            }
        }
    }
}
