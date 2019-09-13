using ECommerce_Shop.Helpers;
using ECommerce_Shop.Models;
using ECommerce_Shop.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace ECommerce_Shop.Areas.ControlPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private DBEntities _db = new DBEntities();

        public async Task<ActionResult> Index()
        {
            try
            {
                using (_db = new DBEntities())
                {
                    var allCategory = await _db.Categories.ToListAsync();
                    var productVms = await _db.Products.Select(m => new ProductVm
                    {
                        SKU = m.SKU,
                        ProductName = m.ProductName,
                        SubCategoryName = m.Category.Name,
                        MRP = m.MRP,
                        CategoryId = m.CategoryId,
                        SubCategoryId = m.Category.RootCategoryId,
                        Price = m.Price,
                        CoverImage = string.Empty,
                        CategoryName = string.Empty,
                        ProductId = m.ProductId,
                        IsActive = m.IsActive,
                        OfferId = m.OfferId
                    }).ToListAsync();

                    var imageCollection = await _db.ProductImages.ToListAsync();
                    foreach (var product in productVms)
                    {
                        product.CategoryName = allCategory.FirstOrDefault(p => p.CategoryId == product.SubCategoryId)?.Name;
                        product.CoverImage = imageCollection
                            .FirstOrDefault(m => m.ProductId == product.ProductId && m.CoverImage)?.ImageName;
                    }

                    if (!productVms.Any())
                    {
                        productVms = new List<ProductVm>();
                    }

                    return View(productVms);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ActionResult Add()
        {
            return View(new ProductVm());
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Add(ProductVm model)
        {
            try
            {
                #region Product Info

                var product = new Product
                {
                    ProductName = model.ProductName,
                    CategoryId = model.CategoryId,
                    MRP = model.MRP,
                    Price = model.Price,
                    TAX = model.TAX,
                    SKU = model.SKU,
                    Tag = model.Tag,
                    ShortDescription = model.ShortDescription,
                    FullDescription = model.FullDescription,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = StaticValues.UserId,//Static UserId
                    IsActive = true
                };
                _db.Products.Add(product);
                await _db.SaveChangesAsync();

                #endregion

                #region Product Feature(s) 

                var productId = product.ProductId;
                if (model.ProductFeatureDetails != null && model.ProductFeatureDetails.Any())
                {
                    var productFeatureDetails = new List<ProductFeature>();
                    foreach (var feature in model.ProductFeatureDetails)
                    {
                        if (!string.IsNullOrWhiteSpace(feature.FeatureValue) &&
                            !string.IsNullOrWhiteSpace(feature.FeatureType))
                        {
                            productFeatureDetails.Add(new ProductFeature
                            {
                                ProductId = productId,
                                FeatureType = feature.FeatureType,
                                FeatureValue = feature.FeatureValue,
                            });
                        }
                    }

                    if (productFeatureDetails.Any())
                    {
                        _db.ProductFeatures.AddRange(productFeatureDetails);
                    }
                }

                #endregion

                #region Product Size/Stock

                if (model.ProductDetails != null && model.ProductDetails.Any())
                {
                    var productDetails = new List<ProductDetail>();
                    foreach (var details in model.ProductDetails)
                    {
                        if (details.ColorId != 0)
                        {
                            productDetails.Add(new ProductDetail
                            {
                                ProductId = productId,
                                ColorId = details.ColorId,
                                SizeId = details.SizeId
                            });
                        }
                    }

                    if (productDetails.Any())
                    {
                        _db.ProductDetails.AddRange(productDetails);
                    }
                }

                #endregion

                #region Product Images

                var productImageArray = JsonConvert.DeserializeObject<List<ProductImageArrayVm>>(model.ProductImages);
                var virtualPath = StaticValues.ProductImagePath;
                var physicalPath = Server.MapPath(virtualPath);
                if (productImageArray.Any())
                {
                    var productImages = new List<ProductImage>();
                    foreach (var singleImage in productImageArray)
                    {
                        var imageName = SaveImage(singleImage.imageSrc, singleImage.extension, physicalPath);
                        if (!string.IsNullOrEmpty(imageName))
                        {
                            productImages.Add(new ProductImage
                            {
                                ProductId = productId,
                                ImageName = imageName,
                                CoverImage = singleImage.iscover
                            });
                        }
                    }

                    if (productImages.Any())
                    {
                        _db.ProductImages.AddRange(productImages);
                    }
                }

                #endregion

                await _db.SaveChangesAsync();

                return Json(new { status = true, message = SuccessMessage.Added, JsonRequestBehavior.AllowGet });
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message, JsonRequestBehavior.AllowGet });
            }
        }

        public async Task<ActionResult> Edit(long id)
        {
            try
            {
                var productVm = new ProductVm();
                using (_db = new DBEntities())
                {
                    if (id != 0)
                    {
                        var product = await _db.Products.FindAsync(id);
                        if (product != null)
                        {
                            //Get Product Details
                            productVm.ProductId = product.ProductId;
                            productVm.OfferId = product.OfferId;
                            productVm.ProductName = product.ProductName;
                            productVm.CategoryId = product.CategoryId;
                            productVm.MRP = product.MRP;
                            productVm.Price = product.Price;
                            productVm.TAX = product.TAX;
                            productVm.SKU = product.SKU;
                            productVm.Tag = product.Tag;
                            productVm.ShortDescription = product.ShortDescription;
                            productVm.FullDescription = product.FullDescription;

                            //Get Product Size/Stock/QTY Details
                            var productDetails = await _db.ProductDetails.Where(m => m.ProductId == id).Select(m => new ProductDetailsVm
                            {
                                ColorId = m.ColorId,
                                SizeId = m.SizeId
                            }).ToListAsync();

                            productVm.ProductDetails = productDetails.Any() ? productDetails : new List<ProductDetailsVm>();

                            //Get Product Feature Details
                            var productFeatureDetails = await _db.ProductFeatures.Where(m => m.ProductId == id).Select(m => new ProductFeatureDetailsVm
                            {
                                FeatureType = m.FeatureType,
                                FeatureValue = m.FeatureValue
                            }).ToListAsync();

                            productVm.ProductFeatureDetails = productFeatureDetails.Any() ? productFeatureDetails : new List<ProductFeatureDetailsVm>();

                            //Get Product Feature Details
                            var productImagesDetails = await _db.ProductImages.Where(m => m.ProductId == id).Select(m => new ProductImageArrayVm
                            {
                                isdeleted = false,
                                issaved = true,
                                iscover = m.CoverImage,
                                ImageName = m.ImageName,
                                ProductImageId = m.ProductImageId,
                                key = m.ProductImageId
                            }).ToListAsync();

                            productVm.ProductImagesDetails = productImagesDetails.Any() ? productImagesDetails : new List<ProductImageArrayVm>();
                        }
                    }
                }
                return View(productVm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<JsonResult> Edit(ProductVm model)
        {
            try
            {
                #region Product Info

                var existingProductDetails = _db.Products.Find(model.ProductId);
                if (existingProductDetails != null)
                {
                    existingProductDetails.ProductName = model.ProductName;
                    existingProductDetails.OfferId = model.OfferId;
                    existingProductDetails.CategoryId = model.CategoryId;
                    existingProductDetails.MRP = model.MRP;
                    existingProductDetails.Price = model.Price;
                    existingProductDetails.TAX = model.TAX;
                    existingProductDetails.SKU = model.SKU;
                    existingProductDetails.Tag = model.Tag;
                    existingProductDetails.ShortDescription = model.ShortDescription;
                    existingProductDetails.FullDescription = model.FullDescription;
                    existingProductDetails.UpdatedDate = DateTime.UtcNow;
                    existingProductDetails.UpdatedBy = StaticValues.UserId;
                }

                #endregion

                #region Product Feature(s) 

                if (model.ProductFeatureDetails != null && model.ProductFeatureDetails.Any())
                {
                    var productSavedFeatureDetails =
                        await _db.ProductFeatures.Where(m => m.ProductId == model.ProductId).ToListAsync();

                    if (productSavedFeatureDetails.Any())
                    {
                        _db.ProductFeatures.RemoveRange(productSavedFeatureDetails);
                    }

                    var productFeatureDetails = new List<ProductFeature>();
                    foreach (var feature in model.ProductFeatureDetails)
                    {
                        if (!string.IsNullOrWhiteSpace(feature.FeatureValue) &&
                            !string.IsNullOrWhiteSpace(feature.FeatureType))
                        {
                            productFeatureDetails.Add(new ProductFeature
                            {
                                ProductId = model.ProductId,
                                FeatureType = feature.FeatureType,
                                FeatureValue = feature.FeatureValue,
                            });
                        }
                    }

                    if (productFeatureDetails.Any())
                    {
                        _db.ProductFeatures.AddRange(productFeatureDetails);
                    }
                }

                #endregion

                #region Product Size/Stock

                if (model.ProductDetails != null && model.ProductDetails.Any())
                {
                    var productSavedDetails =
                        await _db.ProductDetails.Where(m => m.ProductId == model.ProductId).ToListAsync();

                    if (productSavedDetails.Any())
                    {
                        _db.ProductDetails.RemoveRange(productSavedDetails);
                    }

                    var productDetails = new List<ProductDetail>();
                    foreach (var details in model.ProductDetails)
                    {
                        if (details.ColorId != 0)
                        {
                            productDetails.Add(new ProductDetail
                            {
                                ProductId = model.ProductId,
                                ColorId = details.ColorId,
                                SizeId = details.SizeId
                            });
                        }
                    }

                    if (productDetails.Any())
                    {
                        _db.ProductDetails.AddRange(productDetails);
                    }
                }

                #endregion

                #region Product Images

                var virtualPath = StaticValues.ProductImagePath;
                var physicalPath = Server.MapPath(virtualPath);
                var productImageArray = JsonConvert.DeserializeObject<List<ProductImageArrayVm>>(model.ProductImages);
                if (productImageArray.Any())
                {
                    var productImages = new List<ProductImage>();
                    foreach (var singleImage in productImageArray)
                    {
                        if (singleImage.issaved)
                        {
                            var existingImage = await _db.ProductImages.FindAsync(singleImage.ProductImageId);
                            if (singleImage.issaved && !singleImage.isdeleted)
                            {
                                //update data of existing
                                if (existingImage != null)
                                {
                                    existingImage.CoverImage = singleImage.iscover;
                                }
                            }
                            else
                            {
                                //delete file/image from the folder
                                if (existingImage != null)
                                {
                                    var existingImagePath = Path.Combine(physicalPath, existingImage.ImageName);
                                    if (System.IO.File.Exists(existingImagePath))
                                    {
                                        System.IO.File.Delete(existingImagePath);
                                    }
                                }

                                //delete existing record
                                if (existingImage != null)
                                {
                                    _db.ProductImages.Remove(existingImage);
                                }
                            }
                        }
                        else if (!singleImage.isdeleted)
                        {
                            //add new image
                            var imageName = SaveImage(singleImage.imageSrc, singleImage.extension, physicalPath);
                            if (!string.IsNullOrEmpty(imageName))
                            {
                                productImages.Add(new ProductImage
                                {
                                    ProductId = model.ProductId,
                                    ImageName = imageName,
                                    CoverImage = singleImage.iscover
                                });
                            }

                            if (productImages.Any())
                            {
                                _db.ProductImages.AddRange(productImages);
                            }
                        }
                    }
                }

                #endregion

                await _db.SaveChangesAsync();
                return Json(new { status = true, message = SuccessMessage.Updated, JsonRequestBehavior.AllowGet });
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message, JsonRequestBehavior.AllowGet });
            }
        }

        public ActionResult Vertical()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Test(string things)
        {
            var deserialize = JsonConvert.DeserializeObject<List<ProductImageArrayVm>>(things);
            var virtualPath = StaticValues.ProductImagePath;
            //var physicalPath = Server.MapPath(virtualPath);
            //foreach (var singleImage in values)
            //{

            //    SaveImage(singleImage, physicalPath);
            //}
            return Json(things);
        }

        public string SaveImage(string simpleString, string extension, string path)
        {
            var filename = Guid.NewGuid().ToString() + "." + extension;
            var imagePath = Path.Combine(path, filename);
            var base64 = simpleString.Substring(simpleString.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            var chartData = Convert.FromBase64String(base64);
            System.IO.File.WriteAllBytes(imagePath, chartData);
            return filename;
        }

        [HttpPost]
        public async Task<JsonResult> ActivateDeActivate(long id, bool status)
        {
            try
            {
                if (id != 0)
                {
                    using (_db = new DBEntities())
                    {
                        var product = await _db.Products.FindAsync(id);
                        if (product != null)
                        {
                            product.IsActive = status;
                            _db.Entry(product).State = EntityState.Modified;
                            _db.SaveChanges();
                            var message = status ? SuccessMessage.Activated : SuccessMessage.DeActivated;
                            return Json(new { status = true, message }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, message = ErrorMessage.DataNotFound }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            try
            {
                if (id == 0)
                {
                    return Json(new { status = false, message = ErrorMessage.DataNotFound },
                        JsonRequestBehavior.AllowGet);
                }

                using (_db = new DBEntities())
                {
                    var product = await _db.Products.FindAsync(id);

                    if (product == null)
                    {
                        return Json(new { status = false, message = ErrorMessage.DataNotFound },
                            JsonRequestBehavior.AllowGet);
                    }

                    _db.Products.Remove(product);

                    var productDetails = await _db.ProductDetails.Where(m => m.ProductId == id).ToListAsync();
                    if (productDetails.Any())
                    {
                        _db.ProductDetails.RemoveRange(productDetails);
                    }

                    var productFeatures = await _db.ProductFeatures.Where(m => m.ProductId == id).ToListAsync();
                    if (productFeatures.Any())
                    {
                        _db.ProductFeatures.RemoveRange(productFeatures);
                    }

                    var productImages = await _db.ProductImages.Where(m => m.ProductId == id).ToListAsync();
                    if (productImages.Any())
                    {

                        var virtualPath = StaticValues.ProductImagePath;
                        var physicalPath = Server.MapPath(virtualPath);
                        foreach (var imageDetail in productImages)
                        {
                            var existingImagePath = Path.Combine(physicalPath, imageDetail.ImageName);
                            if (System.IO.File.Exists(existingImagePath))
                            {
                                System.IO.File.Delete(existingImagePath);
                            }
                        }
                        _db.ProductImages.RemoveRange(productImages);
                    }

                    await _db.SaveChangesAsync();
                    return Json(new { status = true, message = SuccessMessage.Deleted }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CloneProduct(long id)
        {
            try
            {
                using (_db = new DBEntities())
                {
                    if (id != 0)
                    {
                        var product = await _db.Products.FindAsync(id);
                        if (product != null)
                        {
                            _db.Products.Add(product);
                            await _db.SaveChangesAsync();

                            var productDetails = await _db.ProductDetails.Where(m => m.ProductId == id).ToListAsync();
                            if (productDetails.Any())
                            {
                                productDetails.ForEach(m => { m.ProductId = product.ProductId; });
                                _db.ProductDetails.AddRange(productDetails);
                            }

                            var productFeatures = await _db.ProductFeatures.Where(m => m.ProductId == id).ToListAsync();
                            if (productFeatures.Any())
                            {
                                productFeatures.ForEach(m => { m.ProductId = product.ProductId; });
                                _db.ProductFeatures.AddRange(productFeatures);
                            }

                            var productImages = await _db.ProductImages.Where(m => m.ProductId == id).ToListAsync();
                            var coverImageName = string.Empty;
                            if (productImages.Any())
                            {
                                var virtualPath = StaticValues.ProductImagePath;
                                var physicalPath = Server.MapPath(virtualPath);
                                var newProductImages = new List<ProductImage>();
                                foreach (var imageDetails in productImages)
                                {
                                    var imageName = imageDetails.ImageName.Split('.');
                                    var newImage = Guid.NewGuid().ToString() + "." + imageName[1];
                                    var oldImage = Path.Combine(physicalPath, imageDetails.ImageName);
                                    var cloneImage = Path.Combine(physicalPath, newImage);
                                    if (System.IO.File.Exists(oldImage))
                                    {
                                        System.IO.File.Copy(oldImage, cloneImage);
                                    }

                                    if (imageDetails.CoverImage)
                                    {
                                        coverImageName = Url.Content(StaticValues.ProductImagePath + newImage);
                                    }

                                    newProductImages.Add(new ProductImage
                                    {
                                        ProductId = product.ProductId,
                                        ImageName = newImage,
                                        CoverImage = imageDetails.CoverImage
                                    });
                                }
                                if (newProductImages.Any())
                                {
                                    _db.ProductImages.AddRange(newProductImages);
                                }
                            }

                            await _db.SaveChangesAsync();

                            return Json(new { status = true, message = SuccessMessage.Cloned, product.ProductId, coverImageName }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    return Json(new { status = false, message = ErrorMessage.DataNotFound }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}