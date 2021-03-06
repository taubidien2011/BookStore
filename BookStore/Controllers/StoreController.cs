﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

using PagedList;
using PagedList.Mvc;


namespace BookStore.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store

        dbQLBansachDataContext data = new dbQLBansachDataContext();

        private List<SACH> laySachMoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        
        public ActionResult Index(int ? page)
        {
            int pageSize = 4;

            int pageNum = (page ?? 1);

            var sachMoi = laySachMoi(16);
            return View(sachMoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult ChuDe()
        {
            var chuDe = from cd in data.CHUDEs select cd;
            return PartialView(chuDe);
        }
        public ActionResult NhaXuatBan()
        {
            var nxb = from x in data.NHAXUATBANs select x;
            return PartialView(nxb);
        }
        
        public ActionResult SPTheochude(int id)
        {
            var Sach = from s in data.SACHes where s.MaCD == id select s;
           return View(Sach);
        }
        public ActionResult SPTheoNXB(int id)
        {
            var Sach = from s in data.SACHes where s.MaNXB == id select s;
            return View(Sach);
        }
        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes where s.Masach == id select s;
            return View(sach.Single());
        }


    }
}