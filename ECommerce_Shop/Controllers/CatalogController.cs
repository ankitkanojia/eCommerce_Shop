using ECommerce_Shop.Helpers;
using ECommerce_Shop.Models;
using ECommerce_Shop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ECommerce_Shop.Controllers
{
    public class CatalogController : Controller
    {
        private readonly DBEntities _db = new DBEntities();

        public ActionResult Index(long? id)
        {
            try
            {
                using (_db)
                {
                    var tuple = IndexTuple(id);
                    return View(tuple);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(long productId, double fromPrice, double toPrice, string colorIds, string filterIds, string sizeIds)
        {
            try
            {
                using (_db)
                {
                    var tuple = IndexTuple(productId);

                    var colorId = new List<long>();
                    var filterId = new List<long>();
                    var sizeId = new List<long>();

                    if (!string.IsNullOrWhiteSpace(colorIds))
                    {
                        var splitColors = colorIds.Split(',');
                        colorId.AddRange(from item in splitColors where !string.IsNullOrWhiteSpace(item) select Convert.ToInt64(item));
                    }

                    if (!string.IsNullOrWhiteSpace(filterIds))
                    {
                        var splitFilter = filterIds.Split(',');
                        filterId.AddRange(from item in splitFilter where !string.IsNullOrWhiteSpace(item) select Convert.ToInt64(item));
                    }

                    if (!string.IsNullOrWhiteSpace(sizeIds))
                    {
                        var splitSize = sizeIds.Split(',');
                        sizeId.AddRange(from item in splitSize where !string.IsNullOrWhiteSpace(item) select Convert.ToInt64(item));
                    }

                    if (tuple.Item1.Any())
                    {

                        var filterData = new List<ProductVm>();
                        var sizeData = new List<ProductVm>();
                        var colorData = new List<ProductVm>();
                        var priceData = new List<ProductVm>();


                        //Filter by FilterData
                        if (filterId.Any())
                        {
                            foreach (var l in filterId)
                            {
                                var fod = tuple.Item3.FilterDataVms.FirstOrDefault(s => s.FilterDataId == l);
                                if (fod != null)
                                {
                                    tuple.Item3.FilterDataVms.FirstOrDefault(s => s.FilterDataId == l).IsChecked = true;
                                }

                                foreach (var item in tuple.Item1)
                                {
                                    if (item.FilterDataId == l)
                                    {
                                        filterData.Add(item);
                                    }
                                }
                            }
                        }


                        //Filter by Sizes
                        if (sizeId.Any())
                        {
                            foreach (var l in sizeId)
                            {
                                foreach (var item in tuple.Item1)
                                {
                                    if (item.ProductDetails.Where(s => s.SizeId == l).ToList().Any())
                                    {
                                        sizeData.Add(item);
                                    }
                                }
                            }
                        }

                        //Filter by Colors
                        if (colorId.Any())
                        {
                            foreach (var l in colorId)
                            {
                                foreach (var item in tuple.Item1)
                                {
                                    if (item.ProductDetails.Where(s => s.ColorId == l).ToList().Any())
                                    {
                                        colorData.Add(item);
                                    }
                                }
                            }
                        }

                        //Filter by Prices
                        if (fromPrice >= 0 && toPrice >= fromPrice)
                        {
                            foreach (var l in filterId)
                            {
                                foreach (var item in tuple.Item1)
                                {
                                    if (item.Price >= fromPrice && item.Price <= toPrice)
                                    {
                                        priceData.Add(item);
                                    }
                                }
                            }
                        }



                        //intercept operation here
                        var finalData = new List<ProductVm>();
                        if (filterData.Any())
                        {
                            finalData.AddRange(filterData.ToList());
                        }

                        if (sizeData.Any())
                        {
                            var sizeCommonIds = sizeData.Select(m => m.ProductId).ToList();
                            if (sizeCommonIds.Any() && finalData.Any())
                            {
                                finalData = finalData.Where(m => sizeCommonIds.Contains(m.ProductId)).ToList();
                            }
                            else
                            {
                                finalData = sizeData;
                            }
                        }

                        if (colorData.Any())
                        {
                            var colorCommonIds = colorData.Select(m => m.ProductId).ToList();
                            if (colorCommonIds.Any() && finalData.Any())
                            {
                                finalData = finalData.Where(m => colorCommonIds.Contains(m.ProductId)).ToList();
                            }
                            else
                            {
                                finalData = colorData;
                            }
                        }

                        if (priceData.Any())
                        {
                            var priceCommonIds = priceData.Select(m => m.ProductId).ToList();
                            if (priceCommonIds.Any() && finalData.Any())
                            {
                                finalData = finalData.Where(m => priceCommonIds.Contains(m.ProductId)).ToList();
                            }
                            else
                            {
                                finalData = new List<ProductVm>();
                            }
                        }
                        //Niche na 4 object ma product id same hoy eva  data tare finalData na object ma nakhva na
                        //var filterData = new List<ProductVm>();
                        //var sizeData = new List<ProductVm>();
                        //var colorData = new List<ProductVm>();
                        //var priceData = new List<ProductVm>();


                        var newTuple = new Tuple<List<ProductVm>, List<Category>, CatelogVm>(finalData, tuple.Item2, tuple.Item3);

                        return View(newTuple);
                    }

                    return View(tuple);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public Tuple<List<ProductVm>, List<Category>, CatelogVm> IndexTuple(long? id)
        {
            try
            {
                using (_db)
                {
                    #region --> Get Product and Category Details

                    var products = new List<ProductVm>();
                    if (id == 5)
                    {
                        products = CommonFunctions.GetProductsVms(null, 44);
                    }
                    else if (id == 43)
                    {
                        products = CommonFunctions.GetNewArrivalsProductsVms(null, 43);
                    }
                    else if (id == 8)
                    {
                        var db = new DBEntities();
                        var productDetails = db.Products.Where(s => s.Category.RootCategoryId == 2).OrderByDescending(m => m.CreatedDate).ToList();
                        if (productDetails.Any())
                        {
                            var offers = db.Offers.Where(m => m.OfferId != 0).ToList();
                            products = productDetails.Select(s => new ProductVm
                            {
                                OfferTitle = (s.OfferId != 0) ? offers.FirstOrDefault(m => m.OfferId == s.OfferId)?.Title : string.Empty,
                                CategoryId = s.CategoryId,
                                CategoryName = s.Category.Name,
                                CoverImage = s.ProductImages.FirstOrDefault(k => k.ProductId == s.ProductId && k.CoverImage)?.ImageName,
                                FullDescription = s.FullDescription,
                                MRP = s.MRP,
                                Price = s.Price,
                                ProductDetails = s.ProductDetails.Where(k => k.ProductId == s.ProductId).Select(k => new ProductDetailsVm { ColorId = k.ColorId, SizeId = k.SizeId }).ToList(),
                                ProductFeatureDetails = s.ProductFeatures.Where(k => k.ProductId == s.ProductId).Select(k => new ProductFeatureDetailsVm { FeatureType = k.FeatureType, FeatureValue = k.FeatureValue }).ToList(),
                                ProductId = s.ProductId,
                                ProductName = s.ProductName,
                                SKU = s.SKU,
                                ShortDescription = s.ShortDescription,
                                TAX = s.TAX,
                                Tag = s.Tag,
                                ProductImagesDetails = s.ProductImages.Where(k => k.ProductId == s.ProductId).Select(k => new ProductImageArrayVm { ProductId = k.ProductId, ImageName = k.ImageName, ProductImageId = k.ProductImageId, iscover = k.CoverImage }).ToList(),
                                CreatedDate = s.CreatedDate
                            }).OrderByDescending(p => p.CreatedDate).ToList();
                        }
                    }
                    else
                    {
                        products = CommonFunctions.GetProductsVms(null, id);
                    }

                    var category = _db.Categories.Where(s => s.IsActive).ToList();

                    var catelogVm = new CatelogVm
                    {
                        CategoryName = category.FirstOrDefault(s => s.CategoryId == id)?.Name,
                        FilterDataVms = new List<FilterDataVm>()
                    };

                    if (!string.IsNullOrWhiteSpace(catelogVm.CategoryName) || catelogVm.CategoryName == "one size")
                    {
                        catelogVm.CategoryName = "All Categories";
                    }

                    if (string.IsNullOrWhiteSpace(catelogVm.CategoryName))
                    {
                        catelogVm.CategoryName = "All Categories";
                    }

                    if (id != null && id > 0)
                    {
                        catelogVm.CategoryId = id.Value;
                        category = category.Where(s => s.RootCategoryId == id.Value).ToList();
                    }
                    else
                    {
                        catelogVm.CategoryId = 0;
                        category = category.Where(s => s.RootCategoryId == 0 && s.CategoryId > 0).ToList();
                    }
                    #endregion

                    #region --> Get Filter Data

                    if (id != null && id > 0)
                    {
                        catelogVm.Sizes = _db.Sizes.Where(s => s.SizeId > 0 && s.CategoryId == id).Select(s => new SizeVm
                        {
                            CategoryId = s.CategoryId,
                            SizeId = s.SizeId,
                            Name = s.Name,
                            CategoryName = s.Category.Name,
                            IsChecked = false
                        }).ToList();
                        catelogVm.ProductId = id.Value;
                    }
                    else
                    {
                        catelogVm.Sizes = _db.Sizes.Where(s => s.SizeId == 0).Select(s => new SizeVm
                        {
                            CategoryId = s.CategoryId,
                            SizeId = s.SizeId,
                            Name = s.Name,
                            CategoryName = s.Category.Name,
                            IsChecked = false
                        }).ToList();
                        catelogVm.ProductId = 0;
                    }

                    #endregion 

                    return new Tuple<List<ProductVm>, List<Category>, CatelogVm>(products, category, catelogVm);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ActionResult Detail(long id)
        {
            try
            {
                using (_db)
                {
                    var products = CommonFunctions.GetProductsVms(null, null);
                    var product = products.FirstOrDefault(s => s.ProductId == id);
                    var relatedProduct = new List<ProductVm>();

                    var catelogVm = new CatelogVm
                    {
                        Colors = new List<ColorVm>(),
                        Sizes = new List<SizeVm>()
                    };

                    if (product?.ProductDetails != null)
                    {
                        var sizes = _db.Sizes.ToList();
                        var colors = _db.Colors.ToList();

                        foreach (var item in product.ProductDetails)
                        {
                            var addSize = sizes.Where(s => s.SizeId == item.SizeId).Select(s => new SizeVm
                            {
                                CategoryId = s.CategoryId,
                                SizeId = s.SizeId,
                                Name = s.Name,
                                CategoryName = s.Category.Name,
                                IsChecked = false
                            }).FirstOrDefault();

                            var addColor = colors.Where(s => s.ColorId == item.ColorId).Select(s => new ColorVm
                            {
                                Name = s.Name,
                                ColorId = s.ColorId,
                                Hex = s.Hex,
                                IsChecked = false
                            }).FirstOrDefault();

                            if (addSize != null && catelogVm.Sizes.FirstOrDefault(s => s.SizeId == item.SizeId) == null)
                            {
                                catelogVm.Sizes.Add(addSize);
                            }

                            if (addColor != null && catelogVm.Colors.FirstOrDefault(s => s.ColorId == item.ColorId) == null)
                            {
                                catelogVm.Colors.Add(addColor);
                            }
                        }
                    }

                    if (product != null)
                    {
                        relatedProduct = products.Where(s => s.CategoryId == product.CategoryId).Take(4).ToList();
                    }

                    var tuple = new Tuple<ProductVm, CatelogVm, List<ProductVm>>(product, catelogVm, relatedProduct);
                    return View(tuple);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public JsonResult AddToCart(long productId = 0, long sizeId = 0, int quantity = 0, long colorId = 0)
        {
            try
            {
                using (_db)
                {
                    var cartCookie = CookieHelper.Get(StaticValues.CookieNameCartCookie);

                    if (string.IsNullOrWhiteSpace(cartCookie))
                    {
                        //Create a cookie
                        cartCookie = Guid.NewGuid().ToString();
                        CookieHelper.Set(StaticValues.CookieNameCartCookie, cartCookie);
                    }
                    var size = _db.Sizes.FirstOrDefault(s => s.SizeId == sizeId);
                    var product = _db.Products.FirstOrDefault(s => s.ProductId == productId);

                    if (product != null && size != null)
                    {
                        var isOfferAvailable = (product.OfferId != 0);
                        var totalDiscout = 0d;
                        var offer = new Offer();
                        if (isOfferAvailable)
                        {
                            offer = _db.Offers.Find(product.OfferId);
                        }

                        //Check that product is available on cart
                        var availability = _db.CartDetails.FirstOrDefault(s => s.ProductId == productId && s.CartCookie == cartCookie && s.ColorId != colorId && s.SizeName != size.Name);

                        if (availability != null)
                        {
                            //Remove existing data
                            _db.CartDetails.Remove(availability);
                        }


                        if (isOfferAvailable)
                        {
                            offer = _db.Offers.Find(product.OfferId);
                            if (offer != null && offer.DiscountId == (int)EnumList.DiscountTypes.FlatDiscount)
                            {
                                if (offer.FlatDiscount != null)
                                {
                                    totalDiscout = (int)offer.FlatDiscount;
                                }
                            }
                            if (offer != null && offer.DiscountId == (int)EnumList.DiscountTypes.Percentage)
                            {
                                if (offer.DiscountPercentage != null)
                                {
                                    var percentage = (double)offer.DiscountPercentage.Value / 100;
                                    totalDiscout = (double) (product.Price * quantity * percentage);
                                }
                            }
                            if (offer != null && offer.DiscountId == (int)EnumList.DiscountTypes.SpecificOffer)
                            {
                                if (offer.SpecificItems != null)
                                {
                                    var exactDiscountItem = Math.Abs(quantity / (int)offer.SpecificItems);
                                    if (offer.SpecificValue != null)
                                    {
                                        var minusAmount = (int)(exactDiscountItem * offer.SpecificValue);
                                        var exactAmount = (product.Price * (int)offer.SpecificItems);
                                        totalDiscout = exactAmount - minusAmount;
                                    }
                                }
                            }
                        }

                        var cartDetail = new CartDetail
                        {
                            CartCookie = cartCookie,
                            Quantity = quantity,
                            SizeName = size.Name,
                            ColorId = colorId,
                            ProductId = productId,
                            Price = product.Price,
                            ContactId = 0,
                            CreatedDate = DateTime.UtcNow,
                            MRP = product.MRP,
                            TAX = product.TAX,
                            Discount = totalDiscout,
                            OfferTitle = (isOfferAvailable) ? offer.Title : string.Empty
                        };

                        cartDetail.CreatedDate = DateTime.UtcNow;
                        _db.CartDetails.Add(cartDetail);
                        _db.SaveChanges();

                        return Json(new { status = true, cartItems = _db.CartDetails.Count(s => s.CartCookie == cartCookie), JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { status = false, JsonRequestBehavior.AllowGet });
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}