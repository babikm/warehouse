using DAL;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class CharacteristicController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CharacteristicController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public ActionResult Index()
        {
            var all = _unitOfWork.CharacteristicRepository.GetAll();
            return View(all);
        }

        public ActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Characteristic characteristic)
        {
            if (!ModelState.IsValid)
                return View(characteristic);
            _unitOfWork.CharacteristicRepository.Add(characteristic);
            return RedirectToAction("Index", "Characteristic");
        }

        public IActionResult Delete(string id)
        {
            _unitOfWork.CharacteristicRepository.Delete(id);
            return RedirectToAction("Index", "Characteristic");
        }

        public IActionResult Edit(string id) => View(_unitOfWork.CharacteristicRepository.Get(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Characteristic characteristic)
        {
            try
            {
                _unitOfWork.CharacteristicRepository.Get(characteristic.Id);
                _unitOfWork.ProductRepository.GetAll();
                _unitOfWork.CharacteristicRepository.Update(x => x.Id == characteristic.Id, characteristic);
                return RedirectToAction("Index", "Characteristic");
            }
            catch
            {
                return View();
            }
        }
    }
}
