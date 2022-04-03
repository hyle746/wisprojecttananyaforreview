using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using WISProject1.DAL;
using WISProject1.Helper;
using WISProject1.Models;
using WISProject1.Repository;
using System.Data.Entity;
using System.Web.Security;

namespace WISProject1.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        public List<SelectListItem> GetCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cat = _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecords();
            foreach (var item in cat)
            {
                list.Add(new SelectListItem { Value = item.CategoryId.ToString(), Text = item.CategoryName });
            }
            return list;
        }
   
        public ActionResult CategoryEdit(int catId)
        {
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Category>().GetFirstorDefault(catId));
        }
        [HttpPost]
        public ActionResult CategoryEdit(Tbl_Category tbl)
        {
            _unitOfWork.GetRepositoryInstance<Tbl_Category>().Update(tbl);
            return RedirectToAction("Categories2");
        }
        [AuthorizationFilter]
        public ActionResult Product()
        {
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Product>().GetProduct());
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult AdminLogin()
        {
            return View();
        }

        public JsonResult CheckLogin(string username, string password)
        {

            WISProjectEntities db = new WISProjectEntities();
            string md5StringPassword = AppHelper.GetMd5Hash(password);
            var dataItem = db.Tbl_User.Where(x => x.UserName == username && x.Password == md5StringPassword).SingleOrDefault();
            bool isLogged = true;
            if (dataItem != null)
            {
                Session["Username"] = dataItem.UserName;
                Session["Role"] = dataItem.Role;
                FormsAuthentication.SetAuthCookie(dataItem.UserName, false);
                isLogged = true;
            }
            else
            {
                isLogged = false;
            }
            return Json(isLogged, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdminRegister()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SaveUser(Tbl_User user)
        {

            WISProjectEntities db = new WISProjectEntities();
            //เช็คว่ามี user นี้อยู่แล้วหรือไม่
            bool isSuccess = true;
            if (db.Tbl_User.Any(x => x.UserName == user.UserName))
            {
                ViewBag.DuplicateMessage = "Username already exist.";

            }
            else
          if (user.AdminId > 0)
            {
                db.Entry(user).State = EntityState.Modified;
            }
            else
            {
                user.Status = 1;
                user.Password = AppHelper.GetMd5Hash(user.Password);
                db.Tbl_User.Add(user);
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                isSuccess = false;
            }

            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }
      
    }

}

