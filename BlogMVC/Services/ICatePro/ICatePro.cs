using AutoMapper;
using BlogMVC.Areas.Admin.Models;
using BlogMVC.Helpers;
using BlogMVC.Models;

namespace BlogMVC.Services.ICatePro
{
    public interface ICatePro
    {
        Task CreateCateProAsync(CateProductViewModel model);
    }
    public class RepoCatePro : ICatePro
    {
        private readonly DbBlogContext _db;
        private readonly IMapper _mapper;
        public RepoCatePro(DbBlogContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task CreateCateProAsync(CateProductViewModel model)
        {
            model.Alias = Uitilities.SEOUrl(model.CatName);
            if (model.ParentId == null)
            {
                model.Level = 1;
            }
            else
            {
                model.Level = model.ParentId == 0 ? 1 : 2;
            }

            if (model.fThumb != null)
            {
                string extension = Path.GetExtension(model.fThumb.FileName);
                var up = await Uitilities.UploadFileToFolderThumbCate(model.fThumb);
                model.Thumb = up;
            }
            else if (model.fCover != null)
            {
                string extension = Path.GetExtension(model.fCover.FileName);
                var up = await Uitilities.UploadFileToFolderImages(model.fCover);
                model.Cover = up;   
            }
            await _db.CateProducts.AddAsync(_mapper.Map<CateProduct>(model));
            await _db.SaveChangesAsync();
        }
    }
}
