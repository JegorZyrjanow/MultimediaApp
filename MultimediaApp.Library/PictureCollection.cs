using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MultimediaApp.Library
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

        public PictureCollection()
        {

        }

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

        delegate void AddEventHandler();
        event AddEventHandler AddCat;
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
        public IMemento Save()
        {
            return new CollectionMemento(this._collectionList);
        }

        // Восстанавливает состояние Создателя из объекта снимка.
        public void Restore(IMemento memento)
        {
            if (!(memento is CollectionMemento))
            {
                throw new Exception("Unknown memento class " + memento.ToString());
            }

            this._collectionList = memento.GetState();
            //this._uniqueCategories = 
            //Console.Write($"Originator: My state has changed to: {_collection}");
        }

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

    internal class CategoryEnumerator
    {

    }

    class CollectionMemento : IMemento
    {
        private ObservableCollection<Picture> _collection = new ObservableCollection<Picture>();

        private DateTime _date; // МОЖНО ВЫВОДИТЬ ПОСЛЕДНЮЮ ДАТУ ИЗМЕНЕНИЯ

        public CollectionMemento(ObservableCollection<Picture> MainList)
        {
            this._collection = MainList;
            this._date = DateTime.Now;
        }

        // Создатель использует этот метод, когда восстанавливает своё
        // состояние.
        public ObservableCollection<Picture> GetState()
        {
            return this._collection;
        }

        // Остальные методы используются Опекуном для отображения метаданных.
        public string GetName()
        {
            return $"{this._date} / ({this._collection[0].Name}, {this._collection[1].Name}, {this._collection[2].Name})...";
            // МОЖНО ВЫВОДИТЬ ИМЯ СОХРАНЕННОЙ КОЛЛЕКЦИИ ИЛИ НАЗВАНИЯ ПЕРВЫХ КАРТИНОК КАК АЛБТЕРНАТИВА
        }

        public DateTime GetDate()
        {
            return this._date;
        }
    }

    public class Caretaker
    {
        private List<IMemento> _mementos = new List<IMemento>();

        private PictureCollection _collection;

        public Caretaker(PictureCollection Collection)
        {
            this._collection = Collection;
        }

        public void Backup()
        {
            //Console.WriteLine("\nCaretaker: Saving Originator's state...");
            this._mementos.Add(this._collection.Save());
        }

        public void Undo()
        {
            if (this._mementos.Count == 0)
            {
                return;
            }

            var memento = this._mementos.Last();
            this._mementos.Remove(memento);

            //Console.WriteLine("Caretaker: Restoring state to: " + memento.GetName());

            try
            {
                this._collection.Restore(memento);
            }
            catch (Exception)
            {
                this.Undo();
            }
        }

        public void ShowHistory()
        {
            //Console.WriteLine("Caretaker: Here's the list of mementos:");

            foreach (var memento in this._mementos)
            {
                //Console.WriteLine(memento.GetName());
            }
        }


    }
}
