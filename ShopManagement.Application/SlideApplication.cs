using System.Collections.Generic;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Application
{
  public  class SlideApplication:ISlideApplication
  {
      private readonly IFileUploader _fileUploader;
      private readonly ISlideRepository _slideRepository;

      public SlideApplication(ISlideRepository slideRepository, IFileUploader fileUploader)
      {
          _slideRepository = slideRepository;
          _fileUploader = fileUploader;
      }

      public OperationResult Create(CreateSlide command)
      {
          var operation = new OperationResult();

          var path ="slides";
          var pictureName = _fileUploader.Upload(command.Picture, path);


            var Slide = new Slide(pictureName, command.PictureAlt, command.PictureTitle, command.Heading,
              command.PictureTitle, command.Text, command.BtnText);
            _slideRepository.Create(Slide);
            _slideRepository.Save();
            return operation.Succeeded();
      }

        public OperationResult Edit(EditSlide command)
        {
            var operation = new OperationResult();
            var path = "slides";
            var pictureName = _fileUploader.Upload(command.Picture, path);
            var slide = _slideRepository.Get(command.Id);
            if (slide == null)
                operation.Failed(ApplicationMessages.RecordNotFound);
            slide.Edit(pictureName, command.PictureAlt, command.PictureTitle, command.Heading,
                command.PictureTitle, command.Text, command.BtnText);
            _slideRepository.Save();
            return operation.Succeeded();
        }

        public OperationResult Remove(long id)
        {
            var operation = new OperationResult();
            var slide = _slideRepository.Get(id);
            if (slide == null)
                operation.Failed(ApplicationMessages.RecordNotFound);
            slide.Remove();
            _slideRepository.Save();
            return operation.Succeeded();
        }

        public OperationResult Restore(long id)
        {
            var operation = new OperationResult();
            var slide = _slideRepository.Get(id);
            if (slide == null)
                operation.Failed(ApplicationMessages.RecordNotFound);
            slide.Restore();
            _slideRepository.Save();
            return operation.Succeeded();
        }

        public EditSlide GetDetails(long id)
        {
            return _slideRepository.GetDetails(id);
        }

        public List<SlideViewModel> GetList()
        {
            return _slideRepository.GetList();
        }
    }
}
