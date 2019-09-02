using ECommerce_Shop.Models;
using ECommerce_Shop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ECommerce_Shop.Helpers
{
    public static class CommonFunctions
    {
        public static EmailTemplete GetEmailTemplete(long templeteId, bool isDefaultReplacementInsert, Dictionary<string, string> replacement = null)
        {
            try
            {
                using (var db = new DBEntities())
                {
                    var templete = db.EmailTempletes.Find(templeteId);

                    if (templete == null)
                    {
                        return null;
                    }

                    if (replacement != null && replacement.Any())
                    {
                        templete.Body = replacement.Aggregate(templete.Body, (current, value) => current.Replace(value.Key, value.Value));

                        if (isDefaultReplacementInsert)
                        {
                            var defaultReplacement = new Dictionary<string, string>
                            {
                                {"#copyrightname#", StaticValues.EmailCopyrightName},
                                {"#copyrightyear#", StaticValues.EmailCopyrightYear},
                                {"#website#", StaticValues.EmailWebsite},
                                {"#signaturename#", StaticValues.EmailSignatureName},
                                {"#signatureimg#", StaticValues.EmailSignatureImg},
                                {"#linkfacebook#", StaticValues.EmaiLinkFacebook},
                                {"#linktwitter#", StaticValues.EmaiLinkTwitter},
                                {"#linklinkedin#", StaticValues.EmaiLinkLinkedin},
                                {"#logolight#", StaticValues.EmaiLogoLight},
                                {"#logodark#", StaticValues.EmaiLogodDark}
                            };

                            templete.Body = defaultReplacement.Aggregate(templete.Body, (current, value) => current.Replace(value.Key, value.Value));
                        }
                    }

                    return templete;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static List<SelectListItem> GetGenderDropDown()
        {
            var response = new List<SelectListItem>();
            try
            {
                response.Add(new SelectListItem { Text = "Select Gender", Value = "" });
                response.Add(new SelectListItem { Text = "Male", Value = "Male" });
                response.Add(new SelectListItem { Text = "Female", Value = "Female" });
                response.Add(new SelectListItem { Text = "Herm", Value = "Herm" });
                response.Add(new SelectListItem { Text = "Androgyny", Value = "Androgyny" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return response;
        }

        public static List<SelectListItem> GetCategories(long? id, bool isMasterAdded = true, bool isOneSizeAdded = false)
        {
            var response = new List<SelectListItem>();
            try
            {
                using (var db = new DBEntities())
                {

                    response.Add(new SelectListItem
                    {
                        Text = "Select Category",
                        Value = ""
                    });

                    if (isOneSizeAdded)
                    {
                        response.Add(new SelectListItem
                        {
                            Text = "One Size",
                            Value = "0"
                        });
                    }

                    var cats = db.Categories.Where(m => m.IsActive).ToList();

                    if (cats.Any())
                    {
                        if (!isMasterAdded)
                        {
                            //Remove master category from cats object
                            var masterCatIds = cats.Where(s => s.RootCategoryId != 0).Select(s => s.RootCategoryId).Distinct().ToList();

                            if (masterCatIds.Any())
                            {
                                cats = cats.Where(m => masterCatIds.Contains(m.RootCategoryId)).ToList();
                            }
                        }

                        var categories = cats.Select(p => new SelectListItem
                        {
                            Value = p.CategoryId.ToString(),
                            Text = p.Name,
                            Selected = (p.CategoryId == id)
                        }).OrderBy(m => m.Value).ToList();
                        response.AddRange(categories);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return response;
        }

        public static SelectList GetCategoriesGroupWise(long? id, bool isMasterAdded = true, bool isOneSizeAdded = false)
        {
            var categoryList = new List<SelectListItem>();
            try
            {
                using (var db = new DBEntities())
                {

                    categoryList.Add(new SelectListItem
                    {
                        Text = "Select Category",
                        Value = "0"
                    });

                    if (isOneSizeAdded)
                    {
                        categoryList.Add(new SelectListItem
                        {
                            Text = "One Size",
                            Value = "0",
                            Group = new SelectListGroup { Name = "One Size" }
                        });
                    }

                    var uniformCategory = Convert.ToInt32(StaticValues.UNIFORMCategory);
                    var serviceCategory = Convert.ToInt32(StaticValues.SERVICECategory);
                    var studioCategory = Convert.ToInt32(StaticValues.STUDIOCategory);
                    var cats = db.Categories.Where(m => m.IsActive && m.RootCategoryId != serviceCategory && m.RootCategoryId != studioCategory).ToList();
                    var allCategory = cats;

                    var groupList = new List<string>();
                    if (cats.Any())
                    {
                        if (!isMasterAdded)
                        {
                            //Remove master category from cats object
                            var masterCatIds = cats.Where(s => s.RootCategoryId != 0).Select(s => s.RootCategoryId).Distinct().ToList();

                            if (masterCatIds.Any())
                            {
                                cats = cats.Where(m => masterCatIds.Contains(m.RootCategoryId)).ToList();
                            }
                        }

                        var selectedCategories = new List<ListItem>();
                        foreach (var category in cats)
                        {
                            var newItem = new ListItem();
                            var groupName = string.Empty;
                            if (category.RootCategoryId == uniformCategory)
                            {
                                groupName = "SERVICES - UNIFORMS";
                            }
                            else
                            {
                                var groupDetails = allCategory.FirstOrDefault(m => m.CategoryId == category.RootCategoryId);
                                if (groupDetails != null)
                                {
                                    groupName = groupDetails.Name;
                                }
                            }

                            if (!string.IsNullOrEmpty(groupName) && !groupList.Contains(groupName))
                            {
                                var newGroup = new SelectListGroup { Name = groupName };
                                categoryList.Add(new SelectListItem
                                {
                                    Value = category.CategoryId.ToString(),
                                    Text = category.Name,
                                    Selected = (category.CategoryId == id),
                                    Group = newGroup
                                });
                            }
                            else
                            {
                                categoryList.Add(new SelectListItem
                                {
                                    Value = category.CategoryId.ToString(),
                                    Text = category.Name,
                                    Selected = (category.CategoryId == id),
                                    Group = categoryList.FirstOrDefault(m => m.Group.Name.Equals(groupName))?.Group
                                });
                            }
                        }

                        if (categoryList.Any())
                        {
                            categoryList = categoryList.Where(m => m.Text != "All Catagories").ToList();
                        }

                        var response = new SelectList(categoryList, "Value", "Text", "Group.Name", -1);
                        return response;

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return new SelectList(new List<SelectListItem>());
        }

        public static SelectList GetRootCategoriesGroupWise(long? id, bool isMasterAdded = true)
        {
            var categoryList = new List<SelectListItem>();
            try
            {
                using (var db = new DBEntities())
                {

                    categoryList.Add(new SelectListItem
                    {
                        Text = "Select Category",
                        Value = "0"
                    });

                    var uniformCategory = Convert.ToInt32(StaticValues.UNIFORMCategory);
                    var serviceCategory = Convert.ToInt32(StaticValues.SERVICECategory);
                    var studioCategory = Convert.ToInt32(StaticValues.STUDIOCategory);
                    var cats = db.Categories.Where(m => m.IsActive && m.RootCategoryId != serviceCategory && m.RootCategoryId != studioCategory).ToList();
                    var allCategory = cats;

                    var groupList = new List<string>();
                    if (cats.Any())
                    {
                        //Remove master category from cats object
                        var newRootGroup = new SelectListGroup { Name = "Root Categories" };
                        var masterCatIds = cats.Where(s => s.RootCategoryId != 0).Select(s => s.RootCategoryId)
                            .Distinct().ToList();
                        foreach (var masterCat in masterCatIds)
                        {
                            if (uniformCategory == masterCat)
                            {
                                var categoryDetails =
                                    db.Categories.FirstOrDefault(m => m.CategoryId == masterCat && m.CategoryId != 0);

                                if (categoryDetails != null)
                                {
                                    var rootCategory = new SelectListItem
                                    {
                                        Value = categoryDetails.CategoryId.ToString(),
                                        Text = categoryDetails.Name,
                                        Selected = (categoryDetails.CategoryId == id),
                                        Group = newRootGroup
                                    };

                                    categoryList.Add(rootCategory);
                                }
                            }
                            else
                            {
                                var categoryDetails =
                                    cats.FirstOrDefault(m => m.CategoryId == masterCat && m.CategoryId != 0);
                                if (categoryDetails != null)
                                {
                                    var rootCategory = new SelectListItem
                                    {
                                        Value = categoryDetails.CategoryId.ToString(),
                                        Text = categoryDetails.Name,
                                        Selected = (categoryDetails.CategoryId == id),
                                        Group = newRootGroup
                                    };

                                    categoryList.Add(rootCategory);
                                }
                            }
                        }

                        cats = cats.Where(m => masterCatIds.Contains(m.RootCategoryId)).ToList();

                        var selectedCategories = new List<ListItem>();
                        foreach (var category in cats)
                        {
                            var newItem = new ListItem();
                            var groupName = string.Empty;
                            if (category.RootCategoryId == uniformCategory)
                            {
                                groupName = "SERVICES - UNIFORMS";
                            }
                            else
                            {
                                var groupDetails = allCategory.FirstOrDefault(m => m.CategoryId == category.RootCategoryId);
                                if (groupDetails != null)
                                {
                                    groupName = groupDetails.Name;
                                }
                            }

                            if (!string.IsNullOrEmpty(groupName) && !groupList.Contains(groupName))
                            {
                                var newGroup = new SelectListGroup { Name = groupName };
                                categoryList.Add(new SelectListItem
                                {
                                    Value = category.CategoryId.ToString(),
                                    Text = category.Name,
                                    Selected = (category.CategoryId == id),
                                    Group = newGroup
                                });
                            }
                            else
                            {
                                categoryList.Add(new SelectListItem
                                {
                                    Value = category.CategoryId.ToString(),
                                    Text = category.Name,
                                    Selected = (category.CategoryId == id),
                                    Group = categoryList.FirstOrDefault(m => m.Group.Name.Equals(groupName))?.Group
                                });
                            }
                        }

                        if (categoryList.Any())
                        {
                            categoryList = categoryList.Where(m => m.Text != "All Catagories").ToList();
                        }

                        var response = new SelectList(categoryList, "Value", "Text", "Group.Name", -1);
                        return response;

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return new SelectList(new List<SelectListItem>());
        }

        public static List<SelectListItem> GetRootCategories(long? id)
        {
            var response = new List<SelectListItem>();
            try
            {
                using (var db = new DBEntities())
                {
                    response.Add(new SelectListItem
                    {
                        Text = "Select Root Category",
                        Value = ""
                    });

                    var cats = db.Categories.Where(m => m.IsActive && !m.Name.Contains("One Size")).ToList();
                    if (cats.Any())
                    {
                        response.AddRange(cats.Select(p => new SelectListItem
                        {
                            Value = p.CategoryId.ToString(),
                            Text = p.Name,
                            Selected = (p.CategoryId == id)
                        }).ToList());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return response;
        }

        public static List<SelectListItem> GetBrand(long? id)
        {
            var response = new List<SelectListItem>();
            try
            {
                using (var db = new DBEntities())
                {
                    response.Add(new SelectListItem
                    {
                        Text = "Select Brand",
                        Value = ""
                    });

                    response.AddRange(db.Brands.Select(p => new SelectListItem
                    {
                        Value = p.BrandId.ToString(),
                        Text = p.Name,
                        Selected = (p.BrandId == id)
                    }).ToList());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return response;
        }

        public static List<SelectListItem> GetSize(long catId, long sizeId = 0)
        {
            var response = new List<SelectListItem>();
            try
            {
                using (var db = new DBEntities())
                {
                    response.Add(new SelectListItem
                    {
                        Text = "Select Size",
                        Value = ""
                    });

                    response.Add(new SelectListItem
                    {
                        Text = "One Size",
                        Value = "0"
                    });

                    response.AddRange(db.Sizes.Where(m => m.CategoryId == catId && m.CategoryId != 0).Select(p => new SelectListItem
                    {
                        Value = p.SizeId.ToString(),
                        Text = p.Name,
                        Selected = (p.SizeId == sizeId)
                    }).ToList());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return response;
        }

        public static List<ColorVm> GetColor()
        {
            var response = new List<ColorVm>();
            try
            {
                using (var db = new DBEntities())
                {
                    response.Add(new ColorVm
                    {
                        Hex = string.Empty,
                        Name = "Select Color",
                        ColorId = 0
                    });

                    response.AddRange(db.Colors.Select(p => new ColorVm
                    {
                        ColorId = p.ColorId,
                        Hex = p.Hex,
                        Name = p.Name
                    }).ToList());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return response;
        }

        public static List<Category> GetCategoriesMaster()
        {
            List<Category> response;

            try
            {
                using (var db = new DBEntities())
                {
                    response = db.Categories.Where(s => s.IsActive && s.CategoryId != 0).OrderByDescending(p => p.OrderNo.HasValue).ThenBy(p => p.OrderNo).ThenByDescending(m => m.OrderNo).ThenBy(m => m.RootCategoryId).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return response;
        }

        public static List<ProductVm> GetProductsVms(long? productId, long? categoryId)
        {
            var response = new List<ProductVm>();

            try
            {
                using (var db = new DBEntities())
                {
                    var product = db.Products.Where(s => s.IsActive).ToList();

                    if (product.Any() && categoryId != null && categoryId > 0)
                    {
                        //Check subcat is Available
                        var subCatId = db.Categories.Where(s => s.RootCategoryId == categoryId).Select(s => s.CategoryId).ToList();

                        if (subCatId.Any())
                        {
                            var tempData = new List<Product>();

                            foreach (var item in subCatId)
                            {
                                tempData.AddRange(product.Where(s => s.CategoryId == item).ToList());
                            }

                            product = product.Where(s => s.CategoryId == categoryId.Value).ToList();

                            if (tempData.Any())
                            {
                                product.AddRange(tempData);
                            }
                        }
                        else
                        {
                            product = product.Where(s => s.CategoryId == categoryId.Value).ToList();
                        }
                    }

                    if (product.Any() && productId != null && productId > 0)
                    {
                        product = product.Where(s => s.ProductId == productId.Value).ToList();
                    }

                    if (product.Any())
                    {
                        var offers = db.Offers.Where(m => m.OfferId != 0).ToList();
                        response = product.Select(s => new ProductVm
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
                        }).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return response;
        }

        public static List<ProductVm> GetOfferProductsVms(long? offerId)
        {
            var response = new List<ProductVm>();

            try
            {
                using (var db = new DBEntities())
                {
                    var product = db.Products.Where(s => s.IsActive && s.OfferId != null && s.OfferId > 0).ToList();

                    if (product.Any() && offerId != null && offerId > 0)
                    {
                        product = product.Where(s => s.OfferId == offerId).ToList();
                    }

                    if (product.Any())
                    {
                        var offers = db.Offers.Where(m=> m.OfferId != 0).ToList();
                        response = product.Select(s => new ProductVm
                        {
                            OfferTitle = (s.OfferId != 0)? offers.FirstOrDefault(m=> m.OfferId == s.OfferId)?.Title: string.Empty,
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
                        }).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return response;
        }

        public static List<ProductVm> GetNewArrivalsProductsVms(long? productId, long? categoryId)
        {
            var response = new List<ProductVm>();

            try
            {
                using (var db = new DBEntities())
                {
                    var productList = db.Products.Where(s => s.IsActive).ToList();
                    var products = new List<Product>();
                    //if (productList.Any() && categoryId != null && categoryId > 0)
                    //{
                    //    //Check subcat is Available
                    //    var subCatId = db.Categories.Where(s => s.RootCategoryId == categoryId).Select(s => s.CategoryId).ToList();

                    //    if (subCatId.Any())
                    //    {
                    //        var tempData = new List<Product>();

                    //        foreach (var item in subCatId)
                    //        {
                    //            tempData.AddRange(productList.Where(s => s.CategoryId == item).ToList());
                    //        }

                    //        productList = productList.Where(s => s.CategoryId == categoryId.Value).ToList();

                    //        if (tempData.Any())
                    //        {
                    //            productList.AddRange(tempData);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        productList = productList.Where(s => s.CategoryId == categoryId.Value).ToList();
                    //    }
                    //}

                    if (productList.Any())
                    {
                        var womencat = productList.Where(s => s.Category.RootCategoryId == 2).OrderByDescending(m => m.CreatedDate).Take(6).ToList();
                        var mencat = productList.Where(s => s.Category.RootCategoryId == 3).OrderByDescending(m => m.CreatedDate).Take(6).ToList();
                        var kidscat = productList.Where(s => s.Category.RootCategoryId == 4).OrderByDescending(m => m.CreatedDate).Take(6).ToList();
                        var servicecat = productList.Where(s => s.Category.RootCategoryId == 5).OrderByDescending(m => m.CreatedDate).Take(6).ToList();
                        if (womencat.Any())
                        {
                            products.AddRange(womencat);
                        }
                        if (mencat.Any())
                        {
                            products.AddRange(mencat);
                        }
                        if (kidscat.Any())
                        {
                            products.AddRange(kidscat);
                        }
                        if (servicecat.Any())
                        {
                            products.AddRange(servicecat);
                        }
                    }

                    if (products.Any())
                    {
                        var offers = db.Offers.Where(m => m.OfferId != 0).ToList();
                        response = products.Select(s => new ProductVm
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
                            CreatedDate = s.CreatedDate,
                        }).OrderByDescending(p => p.CreatedDate).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return response;
        }

        public static int GetCurrentCartItems()
        {
            var response = 0;

            try
            {
                using (var db = new DBEntities())
                {
                    var cartCookie = CookieHelper.Get(StaticValues.CookieNameCartCookie);

                    if (!string.IsNullOrWhiteSpace(cartCookie))
                    {
                        response = db.CartDetails.Count(s => s.CartCookie == cartCookie);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return response;
        }

        public static List<SelectListItem> GetVideoCategory(long catId = 0)
        {
            var response = new List<SelectListItem>();
            try
            {
                using (var db = new DBEntities())
                {
                    response.Add(new SelectListItem
                    {
                        Text = "Select Video Category",
                        Value = ""
                    });

                    response.AddRange(db.Categories.Where(m => m.RootCategoryId.ToString() == StaticValues.STUDIOCategory).Select(p => new SelectListItem
                    {
                        Value = p.CategoryId.ToString(),
                        Text = p.Name,
                        Selected = (p.CategoryId == catId)
                    }).ToList());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return response;
        }

        public static List<FileManagerVm> GetFileManager(long id)
        {
            try
            {
                using (var _db = new DBEntities())
                {
                    var fileManagers = _db.FileManagers.Where(m => m.typeId == id).Select(p => new FileManagerVm
                    {
                        ImageName = p.ImageName,
                        typeId = p.typeId,
                        ImageId = p.ImageId
                    }).ToList();

                    if (!fileManagers.Any())
                    {
                        fileManagers = new List<FileManagerVm>();
                    }

                    return fileManagers;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static List<SelectListItem> GetOfferList(long? offerid, bool isDefaultPrevent = false)
        {
            var response = new List<SelectListItem>();
            try
            {
                using (var db = new DBEntities())
                {
                    var collection = db.Offers.ToList();
                    if (isDefaultPrevent)
                    {
                        collection = collection.Where(m => m.OfferId != 0).ToList();
                    }
                    var offers = collection.Select(m => new SelectListItem
                    {
                        Text = m.Title,
                        Value = m.OfferId.ToString(),
                        Selected = (m.OfferId == offerid)
                    }).ToList();

                    if (offers.Any())
                    {
                        response = offers;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return response;
        }

        public static List<SelectListItem> GetDiscountTypes(long? id)
        {
            var response = new List<SelectListItem>();
            try
            {
                using (var db = new DBEntities())
                {
                    response.Add(new SelectListItem
                    {
                        Text = "Select Discount Type",
                        Value = ""
                    });

                    response.AddRange(db.DiscountTypes.Select(p => new SelectListItem
                    {
                        Value = p.DiscountId.ToString(),
                        Text = p.Type,
                        Selected = (p.DiscountId == id)
                    }).ToList());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return response;
        }

        public static List<AdvertisementVm> GetAdvertisements()
        {
            var response = new List<AdvertisementVm>();

            try
            {
                using (var _db = new DBEntities())
                {
                    response = _db.Advertisements.Select(m => new AdvertisementVm
                    {
                        LinkUrl = m.LinkUrl,
                        HImageUrl = m.HImageUrl,
                        VImageUrl = m.VImageUrl,
                        AdvertisementId = m.AdvertisementId
                    }).OrderByDescending(m => m.AdvertisementId).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return response;
        }
    }
}