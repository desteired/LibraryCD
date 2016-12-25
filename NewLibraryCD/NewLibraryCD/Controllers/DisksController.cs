using MimeTypes;
using NewLibraryCD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace NewLibraryCD.Controllers
{
    public class DisksController : Controller
    {       
        public ActionResult GetPhoto(int id)
        {
            using (ModelDataBaseContext context = new ModelDataBaseContext())
            {
                Photo getPhoto = context.Disks.FirstOrDefault(x => x.DiskId == id).Photo;

                if(getPhoto == null)
                {
                    string mime = MimeTypeMap.GetMimeType(System.IO.Path.GetExtension(HttpContext.Server.MapPath("~/Content/no-img.jpg")));
                    byte[] data = System.IO.File.ReadAllBytes(HttpContext.Server.MapPath("~/Content/no-img.jpg"));
                    return File(data, mime);
                }
                else
                {
                    return File(getPhoto.ImageData, getPhoto.Mime);
                }
            }
        }

        // метод для вывода списка CD-дисков
        [Authorize]
        public ActionResult Index(int? page)
        {
            
            int pageSize = 3;
            int pageNumber = (page ?? 1);


            List <DiskViewModel> list = new List<DiskViewModel>();

            using (ModelDataBaseContext context = new ModelDataBaseContext())
            {
                List<Disk> listDisk = context.Disks.ToList();

                listDisk = context.Disks.ToList();

                Rating r = null;

                foreach (Disk d in listDisk)
                {
                    DiskViewModel dvm = new DiskViewModel();
                    r = context.Ratings.Where(x => x.Disk.DiskId == d.DiskId && x.User.UserName == User.Identity.Name).FirstOrDefault();
                    if (r != null)
                    {
                        dvm.YourRating = r.UserRating;
                    }
                    else
                    {
                        dvm.YourRating = 0;
                    }

                    dvm.Description = d.Description;
                    dvm.DirectionName = d.Direction.NameDirection;
                    dvm.DiskId = d.DiskId;
                    dvm.NameCD = d.NameCD;
                    dvm.Photo = d.Photo;
                    dvm.Rating = d.TotalRating;
                    dvm.Singer = d.Singer;
                    dvm.Year = d.Year;

                    list.Add(dvm);

                }
            }
            return View(list.ToPagedList(pageNumber,pageSize));


        }
        // GET: AddDissk
        [Authorize]
        public ActionResult Add()
        {
            DiskViewModel model = new DiskViewModel();
             using (ModelDataBaseContext context = new ModelDataBaseContext())
            {

                var getDirection = context.Directions.ToList();
                SelectList ListDirection = new SelectList(getDirection, "DirectionId", "NameDirection");
                ViewBag.DirectionList = ListDirection;

            }
                
                return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Add(DiskViewModel model, int selectedDirection, HttpPostedFileBase file)
        {

                if (file != null)
                {
                    string MimeType = file.ContentType;
                    if (file != null && !MimeType.Contains("image")) ModelState.AddModelError(nameof(model.Photo), "Images only allowed");
                }

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    byte[] data = new byte[file.ContentLength];
                    file.InputStream.Read(data, 0, data.Length);
                    model.Photo = new Photo() { ImageData = data, Mime = file.ContentType };
                }

                Disk newDisk = new Disk();
                newDisk.Description = model.Description;
                newDisk.NameCD = model.NameCD;
                newDisk.TotalRating = 0;
                newDisk.Singer = model.Singer;
                newDisk.Year = model.Year;
                newDisk.Photo = model.Photo;

                
                using (ModelDataBaseContext context = new ModelDataBaseContext())
                {
                    newDisk.Direction = context.Directions.FirstOrDefault(x => x.DirectionId == selectedDirection);
                    context.Disks.Add(newDisk);
                    context.SaveChanges();
                }

                return RedirectToAction("Index", "Disks");

            }
            else
            {
                using (ModelDataBaseContext context = new ModelDataBaseContext())
                {
                    var getDirection = context.Directions.ToList();
                    SelectList ListDirection = new SelectList(getDirection, "DirectionId", "NameDirection");
                    ViewBag.DirectionList = ListDirection;
                    
                }
                return View(model);
            }
            
        }

        public ActionResult Filter(string singer, string direction, int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            List<DiskViewModel> list = new List<DiskViewModel>();

            using (ModelDataBaseContext context = new ModelDataBaseContext())
            {
                List<Disk> listDisk = context.Disks.ToList();

                listDisk = context.Disks.Where(x => x.Direction.NameDirection.Contains(direction) && x.Singer.Contains(singer)).ToList();

                Rating r = null;

                foreach (Disk d in listDisk)
                {
                    DiskViewModel dvm = new DiskViewModel();
                    r = context.Ratings.Where(x => x.Disk.DiskId == d.DiskId && x.User.UserName == User.Identity.Name).FirstOrDefault();
                    if (r != null)
                    {
                        dvm.YourRating = r.UserRating;
                    }
                    else
                    {
                        dvm.YourRating = 0;
                    }

                        dvm.Description = d.Description;
                        dvm.DirectionName = d.Direction.NameDirection;
                        dvm.DiskId = d.DiskId;
                        dvm.NameCD = d.NameCD;
                        dvm.Photo = d.Photo;
                        dvm.Rating = d.TotalRating;
                        dvm.Singer = d.Singer;
                        dvm.Year = d.Year;

                    list.Add(dvm);
                    
                }
            }

            return PartialView("_DiskList", list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AutoCompleteDirection(string term)
        {
            
            string[] arrDirections = null;
            using (ModelDataBaseContext context = new ModelDataBaseContext())
            {
                List<Direction> dList = context.Directions.Where(x => x.NameDirection.Contains(term)).ToList();

                arrDirections = new string[dList.Count];
                int i = 0;
                foreach (Direction d in dList)
                {
                    arrDirections[i] = d.NameDirection;
                    i++;
                }
            }

            return Json(arrDirections, JsonRequestBehavior.AllowGet);

        }
        public ActionResult AutoCompleteSinger(string term)
        {

            string[] arrSingers = null;
            using (ModelDataBaseContext context = new ModelDataBaseContext())
            {
                List<Disk> sList = context.Disks.Where(x => x.Singer.Contains(term)).ToList();

                arrSingers = new string[sList.Count];
                int i = 0;
                foreach (Disk d in sList)
                {
                    arrSingers[i] = d.Singer;
                    i++;
                }
            }

            return Json(arrSingers, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult DeleteDisk(int diskId)
        {

            using (ModelDataBaseContext context = new ModelDataBaseContext())
            {
                Disk d = context.Disks.FirstOrDefault(x => x.DiskId == diskId);
                if(d != null)
                {
                    context.Disks.Remove(d);
                    context.SaveChanges();
                    return Json(true);
                }
                return Json(false);                
            }
        }

        // метод вызывающий вьюшку для отрисовки графика
        [Authorize]
        public ActionResult Graph()
        {
            SelectList listDirection = null;
            using (ModelDataBaseContext context = new ModelDataBaseContext())
            {
                var getDirection = context.Directions.ToList();
                listDirection = new SelectList(getDirection, "DirectionId", "NameDirection");
                ViewBag.DirectionList = listDirection;

            }
            return View();

        }

        // метод для получения массива оценок
        [HttpPost]
        public ActionResult GetArrValue(int directionId)
        {
            double[] arrRating = new double[DateTime.Now.Year - 1949];

            using (ModelDataBaseContext context = new ModelDataBaseContext())
            {
                List<Rating> rList = null;
                    
                // если муз. направление не выбрано, то делаем среднюю оценку по всем направлениям
                if (directionId == 0)
                {
                    double totRat = 0;
                    
                    for (int i = 1950, step = 0; i <= DateTime.Now.Year; i++, step++)
                    {
                        rList = context.Ratings.Where(x => x.Disk.Year == i).ToList();
                        if (rList.Count==0)
                        {
                            arrRating[step] = 0;
                            continue;
                        }
                        else
                        {
                            foreach (Rating r in rList)
                            {
                                totRat = totRat + r.Disk.TotalRating;
                                
                            }
                        }

                        totRat = totRat / rList.Count;
                        arrRating[step] = totRat;
                        totRat = 0;
                    }

                    return Json(arrRating);

                }
                else
                {
                    double totRat = 0;

                    for (int i = 1950, step = 0; i <= DateTime.Now.Year; i++, step++)
                    {
                        rList = context.Ratings.Where(x => x.Disk.Year == i && x.Disk.Direction.DirectionId == directionId).ToList();
                        if (rList.Count == 0)
                        {
                            arrRating[step] = 0;
                            continue;
                        }
                        else
                        {
                            foreach (Rating r in rList)
                            {
                                totRat = totRat + r.Disk.TotalRating;

                            }
                        }

                        totRat = totRat / rList.Count;
                        arrRating[step] = totRat;
                        totRat = 0;
                    }

                    return Json(arrRating);
                }
            }

        }

        // метод для присвоения оценки
        [HttpPost]
        public ActionResult Rate(int rating, int disk)
        {
            double total = 0;
            using (ModelDataBaseContext context = new ModelDataBaseContext())
            {
                User u = context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
                Disk d = context.Disks.Where(x => x.DiskId == disk).FirstOrDefault();

                Rating newR = context.Ratings.Where(x => x.Disk.DiskId == d.DiskId && x.User.UserId == u.UserId).FirstOrDefault();

                if (newR == null)
                {
                    context.Ratings.Add(new Rating() { Disk = d, User = u, UserRating = rating });
                    context.SaveChanges();
                }
                else
                {
                    context.Ratings.Where(x => x.Disk.DiskId == d.DiskId && x.User.UserId == u.UserId).FirstOrDefault().UserRating = rating;
                    context.SaveChanges();
                }
                

                List<Rating> listRating = context.Ratings.Where(x => x.Disk.DiskId == d.DiskId).ToList();

                int i = 0;

                foreach (Rating r in listRating)
                {
                    total = total + r.UserRating;
                    i++;
                }

                total = total / i;
                total = Math.Round(total, 2);

                context.Disks.Where(x => x.DiskId == disk).FirstOrDefault().TotalRating = total;

                context.SaveChanges();
            }


                return Json(total);
        }
    }
}