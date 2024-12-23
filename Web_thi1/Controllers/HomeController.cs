using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using Web_thi1.Models;

namespace Web_thi1.Controllers
{
    public class HomeController : Controller
    {
        QLBanChauCanhEntities db = new QLBanChauCanhEntities();
        [HttpPost]
        public ActionResult XuHuong(string imageName)
        {
            Session["ImageName"] = imageName;

            return Json(new { imageName = imageName });
        }
        public ActionResult Index()
        {
            string tenLoaiSanPham = Session["ImageName"] as string;
            var phanLoaiSanPhams = db.PhanLoaiPhus.ToList();
            var sanPham = db.SanPhams.ToList();
            if (!string.IsNullOrEmpty(tenLoaiSanPham))
            {
                var checkLoc = db.SanPhams.Where(p => p.PhanLoaiPhu.TenPhanLoaiPhu == tenLoaiSanPham)

                  .ToList();
                ViewBag.checkLoc = checkLoc;

            }


            ViewBag.sanPham = sanPham;

            return View(phanLoaiSanPhams);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult detail(string id)
        {
            var sp = db.SanPhams.SingleOrDefault(n => n.MaSanPham == id);

            if (sp == null)
            {
                return RedirectToAction("Index", new { error = "SanPhamNotFound" });
            }
            ViewBag.MaSanPham = sp.MaSanPham;
            ViewBag.MPC = new SelectList(db.PhanLoais.OrderBy(n => n.PhanLoaiChinh).ToList(), "MaPhanLoai", "PhanLoaiChinh");
            ViewBag.MPL = new SelectList(db.PhanLoaiPhus.OrderBy(n => n.TenPhanLoaiPhu).ToList(), "MaPhanLoaiPhu", "TenPhanLoaiPhu");


            return View(sp);
        }
        public ActionResult delete(string id)
        {
            var sp = db.SanPhams.SingleOrDefault(n => n.MaSanPham == id);
            ViewBag.sp = sp.MaSanPham;
            if (sp == null)
            {

                return View("Index");
            }
            db.SanPhams.Remove(sp);
            db.SaveChanges();


            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult create()
        {
            ViewBag.PLC = new SelectList(db.PhanLoais.ToList().OrderBy(n => n.PhanLoaiChinh), "MaPhanLoai", "PhanLoaiChinh");

            ViewBag.PLP = new SelectList(db.PhanLoaiPhus.ToList().OrderBy(n => n.TenPhanLoaiPhu), "MaPhanLoaiPhu", "TenPhanLoaiPhu");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult create(SanPham sp, HttpPostedFileBase fileupdate)
        {
           
            if (fileupdate == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupdate.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images QLBanChauCanh"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupdate.SaveAs(path);
                    }
                    sp.AnhDaiDien = fileName;
                    db.SanPhams.Add(sp);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

        }
        [HttpGet]
        public ActionResult update(string id)
        {
            var sp = db.SanPhams.SingleOrDefault(n => n.MaSanPham == id);
            ViewBag.PLC = new SelectList(db.PhanLoais.ToList().OrderBy(n => n.PhanLoaiChinh), "MaPhanLoai", "PhanLoaiChinh");

            ViewBag.PLP = new SelectList(db.PhanLoaiPhus.ToList().OrderBy(n => n.TenPhanLoaiPhu), "MaPhanLoaiPhu", "TenPhanLoaiPhu");

            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult update(SanPham sp, HttpPostedFileBase fileupload)
        {
            if (fileupload == null)
            {
                sp.AnhDaiDien = sp.AnhDaiDien;
                db.SanPhams.AddOrUpdate(sp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images QLBanChauCanh"), filename);
if(System.IO.File.Exists(path)) {   
                    ViewBag.ThongBao = "Ảnh đã tồn tại";
                }
                else
                {
                    fileupload.SaveAs(path);
                }
                sp.AnhDaiDien = filename;
                db.SanPhams.AddOrUpdate(sp);
                db.SaveChanges();
            }



            return RedirectToAction("Index");
        }
    }

        public ActionResult dm()
        {
           List<PhanLoaiPhu> sp = db.PhanLoaiPhus.ToList();
            return View(sp);
        }

    } }



