using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WISProject1.DAL;
using WISProject1.Repository;
using System.Data.SqlClient;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace WISProject1.Models.Home
{
    public class HomeIndexViewModel
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        WISProjectEntities context = new WISProjectEntities();
        public IPagedList<Tbl_Product> ListOfProducts { get; set; }
        public HomeIndexViewModel CreateModel(string search, int pageSize, int? page)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@search",search??(object)DBNull.Value)
            };
            IPagedList<Tbl_Product> data = context.Database.SqlQuery<Tbl_Product>("GetBySearch @search", param).ToList().ToPagedList(page ?? 1, pageSize);
            return new HomeIndexViewModel
            {
                ListOfProducts = data
            };
        }
    }
}