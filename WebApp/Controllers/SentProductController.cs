using DAL;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Collections.Generic;
using System.Linq;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class SentProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SentProductController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public IActionResult Index() => View(_unitOfWork.SentRepository.GetAll());

        public IActionResult Edit(string id)
        {
            var product = _unitOfWork.SentRepository.Get(id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SentProduct product)
        {
            try
            {
                SentProduct sentProduct = _unitOfWork.SentRepository.Get(product.Id);
                product.List = sentProduct.List;
                product.Company = _unitOfWork.CompanyRepository.GetByName(product.Company.Name);
                product.LicensePate = sentProduct.LicensePate;
                product.Sum = sentProduct.Sum;
                product.WZ = sentProduct.WZ;
                product.Date = product.Date.AddDays(1);
                _unitOfWork.SentRepository.Update(x => x.Id == product.Id, product);
                return RedirectToAction("Index", "SentProduct");
            }
            catch
            {
                return View();
            }
        }

        public IActionResult EditProduct(string id, string itemId) => View(_unitOfWork.SentRepository.Get(id).List.Where(x => x.Id == itemId).FirstOrDefault());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(Product product, string itemId)
        {
            try
            {
                SentProduct item = _unitOfWork.SentRepository.Get(product.Id);
                int weight = product.Weight;
                item.Sum = 0;
                itemId = Request.Query[nameof(itemId)].ToString();
                product = item.List.Where(x => x.Id == itemId).FirstOrDefault();
                product.Weight = weight;
                foreach (Product product1 in item.List)
                    item.Sum += product1.Weight;
                _unitOfWork.SentRepository.Update(x => x.Id == item.Id, item);
                return RedirectToAction("Index", "SentProduct");
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Back(string id)
        {
            foreach (Product entity in _unitOfWork.SentRepository.Get(id).List)
                _unitOfWork.ProductRepository.Add(entity);
            _unitOfWork.SentRepository.Delete(id);
            return RedirectToAction("Index", "SentProduct");
        }

        public IActionResult BackToQueue(string id)
        {
            SentProduct sentProduct = _unitOfWork.SentRepository.Get(id);
            if (_unitOfWork.QueueRepository.GetAll().Count() != 0)
                return RedirectToAction("Index", "SentProduct");
            foreach (Product entity in sentProduct.List)
                _unitOfWork.QueueRepository.Add(entity);
            _unitOfWork.SentRepository.Delete(id);
            return RedirectToAction("Index", "SentProduct");
        }

        public IActionResult Detail(string id) => View(_unitOfWork.SentRepository.Get(id));

        public IActionResult Delete(string id)
        {
            _unitOfWork.SentRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public IActionResult WZPrint(string id)
        {
            SentProduct sentProduct = _unitOfWork.SentRepository.Get(id);
            int num = _unitOfWork.SentRepository.GetAll().Count();
            IEnumerable<ProductViewModel> productViewModels = sentProduct.List.GroupBy(x => new
            {
                Name = x.Name,
                Type = x.Type
            }).Select(r => new ProductViewModel()
            {
                Products = r.ToList(),
                Weight = r.Sum(y => y.Weight),
                Name = r.Key.Name,
                Type = r.Key.Type
            });
            List<string> stringList = new List<string>();
            List<int> intList = new List<int>();
            foreach (ProductViewModel productViewModel in productViewModels)
            {
                intList.Add(productViewModel.Weight);
                stringList.Add(productViewModel.Name);
            }
            ViewData["grouped"] = productViewModels;
            ViewData["name"] = stringList;
            ViewData["weight"] = intList;
            ViewData["count"] = num;
            return View(sentProduct);
        }
    }
}
