using Diacritics.Extensions;
using System.Net;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace BlogMVC.Helpers
{
    public static class Uitilities
    {

        public static int PAGE_SIZE = 10;

        public static int GetFileSize(string urlFile)
        {
            int sizeFile = 0;
            try
            {
                Uri uriPath = new Uri(urlFile);
                var webRequest = HttpWebRequest.Create(uriPath);
                webRequest.Method = "HEAD";
                using (var response = webRequest.GetResponse())
                {
                    var fileSize = response.Headers.Get("Content-Length");
                    var fileSizeInMegaByte = Math.Round(Convert.ToDouble(fileSize) / 1024.0 / 1024.0, 2);
                    sizeFile = Convert.ToInt32(fileSizeInMegaByte);
                }

            }
            catch
            {
                return sizeFile;
            }

            return sizeFile;
        }

        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                return string.Join(" ", words);
            }
            return result;
        }

        public static bool KiemTraHoVaTen(string input)
        {
            bool result = false;
            try
            {
                Match match = Regex.Match(input, @"(\d+)");
                if (match.Success)
                {
                    var number = int.Parse(match.Groups[1].Value);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return result;
        }

        public static bool IsInterger(string str)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            try
            {
                if (String.IsNullOrWhiteSpace(str))
                {
                    return false;
                }
                if (!regex.IsMatch(str))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        public static bool IsNumber(string aNumber)
        {
            BigInteger temp_big_int;
            var is_number = BigInteger.TryParse(aNumber, out temp_big_int);
            return is_number;
        }

        public static string GetDomain(string url)
        {
            string host = "";
            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    Uri myUri = new Uri(url.Trim().ToLower());
                    host = myUri.Host.ToLower();
                }
            }
            catch
            {
                host = "";
            }
            return host;
        }

        public static string RemoveLinks(string html)
        {
            try
            {
                if (!string.IsNullOrEmpty(html))
                {
                    Regex r = new Regex(@"\<a href=.*?\>");
                    html = r.Replace(html, "");
                    r = new Regex(@"\</a\>");
                    html = r.Replace(html, "");
                }
                return html;
            }
            catch
            {
                return html;
            }
        }

        public static string RemoveLinksStyle(string html)
        {
            try
            {
                if (!string.IsNullOrEmpty(html))
                {
                    Regex r = new Regex(@"\sstyle="".*?\""");
                    html = r.Replace(html, "");
                }
                return html;
            }
            catch
            {
                return html;
            }
        }

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string Right(string value, int length)
        {
            return value.Substring(value.Length - length);
        }


        public static string SEOUrl(string url) // convert chao ban --> chao-ban
        {
            url = url.RemoveDiacritics().ToLower();
            string pattern = @"\W+";
            url = Regex.Replace(url, pattern, "-");
            return url;
        }

        public static string GetRandomKey(int length = 5)
        {
            string pattern = @"0123456789zxcvbnmádfghjklqwertyuiop[]{}:~!@#$%^&*()+";
            Random rd = new Random();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(pattern[rd.Next(0, pattern.Length)]);
            }
            return stringBuilder.ToString();

        }

        public static async Task<string> UploadFileToFolderImages(Microsoft.AspNetCore.Http.IFormFile file)
        {
            string folderIamges = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            string path = Path.Combine(folderIamges, file.FileName);

            var supportedFileTypes = new[] { "jpg", "jpeg", "png", "gif" };
            var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
            if (supportedFileTypes.Contains(fileExt.ToLower()))
            {
                await file.CopyToAsync(new FileStream(path, FileMode.Create));
            }

            return file.FileName;

        }

        public static async Task<string> UploadFileToFolderIcon(Microsoft.AspNetCore.Http.IFormFile file, string name)
        {
            string folderIamges = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "icon");

            string path = Path.Combine(folderIamges, file.FileName);

            var supportedFileTypes = new[] { "jpg.", "jpeg", "png", "gif" };
            var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
            if (supportedFileTypes.Contains(fileExt.ToLower()))
            {
                await file.CopyToAsync(new FileStream(path, FileMode.Create));
            }

            return path;

        }

        public static async Task<string> UploadFileToFolderCover(Microsoft.AspNetCore.Http.IFormFile file, string name)
        {
            string folderIamges = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cover");

            string path = Path.Combine(folderIamges, file.FileName);

            var supportedFileTypes = new[] { "jpg.", "jpeg", "png", "gif" };
            var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
            if (supportedFileTypes.Contains(fileExt.ToLower()))
            {
                await file.CopyToAsync(new FileStream(path, FileMode.Create));
            }

            return path;

        }

        public static async Task<string> UpdateFileToFolderImages(Microsoft.AspNetCore.Http.IFormFile file, string? cutFileNameImages)
        {
            string folderIamges = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            // hàm xóa file cũ hoặc tên k đúng định dạng
            string pathD = Path.Combine(folderIamges, cutFileNameImages);

            System.IO.File.Delete(pathD);

            //hàm thêm file mới
            string pathA = Path.Combine(folderIamges, file.FileName);

            var supportedFileTypes = new[] { "jpg", "jpeg", "png", "gif" };
            var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
            if (supportedFileTypes.Contains(fileExt.ToLower()))
            {
                await file.CopyToAsync(new FileStream(pathA, FileMode.Create));
            }

            return file.FileName;

        }

        public static async Task<string> UploadFileToFolderThumbCate(IFormFile file)
        {
            string folderIamges = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ThumpCates");
            string path = Path.Combine(folderIamges, file.FileName);

            var supportedFileTypes = new[] { "jpg", "jpeg", "png", "gif" };
            var fileExt = Path.GetExtension(file.FileName).Substring(1);
            if (supportedFileTypes.Contains(fileExt.ToLower()))
            {
                await file.CopyToAsync(new FileStream(path, FileMode.Create));
            }

            return file.FileName;

        }

        public static async Task<string> UploadFileToVideo(IFormFile file)
        {
            string folderIamges = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Video");
            string path = Path.Combine(folderIamges, file.FileName);

            var supportedFileTypes = new[] { "mp4" };
            var fileExt = Path.GetExtension(file.FileName).Substring(1);
            if (supportedFileTypes.Contains(fileExt.ToLower()))
            {
                await file.CopyToAsync(new FileStream(path, FileMode.Create));
            }

            return file.FileName;

        }

        public static async Task<List<String>> UploadMutifileFileToFolderThumPros(List<IFormFile> mutiFile)
        {
            List<string> lstThumbName = new List<string>();
            string folderIamges = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ThumbPros");
            foreach (var item in mutiFile)
            {
                string path = Path.Combine(folderIamges, item.FileName);

                var supportedFileTypes = new[] { "jpg", "jpeg", "png", "gif" };
                var fileExt = System.IO.Path.GetExtension(item.FileName).Substring(1);
                if (supportedFileTypes.Contains(fileExt.ToLower()))
                {
                    await item.CopyToAsync(new FileStream(path, FileMode.Create));
                }
                lstThumbName.Add(item.FileName);
            }
            return lstThumbName;
        }


        public static void DeleteFileImages(string? cutFileNameImages)
        {
            string folderIamges = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            // hàm xóa file cũ hoặc tên k đúng định dạng
            string pathD = Path.Combine(folderIamges, cutFileNameImages);

            System.IO.File.Delete(pathD);
        }


    }
}
