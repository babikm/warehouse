using DAL;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            MultipleViewModel multipleViewModel = new MultipleViewModel();
            IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAll();
            IEnumerable<Characteristic> characteristics = _unitOfWork.CharacteristicRepository.GetAll();
            multipleViewModel.Characteristics = characteristics;
            multipleViewModel.Products = products;
            return View(multipleViewModel);
        }
        

        public IActionResult GroupedProducts() => View(_unitOfWork.ProductRepository.GetAll().GroupBy(x => new
        {
            x.Name,
            x.Type
        }).Select(r => new ProductViewModel()
        {
            Products = r.ToList(),
            Weight = r.Sum(y => y.Weight),
            Name = r.Key.Name,
            Type = r.Key.Type
        }));

        [HttpGet]
        public ActionResult Create() => View();

        [HttpGet("Product/Create/{type}/{name}")]
        public ActionResult Create(string type, string name)
        {
            if (type == null && name == null)
                return View();
            Product product = new Product();
            product.Type = type;
            product.Name = name;
            product.Date = DateTime.Now;


            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {

            Product byBarcode = _unitOfWork.ProductRepository.GetByBarcode(product.Barcode);
            IEnumerable<SentProduct> sentProductsList = _unitOfWork.SentRepository.GetAll();
            IEnumerable<Product> productInQueue = _unitOfWork.QueueRepository.GetAll();
            bool flag = false;
            foreach (var item in productInQueue)
            {
                if (item.Barcode == product.Barcode)
                    flag = true;
            }
            foreach (SentProduct sentProduct in sentProductsList)
            {
                foreach (Product product1 in sentProduct.List)
                {
                    if (product1.Barcode == product.Barcode)
                        flag = true;
                }
            }
            if (ModelState.IsValid && byBarcode == null && !flag)
            {
                _unitOfWork.ProductRepository.Add(product);
                return View();
            }
            ModelState.AddModelError("Exist", "Taki kod kreskowy już istnieje!");
            return View(product);
        }

        public ActionResult Queue()
        {
            IEnumerable<Product> productList = _unitOfWork.QueueRepository.GetAll();
            IEnumerable<ProductViewModel> productViewModels = productList.GroupBy(x => x.Name).Select(grp => new ProductViewModel()
            {
                Weight = grp.Sum(b => b.Weight),
                Name = grp.Key,
                Products = grp.ToList()
            });

            List<string> stringList = new List<string>();
            List<int> intList = new List<int>();
            foreach (ProductViewModel productViewModel in productViewModels)
            {
                intList.Add(productViewModel.Weight);
                stringList.Add(productViewModel.Name);
            }
            ViewData["grouped"] = productViewModels;
            //ViewData["name"] = stringList;
            //ViewData["weight"] = intList;
            return View(productList);
        }

        public IActionResult DeleteFromQueue(Product product)
        {
            Product entity = _unitOfWork.QueueRepository.Get(product.Id);
            _unitOfWork.QueueRepository.Delete(product.Id);
            _unitOfWork.ProductRepository.Add(entity);
            return RedirectToAction("Queue", "Product");
        }

        public IActionResult AddQueue(string searchString)
        {
            Product byBarcode = _unitOfWork.ProductRepository.GetByBarcode(searchString);
            if (!string.IsNullOrEmpty(searchString) && byBarcode != null)
            {
                _unitOfWork.QueueRepository.Add(byBarcode);
                _unitOfWork.ProductRepository.Delete(byBarcode.Id);
            }
            return RedirectToAction("Queue", "Product");
        }

        public string ConvertToLineBreaks(string input) => Regex.Replace(input, "\r?\n", " ");

        public IActionResult AddQueueMore(string search)
        {
            string str1 = ConvertToLineBreaks(search) + " ";
            List<string> stringList = new List<string>();
            string str2 = "";
            foreach (char ch in str1)
            {
                if (ch != ' ')
                {
                    str2 += ch.ToString();
                }
                else
                {
                    stringList.Add(str2);
                    str2 = "";
                }
            }
            if (!string.IsNullOrEmpty(search))
            {
                foreach (string barcode in stringList)
                {
                    Product byBarcode = _unitOfWork.ProductRepository.GetByBarcode(barcode);
                    if (byBarcode != null)
                    {
                        _unitOfWork.QueueRepository.Add(byBarcode);
                        _unitOfWork.ProductRepository.Delete(byBarcode.Id);
                    }
                }
            }
            return RedirectToAction("Queue", "Product");
        }

        [HttpPost]
        public JsonResult Company(string Prefix) 
            => Json(_unitOfWork.CompanyRepository.GetAll()
                .Where(N => N.Name.ToLower().Contains(Prefix.ToLower()))
                .Select(N => new
                {
                        N.Name
         }));

        [HttpPost]
        public IActionResult AddSent()
        {
            IEnumerable<Product> productsInQueue = _unitOfWork.QueueRepository.GetAll();
            int num1 = _unitOfWork.SentRepository.GetAll().Count();
            IEnumerable<Product> list = productsInQueue.ToList();
            int num2 = list.Sum(x => x.Weight);
            SentProduct entity = new SentProduct
            {
                WZ = num1 + 1,
                List = list,
                Sum = num2,
                Invoice = "BRAK",
                Company = _unitOfWork.CompanyRepository.GetByName(Request.Form["Company"]),
                LicensePate = Request.Form["licensePlate"],
                Date = DateTime.Parse(Request.Form["Date"]).ToLocalTime()
            };

            _unitOfWork.QueueRepository.DeleteAll();
            if (list.Count() <= 0)
                return RedirectToAction("Queue", "Product");

            _unitOfWork.SentRepository.Add(entity);
            return RedirectToAction("Queue", "Product");
        }

        public IActionResult Delete(string id)
        {
            _unitOfWork.ProductRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(string id)
        {
            var product =_unitOfWork.ProductRepository.Get(id);
            return View(product);
        }
        public IActionResult GroupDetail(string name)
        {
            var product =_unitOfWork.ProductRepository.GetAll().Where(x => x.Name == name);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            try
            {
                _unitOfWork.ProductRepository.Update(x => x.Id == product.Id, product);
                return RedirectToAction("Index", "Product");
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Info() => View(_unitOfWork.ProductRepository.GetAll().Sum(x => x.Weight));
    }
}
