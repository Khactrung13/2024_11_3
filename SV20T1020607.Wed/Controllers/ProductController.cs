using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using SV20T1020067.BusinessLayers;
using SV20T1020607.DomainModels;
using SV20T1020607.Wed.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SV20T1020607.Wed.Controllers
{
    public class ProductController : Controller
    {
        const int PAGE_SIZE = 20;
        const string Create_title = "Bổ sung mặt hàng";
        const string Update_title = "Cập nhật thông tin mặt hàng";
        const string PRODUCT_SEARCH = "product_search";//Tên biến session dùng để lưu lại điều kiện tìm kiếm
        // GET: /<controller>/
        public IActionResult Index()
        {
            //Kiểm tra xem trong session có lưu điều kiện tìm kiếm không
            //Nếu có thì sử dụng điều kiện tìm kiếm , ngược lại thì tìm kiếm theo điều kiện mặt định
            Models.ProductSearchInput? input = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH);
            if (input == null)
            {
                input = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0,
                    minPrice = 0,
                    maxPrice = 0
                };
            }
            return View(input);
        }
        public IActionResult Search(ProductSearchInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListOfProducts(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "", input.CategoryID
            , input.SupplierID, input.minPrice, input.maxPrice);
            var model = new ProductSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                CategoryID = input.CategoryID,
                SupplierID = input.SupplierID,
                minPrice = input.minPrice,
                maxPrice = input.maxPrice,
                RountCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiện tìm kiếm
            ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.IsEdit = false;
            ViewBag.Title = Create_title;
            var model = new Product()
            {
                ProductID = 0,
                Photo = "nophoto.png",
                IsSelling= true
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Title = Update_title;
            ViewBag.IsEdit = true;
            var model = ProductDataService.GetProduct(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            if (string.IsNullOrWhiteSpace(model.Photo))
                model.Photo = "nophoto.png";
            return View(model);
        }
        public IActionResult Delete(int id=0)
        {
            ViewBag.Title = "Xóa mặt hàng";
            var model = ProductDataService.GetProduct(id);
            if (Request.Method == "POST")
            {
                bool resutl = ProductDataService.DeleteProduct(id);
                if (!resutl)
                {
                    ModelState.AddModelError("Error", "Xóa mặt hàng không thành công");
                    ViewBag.Title = "Xóa mặt hàng";
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            

            if (model == null)
            {
                return RedirectToAction("Index");
            }
           

            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Product model)
        {
           
            
            if (model.ProductID == 0)
            {
                int id = ProductDataService.AddProduct(model);
                if (id <= 0)
                {
                    ModelState.AddModelError("Error", "Tên mặt hàng đã tồn tại!!!");
                    ViewBag.Title = Create_title;
                    return View("Edit", model);
                }
            }
            else
            {
                //ViewBag.IsEdit = true;
                bool result = ProductDataService.UpdateProduct(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được mặt hàng (Tên mặt hàng có thể đã tồn tại)");
                    ViewBag.Title = Update_title;
                    return View("Edit", model);
                }

            }
            return RedirectToAction("Index");
        }



        public IActionResult Photo(string id,string method, int photoID=0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    ViewBag.IsEdit = true;
                    return View();
                case "edit":
                    ViewBag.Title = "Cập nhật ảnh cho mặt hàng";
                    ViewBag.IsEdit = false;
                    return View();
                case "delete":
                    //TODO: Xóa ảnh có mã là photoId(Xóa trực tiếp,không cần xác nhận
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        public IActionResult Attribute(string id, string method, int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    
                    return View();
                case "edit":
                    ViewBag.Title = "Cập nhật thuộc tính cho mặt hàng";
                    
                    return View();
                case "delete":
                    //TODO: Xóa ảnh có mã là photoId(Xóa trực tiếp,không cần xác nhận
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }


    }

}

