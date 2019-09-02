using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ECommerce_Shop.Helpers
{
    public static class Utilities
    {
        public static void CopyProperties<TSelf, TSource>(this TSelf self, TSource source)
        {
            try
            {
                var sourceAllProperties = source.GetType().GetProperties();

                foreach (var sourceProperty in sourceAllProperties)
                {
                    var selfProperty = self.GetType().GetProperty(sourceProperty.Name);
                    if (selfProperty == null) continue;
                    var sourceValue = sourceProperty.GetValue(source, null);
                    selfProperty.SetValue(self, sourceValue, null);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string SaveFile(HttpPostedFileBase file, string virtualPath, string physicalPath, string filePath)
        {
            try
            {
                //Check directory exis or not
                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }

                //Check file is exist or not  
                if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
                {
                    //Delete Existing file
                    File.Delete(filePath);
                }

                //Save new image and update user data
                var filename = string.Concat(Guid.NewGuid(), Path.GetExtension(file.FileName));
                var savePath = Path.Combine(physicalPath, filename);
                file.SaveAs(savePath);

                return filename;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }
}