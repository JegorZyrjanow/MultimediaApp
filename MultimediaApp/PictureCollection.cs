using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MultimediaApp
{
    public class PictureCollection : INotifyPropertyChanged
    {
        #region PropertyChangedEventHandler

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        private ObservableCollection<Picture> _collectionList = new ObservableCollection<Picture>();

        public PictureCollection(ObservableCollection<Picture> Pictures)
        {
            _collectionList = Pictures;
            if (_collectionList != null)
            {
                for (int i = 0; i < _collectionList.Count; i++)
                {
                    _collectionList[i].Id = i;
                }
            }
        }

        // Бизнес-логика Создателя может повлиять на его внутреннее состояние.
        // Поэтому клиент должен выполнить резервное копирование состояния с
        // помощью метода save перед запуском методов бизнес-логики.
        #region Body

        public ObservableCollection<Picture> GetCollection()
        {
            return _collectionList;
        }

        public Picture GetById(int Id)
        {
            Picture result = new Picture();
            for (int i = 0; i < _collectionList.Count; i++)
            {
                if (_collectionList[i].Id == Id)
                    result = _collectionList[i];
            }

            return result;
        }

        public void Add(Picture Item)
        {
            int newId = _collectionList.Count; // Id counting starts from 0
            Item.Id = newId;
            _collectionList.Add(Item);

            CategoryEnumerator.GetOther();

            //if (!_uniqueCategories.Contains(Item.Category))
            //{
            //    _uniqueCategories.Add(Item.Category);
            //}
        }

        public void RemoveAt(int num)
        {
            _collectionList.RemoveAt(num);
            for (int i = num; num < _collectionList.Count - 1; i++)
            {
                if (!(i == _collectionList.Count - 1))
                {
                    _collectionList[i + 1].Id = i;
                }
                else
                    return;
            }
        }

        //public void AddRange(ObservableCollection<Picture> List)
        //{
        //    int j = 0;
        //    for (int i = _collectionList.Count - 1; i < _collectionList.Count - 1 + List.Count; i++)
        //        List[j++].Id = i;
        //    _collectionList.AddRange(List);
        //}

        //public void AddRange(PictureCollection Collection)
        //{
        //    int j = 0;
        //    for (int i = _collectionList.Count - 1; i < _collectionList.Count - 1 + Collection.GetCollection().Count; i++)
        //        Collection.GetCollection()[j++].Id = i;
        //    _collectionList.AddRange(Collection.GetCollection());
        //}


        // Create Categories list

        #endregion

        #region CatEnum
        //private List<string> _uniqueCategories = new List<string>();
        static internal class CategoryEnumerator
        {
            static public void GetSome()
            {

            }

            static public void GetOther()
            {

            }
        }

        #endregion

        #region Memento

        // Сохраняет текущее состояние внутри снимка.

        #endregion
    }

    //internal class CollectionEnumerator
    //{
    //    private PictureCollection _collection = new PictureCollection(null);

    //    public PictureCollection SortByName(string Name, PictureCollection Collection)
    //    {
    //        _collection = new PictureCollection();

    //        Collection.GetCollection().ForEach(pic =>
    //        {
    //            if (pic.Name.ToLower().Contains(Name.ToLower()))
    //                _collection.Add(pic);
    //        });

    //        return _collection;
    //    }

    //    public PictureCollection SortByCategory(string Category, PictureCollection Collection)
    //    {
    //        _collection = new PictureCollection();

    //        Collection.GetCollection().ForEach(pic =>
    //        {
    //            if (pic.Category.ToLower().Equals(Category.ToLower()))
    //                _collection.Add(pic);
    //        });

    //        return _collection;
    //    }
    //}


}
