using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace BookStore.Controllers
{
    public class AdminController : Controller
    {
        dbQLBansachDataContext data = new dbQLBansachDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Themsachmoi()
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "Tenchude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        public ActionResult Sach(int? page)
        {
            int PageNumber = (page ?? 1);
            int PageSize = 7;
            return View(data.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(PageNumber, PageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        


        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var user = collection["username"];
            var pass = collection["password"];
            if (String.IsNullOrEmpty(user))
                ViewData["Loi1"] = "Phải nhập tên đăng nhp";
            if (String.IsNullOrEmpty(pass))
                ViewData["Loi2"] = "Ph?i nh?p m?t kh?u";
            if (!String.IsNullOrEmpty(user) && !String.IsNullOrEmpty(pass))
            {
                Admin ad = data.Admins.SingleOrDefault(n => n.UserAdmin == user && n.PassAdim == pass);
                if (ad != null)
                {
                    ViewBag.ThongBao = "Ðang nh?p thành công";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.ThongBao = "Tên dang nh?p ho?c m?t kh?u không dúng";
                }
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoisach(SACH sACH, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "Tenchude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            //luu ten file
            //kiem tra anh ton tai
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(fileUpload.FileName);
                    //luu duong dan
                    var path = Path.Combine(Server.MapPath("~/img"), filename);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "hình ảnh này tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    sACH.Hinhminhhoa = filename;
                    data.SACHes.InsertOnSubmit(sACH);
                    data.SubmitChanges();
                }
                return RedirectToAction("Sach");

            }
        }
        public ActionResult Details(int id)
        {
            SACH sACH = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sACH.Masach;
            if(sACH==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sACH);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            SACH sACH = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sACH.Masach;
            if (sACH == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sACH);
        }
        [HttpPost,ActionName("Xoasach")]
        public ActionResult Xacnhanxoa(int id)
        {
            SACH sACH = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sACH.Masach;
            if (sACH == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.SACHes.DeleteOnSubmit(sACH);
            data.SubmitChanges();
            return RedirectToAction("Sach");
        }
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            ViewBag.Mota = sach.Mota;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude", sach.MaCD);
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude", sach.MaCD);
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);

            SACH s = data.SACHes.ToList().Find(n => n.Masach == sach.Masach);
            if (ModelState.IsValid)
            {
                if (fileUpload != null)
                {
                    var filename = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/img"), filename);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình đã tồn tại";
                        return View();
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                        s.Hinhminhhoa = filename;
                    }
                }
                s.Tensach = sach.Tensach;
                s.Dongia = sach.Dongia;
                s.Mota = sach.Mota;
                s.Hinhminhhoa = sach.Hinhminhhoa;
                s.MaCD = sach.MaCD;
                s.MaNXB = sach.MaNXB;
                s.Ngaycapnhat = sach.Ngaycapnhat;
                s.Soluongban = sach.Soluongban;
                data.SubmitChanges();
            }
            return RedirectToAction("Sach");
        }





    }
}