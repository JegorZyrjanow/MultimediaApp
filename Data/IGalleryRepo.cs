using MultimediaApp.Model;
using System.Collections.Generic;

namespace MultimediaApp.Data
{
    internal interface IGalleryRepo
    {
        void Add(Picture pic);
        void Remove(int id);
        //Picture GetByName(string name);
        Picture GetById(int id);
        Picture GetAll(string name);
        List<string> GetTags();
    }
}