//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MultimediaApp
//{
//    public class PictureCollection
//    {
//        // Main collection
//        private List<Picture> _collectionList = new List<Picture>();
//        // Editable collection, showed on gallery
//        //private List<Meme> _cacheList = new List<Meme>();

//        public PictureCollection()
//        {

//        }

//        public PictureCollection(List<Picture> Pictures)
//        {
//            _collectionList = Pictures;
//            if (_collectionList != null)
//            {
//                for (int i = 0; i < _collectionList.Count; i++)
//                {
//                    _collectionList[i].Id = i;
//                }
//            }
//            UniqueCategories();
//        }

//        // Бизнес-логика Создателя может повлиять на его внутреннее состояние.
//        // Поэтому клиент должен выполнить резервное копирование состояния с
//        // помощью метода save перед запуском методов бизнес-логики.
//        #region Body
//        private void UniqueCategories()
//        {
//            List<string> categories = new List<string>();
//            for (int i = 0; i < _collectionList.Count; i++)
//                categories.Add(_collectionList[i].Category);
//            _uniqueCategories = categories.Distinct().ToList();
//        }

//        public List<string> GetUniqueCategories()
//        {
//            return _uniqueCategories;
//        }

//        public string GetLastCategory()
//        {
//            return _uniqueCategories.Last();
//        }

//        public List<Picture> GetCollection()
//        {
//            return _collectionList;
//        }

//        public Picture GetById(int Id)
//        {
//            Picture result = new Picture();
//            for (int i = 0; i < _collectionList.Count; i++)
//            {
//                if (_collectionList[i].Id == Id)
//                    result = _collectionList[i];
//            }

//            return result;
//        }

//        delegate void AddEventHandler();
//        event AddEventHandler AddCat;
//        public void Add(Picture Item)
//        {
//            int newId = _collectionList.Count; // Id counting starts from 0
//            Item.Id = newId;
//            _collectionList.Add(Item);

//            CategoryEnumerator.GetOther();

//            //if (!_uniqueCategories.Contains(Item.Category))
//            //{
//            //    _uniqueCategories.Add(Item.Category);
//            //}
//        }

//        public void RemoveAt(int num)
//        {
//            _collectionList.RemoveAt(num);
//            for (int i = num; num < _collectionList.Count - 1; i++)
//            {
//                if (!(i == _collectionList.Count - 1))
//                {
//                    _collectionList[i + 1].Id = i;
//                }
//                else
//                    return;
//            }
//        }

//        public void AddRange(List<Picture> List)
//        {
//            int j = 0;
//            for (int i = _collectionList.Count - 1; i < _collectionList.Count - 1 + List.Count; i++)
//                List[j++].Id = i;
//            _collectionList.AddRange(List);
//        }

//        public void AddRange(PictureCollection Collection)
//        {
//            int j = 0;
//            for (int i = _collectionList.Count - 1; i < _collectionList.Count - 1 + Collection.GetCollection().Count; i++)
//                Collection.GetCollection()[j++].Id = i;
//            _collectionList.AddRange(Collection.GetCollection());
//        }


//        // Create Categories list

//        #endregion

//        #region CatEnum
//        //private List<string> _uniqueCategories = new List<string>();
//        static internal class CategoryEnumerator
//        {
//            static public void GetSome()
//            {

//            }
            
//            static public void GetOther()
//            {

//            }
//        }

//        #endregion

//        #region Memento

//        // Сохраняет текущее состояние внутри снимка.
//        public IMemento Save()
//        {
//            return new CollectionMemento(this._collectionList, this._uniqueCategories);
//        }

//        // Восстанавливает состояние Создателя из объекта снимка.
//        public void Restore(IMemento memento)
//        {
//            if (!(memento is CollectionMemento))
//            {
//                throw new Exception("Unknown memento class " + memento.ToString());
//            }

//            Tuple<List<Picture>, List<string>> tuple = memento.GetState();
//            //this._collectionList = memento.GetState();
//            //this._uniqueCategories = 
//            //Console.Write($"Originator: My state has changed to: {_collection}");
//        }
    
//        #endregion
//    }
//}